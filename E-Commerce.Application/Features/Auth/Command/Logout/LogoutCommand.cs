using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Auth.Command.Logout;

public record LogoutCommand(Guid UserId) : IRequest<Result<bool>>;