using E_Commerce.Application.Interfaces.Dependency_Injection;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Interfaces.Repositories;

public interface IProductImageRepository : IScopedService
{
    Task<Guid> AddAsync(ProductImage Image, CancellationToken cancellationToken = default);
    Task<bool> RemoveAsync(ProductImage Image, CancellationToken cancellationToken = default);
    Task<bool> RemoveByProductIdAsync(Guid ProductId, CancellationToken cancellationToken = default);
    Task<ProductImage?> GetAsync(Guid ImageId, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(ProductImage Image, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProductImage>> GetAllByProductIdAsync(Guid ProductId, CancellationToken cancellationToken = default);
    Task<bool> SetPrimaryAsync(Guid ProductId, Guid ImageId, CancellationToken cancellationToken = default);
    Task<byte> GetCountByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
}