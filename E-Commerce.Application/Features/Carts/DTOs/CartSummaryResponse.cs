namespace E_Commerce.Application.Features.Carts.DTOs;

public record CartSummaryResponse(
    Guid CartId,
    int TotalItemsCount,
    decimal TotalPrice
);