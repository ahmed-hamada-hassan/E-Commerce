using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Carts.Commands.AddToCart;

public record AddToCartCommand(Guid ProductId, Guid UserId, int Quantity) : IRequest<Result<bool>>;