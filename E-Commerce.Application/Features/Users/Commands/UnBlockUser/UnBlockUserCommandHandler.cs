using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Users.Commands.UnBlockUser;

internal sealed class UnBlockUserCommandHandler : IRequestHandler<UnBlockUserCommand, Result<bool>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UnBlockUserCommandHandler> _logger;

    public UnBlockUserCommandHandler(UserManager<ApplicationUser> userManager, ILogger<UnBlockUserCommandHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(UnBlockUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null) return Result<bool>.Failure(ApplicationUserErrors.NotFound);

        var lockoutEnabled = await _userManager.GetLockoutEnabledAsync(user);
        if (!lockoutEnabled)
        {
            await _userManager.SetLockoutEnabledAsync(user, true);
        }

        var lockoutResult = await _userManager.SetLockoutEndDateAsync(user, null);
        if (!lockoutResult.Succeeded)
        {
            var error = lockoutResult.Errors.First();
            _logger.LogError("Failed to unblock user with ID {UserId}. Error Code: {ErrorCode}, Description: {ErrorDescription}", 
                request.UserId, error.Code, error.Description);
            return Result<bool>.Failure(new Error(error.Code, error.Description));
        }

        await _userManager.UpdateSecurityStampAsync(user);

        return Result<bool>.Success(true);
    }
}
