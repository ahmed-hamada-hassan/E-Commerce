using E_Commerce.Application.Features.Orders.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Shared;
using MediatR;
using System.Collections;

namespace E_Commerce.Application.Features.Orders.Queries.Admin_Get_Order;

internal sealed class GetSummaryProcessingOrdersQueryHandler : 
    IRequestHandler<GetSummaryProcessingOrdersQuery, Result<AdminOrdersProcessingResponse>>
{
    private readonly IOrderRepository _orderRepository;

    public GetSummaryProcessingOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<AdminOrdersProcessingResponse>> Handle(GetSummaryProcessingOrdersQuery request, 
        CancellationToken cancellationToken)
    {
        if(request.day > DateTime.UtcNow)
            return Result<AdminOrdersProcessingResponse>.Failure(new Error("Invalid day", "The specified day cannot be in the future."));

        var processingOrders = await
            _orderRepository.GetProcessingOrdersAsyncByDayAsync(request.Size, request.Cursor, request.day, cancellationToken);

        var states = await _orderRepository.GetProcessingStatsAsync(request.day, cancellationToken);

        var responseItems = processingOrders.Items.Select(order => new AdminProcessingOrderSummaryResponse(
            order.Id,
            order.UserId,
            order.ShippingAddress,
            order.OrderedDate,
            order.TotalAmount
        )).ToList();

        var response = new AdminOrdersProcessingResponse(
            responseItems,
            processingOrders.NextCursor, 
            states.TotalProcessing,
            states.DayProcessing
        );

        return Result<AdminOrdersProcessingResponse>.Success(response);
    }
}
