using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Users.Commands.UpdateUser;

public record UpdateUserCommand(Guid Id, string FirstName, string? MiddleName, string LastName, string Email, string UserName,
    string PhoneNumber, string? ImageUrl, DateOnly DateOfBirth) : IRequest<Result<bool>>;
