using E_Commerce.Application.Interfaces.Dependency_Injection;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Interfaces.Repositories;

public interface IUserRepository : IScopedService
{
    Task<ApplicationUser?> GetDeletedByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Active (non-deleted), non-locked user in the Customer role, with addresses loaded.
    /// </summary>
    Task<ApplicationUser?> GetActiveCustomerWithAddressesAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Customer role user by id for admin (includes soft-deleted and locked), with addresses loaded.
    /// </summary>
    Task<ApplicationUser?> GetCustomerForAdminByIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
