using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Users.Commands.UnBlockUser;

public record UnBlockUserCommand(Guid UserId) : IRequest<Result<bool>>;