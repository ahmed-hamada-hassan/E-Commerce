namespace E_Commerce.Application.Features.Orders.DTOs;

public record AdminProcessingOrderSummaryResponse(
    Guid Id,
    Guid UserId,
    string ShippingAddress,
    DateTimeOffset OrderedDate,
    decimal TotalAmount
);