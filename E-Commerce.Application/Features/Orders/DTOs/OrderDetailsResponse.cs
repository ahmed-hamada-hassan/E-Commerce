namespace E_Commerce.Application.Features.Orders.DTOs;

public record OrderDetailsResponse(
    Guid Id,
    DateTime OrderedDate,
    string Status,
    decimal TotalAmount,
    decimal ShippingCost,
    string ShippingAddress,
    string PaymentMethod,
    List<OrderItemResponse> Items,
    CancellationResponse? Cancellation
);
