using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Orders.Commands.Cancel_Order;

public record CancelOrderCommand(Guid OrderId, Guid UserId, string Reason) : IRequest<Result<Guid>>;