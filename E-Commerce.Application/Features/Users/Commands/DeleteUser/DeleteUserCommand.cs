using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Users.Commands.DeleteUser;

public record DeleteUserCommand (Guid Id) : IRequest<Result<bool>>;
