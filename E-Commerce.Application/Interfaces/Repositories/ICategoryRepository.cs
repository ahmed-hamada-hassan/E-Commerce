using E_Commerce.Application.Interfaces.Dependency_Injection;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Interfaces.Repositories;

public interface ICategoryRepository : IScopedService
{
    Task<Guid> AddAsync(Category category, CancellationToken cancellationToken = default);
    Task<bool> RemoveAsync(Category category, CancellationToken cancellationToken = default);
    Task<(IReadOnlyCollection<Category> Items, Guid? NextCursor)> CategoriesAsync(Guid? cursor, int size, CancellationToken cancellationToken = default);
    Task<(IReadOnlyCollection<Category> Items, Guid? NextCursor)> PublicCategoriesAsync(Guid? cursor, int size, CancellationToken cancellationToken = default);
    Task<(IReadOnlyCollection<Category> Items, Guid? NextCursor)> DeletedCategoriesAsync(Guid? cursor, int size, CancellationToken cancellationToken = default);
    Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Category?> GetDeletedByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Category category, CancellationToken cancellationToken = default);
    Task<bool> IsExistsAsync(Guid id, CancellationToken cancellationToken = default);
}