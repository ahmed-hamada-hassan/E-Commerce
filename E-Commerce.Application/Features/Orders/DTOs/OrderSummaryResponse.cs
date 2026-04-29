namespace E_Commerce.Application.Features.Orders.DTOs;

public record OrderSummaryResponse(
    Guid Id,
    DateTime OrderedDate,
    string Status,
    decimal TotalAmount,
    decimal ShippingCost,
    bool CanCancel 
);