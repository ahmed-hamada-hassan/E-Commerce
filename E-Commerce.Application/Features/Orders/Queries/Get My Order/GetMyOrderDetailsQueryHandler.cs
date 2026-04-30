using E_Commerce.Application.Features.Orders.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Orders.Queries.Get_My_Order;

internal sealed class GetMyOrderDetailsQueryHandler : IRequestHandler<GetMyOrderDetailsQuery, Result<OrderDetailsResponse>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<GetMyOrderDetailsQueryHandler> _logger;

    public GetMyOrderDetailsQueryHandler(IOrderRepository orderRepository, ILogger<GetMyOrderDetailsQueryHandler> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task<Result<OrderDetailsResponse>> Handle(GetMyOrderDetailsQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetAsync(request.OrderId, cancellationToken);

        if(order is null)
        {
            _logger.LogWarning("Order with ID {OrderId} not found for user {UserId}", request.OrderId, request.UserId);
            return Result<OrderDetailsResponse>.Failure(OrderErrors.NotFound);
        }

        if(order.UserId != request.UserId)
        {
            _logger.LogWarning("Unauthorized access attempt to order {OrderId} by user {UserId}", request.OrderId, request.UserId);
            return Result<OrderDetailsResponse>.Failure(OrderErrors.AccessDenied);
        }

        var response = new OrderDetailsResponse(
            order.Id,
            order.OrderedDate,
            order.Status.ToString(),
            order.TotalAmount,
            order.ShippingCost,
            order.ShippingAddress,
            order.Payment!.PaymentMethod.ToString(),
            order.OrderItems.Select(oi => new OrderItemResponse(oi.ProductId, oi.ProductName, oi.Quantity,
            oi.UnitPrice, oi.Quantity * oi.UnitPrice)).ToList(),
            order.Cancellation is not null ? new CancellationResponse(order.Cancellation.CancellationDate, order.Cancellation.Reason) : null
            );

        return Result<OrderDetailsResponse>.Success(response);
    }
}
