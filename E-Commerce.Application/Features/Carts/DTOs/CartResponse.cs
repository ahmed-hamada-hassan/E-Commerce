namespace E_Commerce.Application.Features.Carts.DTOs;

public record CartResponse(
    Guid CartId,
    List<CartItemResponse> Items,
    decimal TotalPrice
);
