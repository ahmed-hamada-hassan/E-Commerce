using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Users.Commands.ChangeUserPassword;

internal sealed class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result<bool>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<ChangePasswordCommandHandler> _logger;

    public ChangePasswordCommandHandler(UserManager<ApplicationUser> userManager, ILogger<ChangePasswordCommandHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null) return Result<bool>.Failure(ApplicationUserErrors.NotFound);

        var identityResult = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!identityResult.Succeeded)
        {
            var error = identityResult.Errors.First();
            _logger.LogError("Failed to change password for user with ID {UserId}. Error Code: {ErrorCode}, Description: {ErrorDescription}", 
                request.UserId, error.Code, error.Description);
            return Result<bool>.Failure(new Error(error.Code, error.Description));
        }

        return Result<bool>.Success(true);
    }
}
