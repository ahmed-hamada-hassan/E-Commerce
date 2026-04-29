using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Users.Commands.UpdateUser;

internal sealed class UpdateUserImageCommandHandler : IRequestHandler<UpdateUserImageCommand, Result<bool>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UpdateUserCommandHandler> _logger;
    private readonly IFileService _fileService;

    public UpdateUserImageCommandHandler(UserManager<ApplicationUser> userManager, ILogger<UpdateUserCommandHandler> logger,
        IFileService fileService)
    {
        _userManager = userManager;
        _logger = logger;
        _fileService = fileService;
    }

    public async Task<Result<bool>> Handle(UpdateUserImageCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user?.Id != request.UserId)
        {
            _logger.LogWarning("User with id {UserId} attempted to update another user's information {AnotherUserId}.", request.UserId, user?.Id);
            return Result<bool>.Failure(ApplicationUserErrors.AccessDenied);
        }
        if (user == null)
            return Result<bool>.Failure(ApplicationUserErrors.NotFound);

        var imageUrl = request.Image is null ? null : await _fileService.UploadImageAsync(request.Image);
        if (string.IsNullOrEmpty(imageUrl))
        {
            _logger.LogError("Failed to upload image for user with id {UserId}.", request.UserId);
            return Result<bool>.Failure(ApplicationUserErrors.UploadImageFaild);
        }

        var oldImageUrl = user.ImageUrl;

        user.UpdateImage(imageUrl);
        var identityResult = await _userManager.UpdateAsync(user);
        if (!identityResult.Succeeded)
        {
            var error = identityResult.Errors.First();
            _logger.LogError("Failed to update image for user with ID {UserId}. Error Code: {ErrorCode}, Description: {ErrorDescription}",
                request.UserId, error.Code, error.Description);
            return Result<bool>.Failure(new Error(error.Code, error.Description));
        }

        if(!string.IsNullOrWhiteSpace(oldImageUrl)) await _fileService.DeleteImageAsync(oldImageUrl);

        return Result<bool>.Success(true);
    }
}
