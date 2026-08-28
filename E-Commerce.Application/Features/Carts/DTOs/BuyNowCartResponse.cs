namespace E_Commerce.Application.Features.Carts.DTOs;

public record BuyNowCartResponse(
    List<CartItemResponse> Items,
    decimal TotalPrice
);