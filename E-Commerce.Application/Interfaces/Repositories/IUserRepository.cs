using E_Commerce.Application.Interfaces.Dependency_Injection;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Interfaces.Repositories;

public interface IUserRepository : IScopedService
{
    Task<ApplicationUser?> GetDeletedByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
