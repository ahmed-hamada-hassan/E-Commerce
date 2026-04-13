using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Users.Commands.BlockUser;

internal sealed class BlockUserCommandHandler : IRequestHandler<BlockUserCommand, Result<bool>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<BlockUserCommandHandler> _logger;

    public BlockUserCommandHandler(UserManager<ApplicationUser> userManager, ILogger<BlockUserCommandHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(BlockUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null) return Result<bool>.Failure(ApplicationUserErrors.NotFound);

        var lockoutEnabled = await _userManager.GetLockoutEnabledAsync(user);
        if (!lockoutEnabled)
        {
            await _userManager.SetLockoutEnabledAsync(user, true);
        }

        var lockoutResult = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
        if (!lockoutResult.Succeeded)
        {
            var error = lockoutResult.Errors.First();
            _logger.LogError("Failed to block user. Error Code: {ErrorCode}, Description: {ErrorDescription}", error.Code, error.Description);
            return Result<bool>.Failure(new Error(error.Code, error.Description));
        }

        await _userManager.UpdateSecurityStampAsync(user);

        return Result<bool>.Success(true);
    }
}
