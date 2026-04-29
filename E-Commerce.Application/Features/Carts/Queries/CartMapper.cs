using E_Commerce.Application.Features.Carts.DTOs;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Features.Carts.Queries;

public static class CartMapper
{
    public static CartResponse ToCartResponse(this Cart cart)
    {
        return new CartResponse
        (
            UserId: cart.UserId,
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
}