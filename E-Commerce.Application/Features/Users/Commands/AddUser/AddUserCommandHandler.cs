using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Users.Commands.AddUser;

internal sealed class AddUserCommandHandler : IRequestHandler<AddUserCommand, Result<Guid>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<AddUserCommandHandler> _logger;

    public AddUserCommandHandler(UserManager<ApplicationUser> userManager, ILogger<AddUserCommandHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(AddUserCommand request, CancellationToken cancellationToken = default)
    {
        var user = ApplicationUser.Create(request.FirstName, request.MiddleName, request.LastName,
            request.Email, request.UserName, request.PhoneNumber, request.ImageUrl, request.DateOfBirth);

        if (user.IsFailure) return Result<Guid>.Failure(user.Error);

        var newUser = user.Value!;
        var identityResult = await _userManager.CreateAsync(newUser, request.Password);

        if (!identityResult.Succeeded)
        {
            var error = identityResult.Errors.First();
            _logger.LogError("Failed to create user. Error Code: {ErrorCode}, Description: {ErrorDescription}", error.Code, error.Description);
            return Result<Guid>.Failure(new Error(error.Code, error.Description));
        }

        return Result<Guid>.Success(newUser.Id);
    }
}
