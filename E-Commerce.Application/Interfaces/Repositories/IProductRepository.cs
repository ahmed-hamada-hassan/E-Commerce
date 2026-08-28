using E_Commerce.Application.Interfaces.Dependency_Injection;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Shared;

namespace E_Commerce.Application.Interfaces.Repositories;

public interface IProductRepository : IScopedService
{
    #region Essential Methods
    Task<Guid> AddAsync(Product product, CancellationToken cancellationToken);
    Task<Product?> GetByIdAsync(Guid productId, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Product product, CancellationToken cancellationToken);
    Task<bool> RemoveAsync(Product product, CancellationToken cancellationToken);
    Task<bool> HardDeleteAsync(Product product, CancellationToken cancellationToken);
    Task<ICollection<Product>> GetExpiredDeletedProductsAsync(DateTime cutoffDate, CancellationToken cancellationToken);
    Task<bool> IsSKUExistsAsync(string sku, Guid? execuldeProductId, CancellationToken cancellationToken);
    Task<(IReadOnlyList<(Product Product, double Rating, int TotalReviews)> Items, int TotalCount, int TotalPages)> FilteredAvailableProductsAsync(Guid? categoryId, string? searchTerm, decimal? minPrice, 
        decimal? maxPrice, string? sortBy, int page, int size, CancellationToken cancellationToken);
    #endregion

    #region Vendor Methods
    Task<(IReadOnlyCollection<Product> Items, Guid? NextCursor)> GetVendorAvailableProductsAsync(Guid vendorId, Guid? cursor, int size, CancellationToken ct);
    Task<(IReadOnlyCollection<Product> Items, Guid? NextCursor)> GetVendorArchivedProductsAsync(Guid vendorId, Guid? cursor, int size, CancellationToken ct);
    Task<Product?> GetVendorArchivedProductAsync(Guid productId, CancellationToken cancellationToken);
    #endregion

    #region Admin Methods
    Task<(IReadOnlyCollection<Product> Items, Guid? NextCursor)> GetAdminAvailableProductsAsync(Guid? vendorId, Guid? cursor, int size, CancellationToken ct);
    Task<(IReadOnlyCollection<Product> Items, Guid? NextCursor)> GetAdminArchivedProductsAsync(Guid? vendorId, Guid? cursor, int size, CancellationToken ct);
    Task<(IReadOnlyCollection<Product> Items, Guid? NextCursor)> GetAdminSuspendedProductsAsync(Guid? vendorId, Guid? cursor, int size, CancellationToken ct);
    Task<Product?> GetAdminProductByIdAsync(Guid productId, CancellationToken cancellationToken);
    Task<Product?> GetAdminSuspendProductByIdAsync(Guid productId, CancellationToken cancellationToken);
    Task<Product?> GetAdminArchivedProductByIdAsync(Guid productId, CancellationToken cancellationToken);
    #endregion
}
