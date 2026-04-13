using E_Commerce.Application.Interfaces.Dependency_Injection;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Interfaces.Repositories;

public interface ICartRepository : IScopedService
{
    Task<Cart?> GetAsync(Guid UserId, CancellationToken ct);
    Task<Cart?> UpdateAsync(Cart cart, CancellationToken ct);
    Task<bool> DeleteAsync(Guid UserId, CancellationToken ct);
}
