using E_Commerce.Application.Interfaces.Dependency_Injection;

namespace E_Commerce.Application.Interfaces.Data;

public interface IUnitOfWork : IScopedService
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
