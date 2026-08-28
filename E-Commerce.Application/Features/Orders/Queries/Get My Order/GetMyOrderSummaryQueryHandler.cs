using E_Commerce.Application.Features.Orders.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Orders.Queries.Get_My_Order;

internal sealed class GetMyOrderSummaryQueryHandler :
    IRequestHandler<GetMyOrderSummaryQuery, Result<CursorPagedResult<OrderSummaryResponse, Guid>>>
{
    private readonly IOrderRepository _orderRepository;

    public GetMyOrderSummaryQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<CursorPagedResult<OrderSummaryResponse, Guid>>> Handle(GetMyOrderSummaryQuery request, CancellationToken cancellationToken)
    {
        var orders = 
            await _orderRepository.GetOrdersByUserAsync(request.UserId, request.Size, request.Cursor, cancellationToken);

        var orderSummaries = orders.Items.Select(order => new OrderSummaryResponse(
            order.Id,
            order.OrderedDate,
            order.Status,
            order.TotalAmount,
            order.ShippingCost,
            order.Status == Domain.Enums.OrderStatus.Pending && order.OrderedDate.AddHours(24) > DateTime.UtcNow
        )).ToList();

        var pagedResult = new CursorPagedResult<OrderSummaryResponse, Guid>(
            orderSummaries,
            orders.NextCursor
        );

        return Result<CursorPagedResult<OrderSummaryResponse, Guid>>.Success(pagedResult);
    }
}
