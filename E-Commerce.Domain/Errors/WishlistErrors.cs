using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Errors;

public class WishlistErrors
{
    public static readonly Error EmptyUserId = new("Wishlist.EmptyUserId", "UserId cannot be empty.");
    public static readonly Error WishlistNotFound = new("Wishlist.NotFound", "Wishlist not found.");
}
