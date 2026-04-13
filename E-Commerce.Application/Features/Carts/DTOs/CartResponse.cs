namespace E_Commerce.Application.Features.Carts.DTOs;

public record CartResponse(
    Guid UserId,
    List<CartItemResponse> Items,
    decimal TotalPrice
);