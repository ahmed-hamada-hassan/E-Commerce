using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Users.Commands.UpdateUser;

internal sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<bool>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UpdateUserCommandHandler> _logger;

    public UpdateUserCommandHandler(UserManager<ApplicationUser> userManager, ILogger<UpdateUserCommandHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null) return Result<bool>.Failure(ApplicationUserErrors.NotFound);


        var updateResult = user.Update(request.FirstName, request.MiddleName, request.LastName, request.Email,
            request.UserName, request.PhoneNumber, request.DateOfBirth);

        if (updateResult.IsFailure) return Result<bool>.Failure(updateResult.Error);

        var identityResult = await _userManager.UpdateAsync(user);

        if (!identityResult.Succeeded)
        {
            var error = identityResult.Errors.First();
            _logger.LogError("Failed to update user with ID {UserId}. Error Code: {ErrorCode}, Description: {ErrorDescription}", 
                request.Id, error.Code, error.Description);
            return Result<bool>.Failure(new Error(error.Code, error.Description));
        }

        return Result<bool>.Success(true);
    }
}
