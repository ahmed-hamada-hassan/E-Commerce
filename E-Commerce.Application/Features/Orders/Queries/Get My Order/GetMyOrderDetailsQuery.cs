using E_Commerce.Application.Features.Orders.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Orders.Queries.Get_My_Order;

public record GetMyOrderDetailsQuery(Guid OrderId, Guid UserId) : IRequest<Result<OrderDetailsResponse>>;