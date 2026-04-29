using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Orders.Commands.Shipped_Order;

public record ShippedOrderCommand(Guid OrderId) : IRequest<Result<bool>>;