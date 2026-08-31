using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Application.Features.Auth.Command.Logout;

internal sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result<bool>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public LogoutCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<bool>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if(user is null) return Result<bool>.Failure(ApplicationUserErrors.NotFound);

        user.UpdateRefreshToken(string.Empty, DateTimeOffset.UtcNow);

        await _userManager.UpdateAsync(user);

        return Result<bool>.Success(true);
    }
}
