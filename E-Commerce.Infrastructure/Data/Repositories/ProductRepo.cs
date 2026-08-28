using E_Commerce.Application.Common;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Data.Repositories.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace E_Commerce.Infrastructure.Data.Repositories;

internal sealed class ProductRepo : IProductRepository
{
    private readonly AppDbContext _dbContext;
    private readonly PaginationSettings _paginationSettings;

    public ProductRepo(AppDbContext dbContext, IOptions<PaginationSettings> paginationSettings)
    {
        _dbContext = dbContext;
        _paginationSettings = paginationSettings.Value;
    }

    #region Essential Methods
    public async Task<Guid> AddAsync(Product product, CancellationToken cancellationToken)
    { 
        await _dbContext.Products.AddAsync(product, cancellationToken);
        return product.Id;
    }
    public async Task<Product?> GetByIdAsync(Guid productId, CancellationToken cancellationToken)
    { 
        var query = _dbContext.Products
            .Where(p => p.Id == productId)
            .Include(p => p.Images)
            .Include(p => p.Category)
            .AsQueryable();

        return await query.FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<bool> UpdateAsync(Product product, CancellationToken cancellationToken)
    { 
        _dbContext.Products.Update(product);
        return true;
    }
    public async Task<bool> RemoveAsync(Product product, CancellationToken cancellationToken) 
    {
        _dbContext.Products.Remove(product);
        return true;
    }
    public async Task<bool> HardDeleteAsync(Product product, CancellationToken cancellationToken)
    {
        var rowsAffected = await _dbContext.Products
        .Where(p => p.Id == product.Id)
        .ExecuteDeleteAsync(cancellationToken);

        return rowsAffected > 0;
    }
    public async Task<ICollection<Product>> GetExpiredDeletedProductsAsync(DateTime cutoffDate, CancellationToken cancellationToken)
    {
        var query = _dbContext.Products
            .IgnoreQueryFilters()
            .Where(p => p.IsDeleted && !p.DeletedByAdmin && p.DeleteOn <= cutoffDate)
            .AsQueryable();

        query = query.OrderBy(p => p.DeleteOn).ThenBy(p => p.Id);

        return await query.ToListAsync(cancellationToken);
    }
    public async Task<bool> IsSKUExistsAsync(string sku, Guid? execuldeProductId, CancellationToken cancellationToken)
    {
        return await _dbContext.Products
            .AnyAsync(p => p.SKU == sku && (!execuldeProductId.HasValue || p.Id != execuldeProductId.Value), cancellationToken);
    }
    public async Task<(IReadOnlyList<(Product Product, double Rating, int TotalReviews)> Items, int TotalCount, int TotalPages)> FilteredAvailableProductsAsync(
    Guid? categoryId, string? searchTerm, decimal? minPrice, decimal? maxPrice, string? sortBy, int page, int size, CancellationToken cancellationToken)
    {
        var query = _dbContext.Products
            .AsNoTracking()
            .Include(p => p.Images)
            .Where(p => !p.IsDeleted && !p.DeletedByAdmin);

        if (categoryId.HasValue)
        {
            query = query.Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId.Value);
        }
        query = string.IsNullOrWhiteSpace(searchTerm) ? query :
            query.Where(p => p.Name.Contains(searchTerm) || (p.Description != null && p.Description.Contains(searchTerm)));
        query = minPrice.HasValue ? query.Where(p => p.Price >= minPrice.Value) : query;
        query = maxPrice.HasValue ? query.Where(p => p.Price <= maxPrice.Value) : query;
        query = sortBy switch
        {
            "price_asc" => query.OrderBy(p => p.Price).ThenBy(p => p.Id),
            "price_desc" => query.OrderByDescending(p => p.Price).ThenBy(p => p.Id),
            "name_asc" => query.OrderBy(p => p.Name).ThenBy(p => p.Id),
            "name_desc" => query.OrderByDescending(p => p.Name).ThenBy(p => p.Id),
            _ => query.OrderBy(p => p.Name).ThenBy(p => p.Id)
        };

        var projectedQuery = query.Select(p => new
        {
            Product = p,
            Rating = _dbContext.Set<Feedback>().Where(f => f.ProductId == p.Id).Average(r => (double?)r.Rating) ?? 0.0,
            TotalReviews = _dbContext.Set<Feedback>().Count(f => f.ProductId == p.Id)
        });

        var paginatedResult = await projectedQuery.OffsetPaginateAsync(page, size, _paginationSettings.MaxSize, cancellationToken);

        var tupleItems = paginatedResult.Items
            .Select(x => (x.Product, x.Rating, x.TotalReviews))
            .ToList();

        return (tupleItems, paginatedResult.TotalCount, paginatedResult.TotalPages);
    }
    #endregion

