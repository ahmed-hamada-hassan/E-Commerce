using E_Commerce.Application.Features.Carts.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Carts.Commands.UpdateItemQuantity;

public record UpdateItemQuantityCommand(Guid ProductId, Guid CartId, int Quantity) : IRequest<Result<CartSummaryResponse>>;