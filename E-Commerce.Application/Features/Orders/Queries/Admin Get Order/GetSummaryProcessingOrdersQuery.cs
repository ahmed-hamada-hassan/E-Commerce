using E_Commerce.Application.Features.Orders.DTOs;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Orders.Queries.Admin_Get_Order;

public record GetSummaryProcessingOrdersQuery(int Size, Guid? Cursor, DateTimeOffset day) : 
    IRequest<Result<AdminOrdersProcessingResponse>>, ICursorPaginationRequest<Guid>;