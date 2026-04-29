using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace E_Commerce.Application.Features.Users.Commands.UpdateUser;

public record UpdateUserCommand(Guid Id, string? FirstName, string? MiddleName, string? LastName, string? Email, string? UserName,
    string? PhoneNumber, DateOnly? DateOfBirth) : IRequest<Result<bool>>;