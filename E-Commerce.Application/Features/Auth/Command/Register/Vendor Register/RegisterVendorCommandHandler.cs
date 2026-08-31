using E_Commerce.Application.Common;
using E_Commerce.Application.Features.Auth.DTOs;
using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace E_Commerce.Application.Features.Auth.Command.Register;

internal sealed class RegisterVendorCommandHandler : IRequestHandler<RegisterVendorCommand, Result<AuthResponse>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IVendorRepository _vendorRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly JWTSettings _jwtSettings;
    private readonly IFileService _fileService;
    private readonly ILogger<RegisterVendorCommandHandler> _logger;

    public RegisterVendorCommandHandler(UserManager<ApplicationUser> userManager, IVendorRepository vendorRepository, 
        IUnitOfWork unitOfWork, ITokenService tokenService, IOptionsSnapshot<JWTSettings> jwtSettings, IFileService fileService,
        ILogger<RegisterVendorCommandHandler> logger)
    {
        _userManager = userManager;
        _vendorRepository = vendorRepository;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _jwtSettings = jwtSettings.Value;
        _fileService = fileService;
        _logger = logger;
    }

    public async Task<Result<AuthResponse>> Handle(RegisterVendorCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if(existingUser is not null) return Result<AuthResponse>.Failure(ApplicationUserErrors.EmailAlreadyExists);

        var isUserNameUnique = await _userManager.FindByNameAsync(request.UserName) == null;
        if (!isUserNameUnique) return Result<AuthResponse>.Failure(ApplicationUserErrors.UserNameAlreadyExists);

        var isStoreNameUnique = await _vendorRepository.IsStoreNameUniquenessAsync(request.StoreName, cancellationToken);
        if(!isStoreNameUnique) return Result<AuthResponse>.Failure(VendorErrors.DuplicateStoreName);

        var isCRNUnique = await _vendorRepository.IsCommercialRegistrationNumberUniquenessAsync(request.CommercialRegistrationNumber, cancellationToken);
        if(!isCRNUnique) return Result<AuthResponse>.Failure(VendorErrors.DuplicateCommercialRegistrationNumber);

        string? imageUploadUrl = string.Empty;
        if (request.Image is not null)
        {
            imageUploadUrl = await _fileService.UploadImageAsync(request.Image);
            if (string.IsNullOrEmpty(imageUploadUrl))
            {
                _logger.LogError("INFRASTRUCTURE ERROR: Image upload failed during user registration for Email: {Email}", request.Email);
                return Result<AuthResponse>.Failure(ApplicationUserErrors.UploadImageFaild); 
            }
        }

        var user = ApplicationUser.Create(request.FirstName, request.MiddleName, request.LastName,
            request.Email, request.UserName, request.PhoneNumber, string.IsNullOrWhiteSpace(imageUploadUrl)? null : imageUploadUrl, request.DateOfBirth);

        if(user.IsFailure) return Result<AuthResponse>.Failure(user.Error);

        var userValue = user.Value!;
        var result = await _userManager.CreateAsync(userValue, request.Password);

        if (!result.Succeeded)
        {
            var firstError = result.Errors.First();
            return Result<AuthResponse>.Failure(new Error(firstError.Code, firstError.Description));
        }   

        var roleResult = await _userManager.AddToRoleAsync(userValue, AppRoles.Vendor);

        if (!roleResult.Succeeded)
        {
            var firstError = result.Errors.First();
            return Result<AuthResponse>.Failure(new Error(firstError.Code, firstError.Description));
        }

        var vendor = Vendor.Create(request.StoreName, request.CommercialRegistrationNumber, userValue.Id);
        if(vendor.IsFailure) return Result<AuthResponse>.Failure(vendor.Error);

        await _vendorRepository.AddAsync(vendor.Value!, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var accessToken = await _tokenService.GenerateAccessToken(userValue, cancellationToken);
        var refreshToken = _tokenService.GenerateRefreshToken();
        userValue.UpdateRefreshToken(refreshToken, DateTimeOffset.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays));
        await _userManager.UpdateAsync(userValue);


        return Result<AuthResponse>.Success(new AuthResponse(AccessToken: accessToken, RefreshToken: refreshToken));
    }
}
