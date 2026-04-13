using E_Commerce.Application.Common;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Data.Repositories.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace E_Commerce.Infrastructure.Data.Repositories;

internal sealed class CategoryRepo : ICategoryRepository
{
    private readonly AppDbContext _dbContext;
    private readonly PaginationSettings _paginationSettings;

    public CategoryRepo(AppDbContext dbContext, IOptions<PaginationSettings> paginationSettings)
    {
        _dbContext = dbContext;
        _paginationSettings = paginationSettings.Value;
    }

    public async Task<Guid> AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        await _dbContext.Categories.AddAsync(category, cancellationToken);
        return category.Id;
    }
    public async Task<bool> RemoveAsync(Category category, CancellationToken cancellationToken = default)
    {
        _dbContext.Categories.Remove(category);
        return true;
    }
    public async Task<(IReadOnlyCollection<Category> Items, Guid? NextCursor)> CategoriesAsync(Guid? cursor, int size, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Categories
            .AsNoTracking()
            .Include(c => c.ParentCategory)
            .AsQueryable();

        query = query.OrderBy(c => c.Id);

        return await query.PaginateWithCursorAsync(c => c.Id, cursor, size, _paginationSettings.MaxSize, cancellationToken);
    }
    public async Task<(IReadOnlyCollection<Category> Items, Guid? NextCursor)> PublicCategoriesAsync(Guid? cursor, int size, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Categories
            .AsNoTracking()
            .Include(c => c.ParentCategory)
            .AsQueryable();
        query = query.OrderBy(c => c.Id);

        return await query.PaginateWithCursorAsync(c => c.Id, cursor, size, _paginationSettings.MaxSize, cancellationToken);
    }
    public async Task<(IReadOnlyCollection<Category> Items, Guid? NextCursor)> DeletedCategoriesAsync(Guid? cursor, int size, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Categories
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(c => c.IsDeleted)
            .AsQueryable();

        query = query.OrderBy(c => c.Id);

        return await query.PaginateWithCursorAsync(c => c.Id, cursor, size, _paginationSettings.MaxSize, cancellationToken);
    }
    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
    public async Task<Category?> GetDeletedByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Categories
            .IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted, cancellationToken);
    }
    public async Task<bool> UpdateAsync(Category category, CancellationToken cancellationToken = default)
    {
        await _dbContext.Categories
            .Where(c => c.Id == category.Id)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(c => c.Name, category.Name)
            .SetProperty(c => c.Description, category.Description)
            .SetProperty(c => c.ParentCategoryId, category.ParentCategoryId)
            .SetProperty(c => c.ImageUrl, category.ImageUrl), cancellationToken);

        return true;
    }
    public Task<bool> IsExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Categories.AnyAsync(c => c.Id == id, cancellationToken);
    }
}
