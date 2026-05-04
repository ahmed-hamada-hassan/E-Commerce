using E_Commerce.Application.Features.Orders.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Orders.Commands.Refund_Order;

public record RefundOrderCommand(Guid OrderId, Guid AdminId, List<ReturnRequestItemsDto> Items, string Reason) : IRequest<Result<Guid>>;