    #region Vendor Methods
    public async Task<(IReadOnlyCollection<Product> Items, Guid? NextCursor)> GetVendorAvailableProductsAsync(Guid vendorId, 
        Guid? cursor, int size, CancellationToken ct)
    {
        var query = _dbContext.Products
            .AsNoTracking()
            .Where(p => p.VendorId == vendorId)
            .Include(p => p.Images)
            .Include(p => p.Category)
            .AsQueryable();

        query = query.OrderByDescending(p => p.CreatedOn).ThenBy(p => p.Id);

        return await query.PaginateWithCursorAsync(p => p.Id, cursor, size, _paginationSettings.MaxSize, ct);
    }
    public async Task<(IReadOnlyCollection<Product> Items, Guid? NextCursor)> GetVendorArchivedProductsAsync(Guid vendorId, 
        Guid? cursor, int size, CancellationToken ct)
    {
        var query = _dbContext.Products
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(p => p.VendorId == vendorId && p.IsDeleted && !p.DeletedByAdmin)
            .Include(p => p.Images)
            .Include(p => p.Category)
            .AsQueryable();

        query = query.OrderByDescending(p => p.DeleteOn).ThenBy(p => p.Id);

        return await query.PaginateWithCursorAsync(p => p.Id, cursor, size, _paginationSettings.MaxSize, ct);
    }
    public async Task<Product?> GetVendorArchivedProductAsync(Guid productId, CancellationToken cancellationToken)
    {
        var query = _dbContext.Products
            .IgnoreQueryFilters()
            .Where(p => p.Id == productId && p.IsDeleted && !p.DeletedByAdmin)
            .Include(p => p.Images)
            .Include(p => p.Category)
            .AsQueryable();

        return await query.FirstOrDefaultAsync(cancellationToken);
    }
    #endregion

    #region Admin Methods
    public async Task<(IReadOnlyCollection<Product> Items, Guid? NextCursor)> GetAdminAvailableProductsAsync(Guid? vendorId, 
        Guid? cursor, int size, CancellationToken ct)
    {
        var query = _dbContext.Products
            .AsNoTracking()
            .Include(p => p.Images.Where(i => i.IsPrimary))
            .Include(p => p.Category)
            .AsQueryable();

        if(vendorId.HasValue)
            query = query.Where(p => p.VendorId == vendorId.Value);

        query = query.OrderByDescending(p => p.CreatedOn).ThenBy(p => p.Id);

        return await query.PaginateWithCursorAsync(p => p.Id, cursor, size, _paginationSettings.MaxSize, ct);
    }
    public async Task<(IReadOnlyCollection<Product> Items, Guid? NextCursor)> GetAdminArchivedProductsAsync(Guid? vendorId, 
        Guid? cursor, int size, CancellationToken ct)
    {
        var query = _dbContext.Products
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(p => p.IsDeleted && !p.DeletedByAdmin)
            .Include(p => p.Images.Where(i => i.IsPrimary))
            .Include(p => p.Category)
            .AsQueryable();

        if(vendorId.HasValue) 
            query = query.Where(p => p.VendorId == vendorId.Value);

        query = query.OrderByDescending(p => p.DeleteOn).ThenBy(p => p.Id);

        return await query.PaginateWithCursorAsync(p => p.Id, cursor, size, _paginationSettings.MaxSize, ct);
    }
    public async Task<(IReadOnlyCollection<Product> Items, Guid? NextCursor)> GetAdminSuspendedProductsAsync(Guid? vendorId, 
        Guid? cursor, int size, CancellationToken ct)
    {
        var query = _dbContext.Products
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(p => p.DeletedByAdmin)
            .Include(p => p.Images.Where(i => i.IsPrimary))
            .Include(p => p.Category)
            .AsQueryable();

        if(vendorId.HasValue)
            query = query.Where(p => p.VendorId == vendorId.Value);

        query = query.OrderByDescending(p => p.DeleteOn).ThenBy(p => p.Id);

        return await query.PaginateWithCursorAsync(p => p.Id, cursor, size, _paginationSettings.MaxSize, ct);
    }
    public async Task<Product?> GetAdminProductByIdAsync(Guid productId, CancellationToken cancellationToken)
    {
        var query = _dbContext.Products
            .Where(p => p.Id == productId)
            .Include(p => p.Images.Where(i => i.IsPrimary))
            .Include(p => p.Category)
            .AsQueryable();

        return await query.FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<Product?> GetAdminSuspendProductByIdAsync(Guid productId, CancellationToken cancellationToken)
    {
        var query = _dbContext.Products
            .IgnoreQueryFilters()
            .Where(p => p.Id == productId && p.DeletedByAdmin)
            .Include(p => p.Images.Where(i => i.IsPrimary))
            .Include(p => p.Category)
            .AsQueryable();
        return await query.FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<Product?> GetAdminArchivedProductByIdAsync(Guid productId, CancellationToken cancellationToken)
    {
        var query = _dbContext.Products
            .IgnoreQueryFilters()
            .Where(p => p.Id == productId && p.IsDeleted && !p.DeletedByAdmin)
            .Include(p => p.Images.Where(i => i.IsPrimary))
            .Include(p => p.Category)
            .AsQueryable();
            
        return await query.FirstOrDefaultAsync(cancellationToken);
    }
    #endregion
}