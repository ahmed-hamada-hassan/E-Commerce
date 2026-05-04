using E_Commerce.Application.Features.Orders.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Orders.Commands.Return_Request_Order;

public record ReturnRequestOrderCommand(Guid OrderId, Guid UserId, List<ReturnRequestItemsDto> Items, string Reason) : IRequest<Result<bool>>;