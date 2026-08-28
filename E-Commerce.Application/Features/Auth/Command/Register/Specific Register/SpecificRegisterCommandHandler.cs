using E_Commerce.Application.Common;
using E_Commerce.Application.Features.Auth.DTOs;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace E_Commerce.Application.Features.Auth.Command.Register;

internal sealed class SpecificRegisterCommandHandler : IRequestHandler<SpecificRegisterCommand, Result<AuthResponse>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly JWTSettings _jwtSettings;
    private readonly IFileService _fileService;
    private readonly ILogger<SpecificRegisterCommandHandler> _logger;

    public SpecificRegisterCommandHandler(UserManager<ApplicationUser> userManager, ITokenService tokenService, 
        IOptionsSnapshot<JWTSettings> jwtSettings, IFileService fileService, ILogger<SpecificRegisterCommandHandler> logger)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _jwtSettings = jwtSettings.Value;
        _fileService = fileService;
        _logger = logger;
    }

    public async Task<Result<AuthResponse>> Handle(SpecificRegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if(existingUser is not null) return Result<AuthResponse>.Failure(ApplicationUserErrors.EmailAlreadyExists);

        string? imageUploadUrl = string.Empty;
        if (request.Image is not null)
        {
            imageUploadUrl = await _fileService.UploadImageAsync(request.Image);
            if (string.IsNullOrEmpty(imageUploadUrl))
                return Result<AuthResponse>.Failure(ApplicationUserErrors.UploadImageFaild);
        }

        var user = ApplicationUser.Create(request.FirstName, request.MiddleName, request.LastName,
            request.Email, request.UserName, request.PhoneNumber, string.IsNullOrWhiteSpace(imageUploadUrl) ? null : imageUploadUrl, request.DateOfBirth);

        if(user.IsFailure) return Result<AuthResponse>.Failure(user.Error);

        var userValue = user.Value!;
        var result = await _userManager.CreateAsync(userValue, request.Password);

        if (!result.Succeeded)
        {
            var firstError = result.Errors.First();
            return Result<AuthResponse>.Failure(new Error(firstError.Code, firstError.Description));
        }

        var roleResult = await _userManager.AddToRoleAsync(userValue, request.Role);

        if (!roleResult.Succeeded)
        {
            var firstError = result.Errors.First();
            return Result<AuthResponse>.Failure(new Error(firstError.Code, firstError.Description));
        }

        var accessToken = await _tokenService.GenerateAccessToken(userValue, cancellationToken);
        var refreshToken = _tokenService.GenerateRefreshToken();
        userValue.UpdateRefreshToken(refreshToken, DateTimeOffset.UtcNow.AddDays(_jwtSettings.AccessRefreshTokenExpirationInDays));
        await _userManager.UpdateAsync(userValue);

        return Result<AuthResponse>.Success(new AuthResponse(AccessToken: accessToken, RefreshToken: refreshToken));
    }
}
