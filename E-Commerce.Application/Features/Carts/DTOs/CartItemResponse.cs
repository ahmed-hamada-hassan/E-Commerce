namespace E_Commerce.Application.Features.Carts.DTOs;

public record CartItemResponse(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity,
    string? ImageUrl,
    decimal SubTotal
);