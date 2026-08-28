using E_Commerce.Application.Features.Carts.DTOs;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Features.Carts;

public static class CartMapper
{
    public static CartResponse ToCartResponse(this Cart cart)
    {
        return new CartResponse
        (
            CartId: cart.Id,
            Items : cart.Items.Select(i => new CartItemResponse
            (
                ProductId : i.ProductId,
                ProductName : i.ProductName,
                UnitPrice : i.UnitPrice,
                Quantity : i.Quantity,
                ImageUrl : i.ImageUrl,
                SubTotal : i.UnitPrice * i.Quantity
            )).ToList(),
            TotalPrice : cart.Items.Sum(i => i.UnitPrice * i.Quantity)
        );

    }

    public static CartResponse ToEmptyCartResponse(this Cart cart)
    {
        return new CartResponse
        (
            CartId: Guid.Empty,
            Items: [],
            TotalPrice: 0
        );

    }

    public static BuyNowCartResponse ToBuyNowCartResponse(this Cart cart)
    {
        return new BuyNowCartResponse
        (
            Items : cart.Items.Select(i => new CartItemResponse
            (
                ProductId : i.ProductId,
                ProductName : i.ProductName,
                UnitPrice : i.UnitPrice,
                Quantity : i.Quantity,
                ImageUrl : i.ImageUrl,
                SubTotal : i.UnitPrice * i.Quantity
            )).ToList(),
            TotalPrice : cart.Items.Sum(i => i.UnitPrice * i.Quantity)
        );

    }

    public static CartSummaryResponse ToCartSummaryResponse(this Cart cart)
    {
        return new CartSummaryResponse
        (
            CartId: cart.Id,
            TotalItemsCount: cart.Items.Sum(i => i.Quantity),
            TotalPrice: cart.Items.Sum(i => i.UnitPrice * i.Quantity)
        );
    }
}