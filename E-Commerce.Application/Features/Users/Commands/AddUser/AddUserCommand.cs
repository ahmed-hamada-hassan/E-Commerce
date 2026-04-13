using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Users.Commands.AddUser;

public record AddUserCommand(string Password,string FirstName, string? MiddleName, string LastName, string Email, 
    string UserName, string? ImageUrl, string PhoneNumber, DateOnly DateOfBirth) : IRequest<Result<Guid>>;
