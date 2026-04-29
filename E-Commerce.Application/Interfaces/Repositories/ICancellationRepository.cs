using E_Commerce.Application.Interfaces.Dependency_Injection;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Shared;

namespace E_Commerce.Application.Interfaces.Repositories;

public interface ICancellationRepository : IScopedService
{
    Task<Guid> AddAsync(Cancellation cancellation, CancellationToken ct = default);
}
