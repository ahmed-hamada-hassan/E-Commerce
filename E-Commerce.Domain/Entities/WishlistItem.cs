using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Entities;

public class WishlistItem
{
    public Guid Id { get; private set; }
    public Guid WishlistId { get; private set; }
    public Wishlist Wishlist { get; private set; } = null!;
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;
    public DateTimeOffset AddedAt { get; private set; }

    protected WishlistItem() { }

    private WishlistItem(Guid wishlistId, Guid productId)
    {
        Id = Guid.NewGuid();
        WishlistId = wishlistId;
        ProductId = productId;
        AddedAt = DateTimeOffset.UtcNow;
    }

    public static Result<WishlistItem> Create(Guid wishlistId, Guid productId)
    {
        if (wishlistId == Guid.Empty)
            return Result<WishlistItem>.Failure(WishlistItemErrors.EmptyWishlistId);

        if (productId == Guid.Empty)
            return Result<WishlistItem>.Failure(WishlistItemErrors.EmptyProductId);

        var wishlistItem = new WishlistItem(wishlistId, productId);
        return Result<WishlistItem>.Success(wishlistItem);
    }
}
