using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Carts.Commands.ClearCart;

public record ClearCartCommand() : IRequest<Result<bool>>;