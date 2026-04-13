using E_Commerce.Application.Common;
using E_Commerce.Application.Features.Auth.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace E_Commerce.Application.Features.Auth.Command.Login;

internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IVendorRepository _vendorRepository;
    private readonly ITokenService _tokenService;
    private readonly JWTSettings _jwtSettings;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(UserManager<ApplicationUser> userManager, IVendorRepository vendorRepository,
        ITokenService tokenService, IOptionsSnapshot<JWTSettings> jwtSettings, ILogger<LoginCommandHandler> logger)
    {
        _userManager = userManager;
        _vendorRepository = vendorRepository;
        _tokenService = tokenService;
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
    }

    public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) return Result<AuthResponse>.Failure(ApplicationUserErrors.InvalidCredentails);

        if (await _userManager.IsLockedOutAsync(user))
        {
            _logger.LogWarning("BRUTE FORCE ALERT: Locked account attempted login. UserID: {UserId}, Email: {Email}", user.Id, user.Email);
            return Result<AuthResponse>.Failure(ApplicationUserErrors.AccountLocked);
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
        {
            await _userManager.AccessFailedAsync(user);

            if (await _userManager.IsLockedOutAsync(user))
            {
                _logger
                    .LogWarning("BRUTE FORCE ALERT: Account locked out due to max failed attempts. UserID: {UserId}, Email: {Email}", user.Id, user.Email);
                return Result<AuthResponse>.Failure(ApplicationUserErrors.AccountLocked);
            }

            return Result<AuthResponse>.Failure(ApplicationUserErrors.InvalidCredentails);
        }

        await _userManager.ResetAccessFailedCountAsync(user);

        var roles = await _userManager.GetRolesAsync(user);

        Guid? vendorId = null;
        if(roles.Contains(AppRoles.Vendor))
        {
            var vendor = await _vendorRepository.GetByUserIdAsync(user.Id, cancellationToken);
            if(vendor is not null)
            {
                if(!vendor.IsActive)
                    return Result<AuthResponse>.Failure(ApplicationUserErrors.InvalidCredentails);

                vendorId = vendor.Id;
            }
        }

        var accessToken = await _tokenService.GenerateAccessToken(user, cancellationToken);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenExpirationTime = DateTime.UtcNow.AddDays(_jwtSettings.AccessRefreshTokenExpirationInDays);
        user.UpdateRefreshToken(refreshToken, refreshTokenExpirationTime);

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            return Result<AuthResponse>.Failure(new Error(error.Code, error.Description));
        }

        return Result<AuthResponse>.Success(new AuthResponse
        (
            AccessToken: accessToken,
            RefreshToken: refreshToken
        ));
    }
}
