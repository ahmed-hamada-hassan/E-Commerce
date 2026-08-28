using E_Commerce.Application.Features.Carts.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Carts.Commands.Buy_Now;

public record BuyNowCommand(Guid ProductId, byte Quantity) : IRequest<Result<Guid>>;