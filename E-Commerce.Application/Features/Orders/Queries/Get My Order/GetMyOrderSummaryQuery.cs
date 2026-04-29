using E_Commerce.Application.Features.Orders.DTOs;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Orders.Queries.Get_My_Order;

public record GetMyOrderSummaryQuery(Guid UserId, Guid? Cursor, int Size) :
    IRequest<Result<CursorPagedResult<OrderSummaryResponse, Guid>>>, ICursorPaginationRequest<Guid>;