using E_Commerce.Domain.Enums;

namespace E_Commerce.Application.Features.Orders.DTOs;

public record OrderDetailsResponse(
    Guid Id,
    DateTimeOffset OrderedDate,
    string Status,
    decimal TotalAmount,
    decimal ShippingCost,
    string ShippingAddress,
    PaymentMethod PaymentMethod,
    List<OrderItemResponse> Items,
    CancellationResponse? Cancellation
);
