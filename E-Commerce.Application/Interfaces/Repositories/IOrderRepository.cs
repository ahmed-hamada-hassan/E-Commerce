using E_Commerce.Application.Interfaces.Dependency_Injection;

namespace E_Commerce.Application.Interfaces.Repositories;

public interface IOrderRepository : IScopedService
{
    Task<bool> HasActiveOrdersForProductAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<bool> HasActiveOrdersForUserAsync(Guid userId, CancellationToken cancellationToken = default);
}
