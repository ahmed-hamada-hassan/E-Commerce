using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Entities;

public class Wishlist
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public ApplicationUser User { get; private set; } = null!;

    private readonly List<WishlistItem> _items = new ();
    public IReadOnlyCollection<WishlistItem> Items => _items.AsReadOnly();

    protected Wishlist() { }

    private Wishlist(Guid userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
    }

    public static Result<Wishlist> Create(Guid userId)
    {
        if (userId == Guid.Empty)
            return Result<Wishlist>.Failure(WishlistErrors.EmptyUserId);

        var wishlist = new Wishlist(userId);
        return Result<Wishlist>.Success(wishlist);
    }

    public Result<bool> AddItem(Guid productId)
    {
        if (productId == Guid.Empty)
            return Result<bool>.Failure(WishlistItemErrors.EmptyProductId);

        var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);

        if (existingItem != null)
            return Result<bool>.Failure(WishlistItemErrors.WishlistItemAlreadyExists);

        var wishlistItemResult = WishlistItem.Create(Id, productId);

        if (wishlistItemResult.IsFailure)
            return Result<bool>.Failure(wishlistItemResult.Error!);

        var wishlistItem = wishlistItemResult.Value!;

        _items.Add(wishlistItem);

        return Result<bool>.Success(true);
    }

    public Result<bool> RemoveItem (Guid productId)
    {
        if (productId == Guid.Empty)
            return Result<bool>.Failure(WishlistItemErrors.EmptyProductId);

        var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem == null)
            return Result<bool>.Failure(WishlistItemErrors.WishlistItemNotFound);

        _items.Remove(existingItem);
        return Result<bool>.Success(true);
    }
}
