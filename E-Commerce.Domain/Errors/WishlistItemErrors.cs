using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Errors;

public class WishlistItemErrors
{
    public static readonly Error EmptyWishlistId = new("WishlistItem.EmptyWishlistId", "WishlistId cannot be empty.");
    public static readonly Error EmptyProductId = new("WishlistItem.EmptyProductId", "ProductId cannot be empty.");
    public static readonly Error WishlistItemAlreadyExists = new("WishlistItem.AlreadyExists", "The product is already in the wishlist.");
    public static readonly Error WishlistItemNotFound = new("WishlistItem.NotFound", "The wishlist item was not found.");
}
