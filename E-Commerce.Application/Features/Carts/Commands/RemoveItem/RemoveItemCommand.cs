using E_Commerce.Application.Features.Carts.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Carts.Commands.RemoveItem;

public record RemoveItemCommand(Guid ProductId, Guid CartId) : IRequest<Result<CartSummaryResponse>>;