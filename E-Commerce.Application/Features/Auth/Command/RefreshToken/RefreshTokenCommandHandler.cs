using E_Commerce.Application.Common;
using E_Commerce.Application.Features.Auth.DTOs;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace E_Commerce.Application.Features.Auth.Command.RefreshToken;

internal sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly JWTSettings _jwtSettings;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;

    public RefreshTokenCommandHandler(UserManager<ApplicationUser> userManager, ITokenService tokenService,
        IOptionsSnapshot<JWTSettings> jwtSettings, ILogger<RefreshTokenCommandHandler> logger)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
    }

    public async Task<Result<AuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken, cancellationToken);
        if (user is null || user.RefreshTokenExpiryTime <= DateTimeOffset.UtcNow)
        {
            _logger.LogWarning("SECURITY ALERT: Invalid or expired Refresh Token attempt. Token tampering or theft possible.");
            return Result<AuthResponse>.Failure(ApplicationUserErrors.InvalidRefreshToken);
        }

        var newAccessToken = await _tokenService.GenerateAccessToken(user, cancellationToken);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        var expiryTime = DateTimeOffset.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);
        user.UpdateRefreshToken(newRefreshToken, expiryTime);

        await _userManager.UpdateAsync(user);

        return Result<AuthResponse>.Success(new AuthResponse
        (
            AccessToken: newAccessToken,
            RefreshToken: newRefreshToken
        ));
    }
}