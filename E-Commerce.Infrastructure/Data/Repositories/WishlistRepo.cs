using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure.Data.Repositories;

internal sealed class WishlistRepo : IWishlistRepository
{
    private readonly AppDbContext _context;

    public WishlistRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddWishlistAsync(Wishlist wishlist, CancellationToken ct)
    {
        await _context.Wishlists.AddAsync(wishlist, ct);
    }

    public async Task<Wishlist?> GetWishlistByUserIdAsync(Guid userId, CancellationToken ct)
    {
        var query = await _context.Wishlists
            .IgnoreQueryFilters()
            .Include(w => w.Items)
            .FirstOrDefaultAsync(w => w.UserId == userId, ct);

        return query;
    }
}
