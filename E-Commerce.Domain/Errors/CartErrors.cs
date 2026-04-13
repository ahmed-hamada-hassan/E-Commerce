using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Errors;

public class CartErrors
{
    public static readonly Error EmptyUserId = new("Cart.EmptyUserId", "The User ID cannot be empty.");
    public static readonly Error CartNotFound = new("Cart.CartNotFound", "The cart was not found for the specified user.");
    public static readonly Error CartItemNotFound = new("Cart.CartItemNotFound", "The specified item was not found in the cart.");
}
