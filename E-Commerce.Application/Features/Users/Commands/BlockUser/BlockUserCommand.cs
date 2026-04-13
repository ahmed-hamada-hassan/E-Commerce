using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Users.Commands.BlockUser;

public record BlockUserCommand(Guid UserId) : IRequest<Result<bool>>;