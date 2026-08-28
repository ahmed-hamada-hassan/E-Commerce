using E_Commerce.Application.Interfaces.Dependency_Injection;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Interfaces.Repositories;

public interface IWishlistRepository : IScopedService
{
    Task<Wishlist?> GetWishlistByUserIdAsync(Guid userId, CancellationToken ct);
    Task AddWishlistAsync(Wishlist wishlist, CancellationToken ct);
}
