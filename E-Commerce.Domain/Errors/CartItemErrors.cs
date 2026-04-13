using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Errors;

public class CartItemErrors
{
    public static readonly Error EmptyProductId = new("CartItem.EmptyProductId", "Product id cannot be empty.");
    public static readonly Error EmptyCartId = new("CartItem.EmptyCartId", "Cart id cannot be empty.");
    public static readonly Error QuantityLessThanOrEqualsZero = new("CartItem.QuantityLessThanOrEqualsZero", "Quantity must be greater than zero.");
}
