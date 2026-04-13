using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Users.Commands.RestoreUser;

internal sealed class RestoreUserCommandHandler : IRequestHandler<RestoreUserCommand, Result<bool>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<RestoreUserCommandHandler> _logger;

    public RestoreUserCommandHandler(UserManager<ApplicationUser> userManager, IUserRepository userRepository, 
        ILogger<RestoreUserCommandHandler> logger)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(RestoreUserCommand request, CancellationToken cancellationToken)
    {
        var deletedUser = await _userRepository.GetDeletedByIdAsync(request.Id, cancellationToken);
        if (deletedUser is null) return Result<bool>.Failure(ApplicationUserErrors.NotFound);

        deletedUser.Restore();

        var identityResult = await _userManager.UpdateAsync(deletedUser);
        if (!identityResult.Succeeded)
        {
            var error = identityResult.Errors.First();
            _logger.LogError("Failed to restore user with ID {UserId}. Error Code: {ErrorCode}, Description: {ErrorDescription}", 
                request.Id, error.Code, error.Description);
            return Result<bool>.Failure(new Error(error.Code, error.Description));
        }

        return Result<bool>.Success(true);
    }
}
