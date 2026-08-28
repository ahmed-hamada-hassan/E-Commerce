using E_Commerce.Application.Interfaces.Dependency_Injection;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Interfaces.Repositories;

public interface ICartRepository : IScopedService
{
    Task<Cart?> GetAsync(Guid CartId, CancellationToken ct);
    Task<bool> UpdateAsync(Cart Cart, CancellationToken ct);
    Task<bool> DeleteAsync(Guid CartId, CancellationToken ct);
    Task<bool> DeleteBuyNowCartAsync(Guid CartId, CancellationToken ct);
    Task<bool> SetBuyNowCartAsync(Cart Cart, CancellationToken ct);
    Task<Cart?> GetBuyNowCartAsync(Guid CartId, CancellationToken ct);
}
