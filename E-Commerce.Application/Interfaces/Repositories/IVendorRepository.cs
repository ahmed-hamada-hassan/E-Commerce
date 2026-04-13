using E_Commerce.Application.Interfaces.Dependency_Injection;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Interfaces.Repositories;

public interface IVendorRepository : IScopedService
{
    Task<Guid> AddAsync(Vendor vendor, CancellationToken ct);
    Task<Vendor?> GetByUserIdAsync(Guid userId, CancellationToken ct);
    Task<bool> IsStoreNameUniquenessAsync(string storyName, CancellationToken ct);
    Task<bool> IsCommercialRegistrationNumberUniquenessAsync(string commercialRegistrationNumber, CancellationToken ct);
    Task<Vendor?> GetByIdAsync(Guid vendorId, CancellationToken ct);
}
