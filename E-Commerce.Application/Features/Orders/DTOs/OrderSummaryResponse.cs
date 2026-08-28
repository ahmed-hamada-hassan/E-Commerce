using E_Commerce.Domain.Enums;

namespace E_Commerce.Application.Features.Orders.DTOs;

public record OrderSummaryResponse(
    Guid Id,
    DateTimeOffset OrderedDate,
    OrderStatus Status,
    decimal TotalAmount,
    decimal ShippingCost,
    bool CanCancel 
);