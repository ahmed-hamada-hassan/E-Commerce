using E_Commerce.Application.Common;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Data.Repositories.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace E_Commerce.Infrastructure.Data.Repositories;

internal sealed class ProductImageRepo : IProductImageRepository
{
    private readonly AppDbContext _dbContext;
    private readonly PaginationSettings _paginationSettings;

    public ProductImageRepo(AppDbContext dbContext, IOptionsSnapshot<PaginationSettings> paginationSettings)
    {
        _dbContext = dbContext;
        _paginationSettings = paginationSettings.Value;
    }

    public async Task<Guid> AddAsync(ProductImage productImage, CancellationToken cancellationToken = default)
    {
        await _dbContext.ProductImages
            .AddAsync(productImage, cancellationToken);
        return productImage.Id;
    }

    public async Task<ProductImage?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ProductImages
            .FirstOrDefaultAsync(img => img.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<ProductImage>> GetAllByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ProductImages
            .AsNoTracking()
            .Where(img => img.ProductId == productId)
            .OrderBy(img => img.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> RemoveAsync(ProductImage image, CancellationToken cancellationToken = default)
    {
          _dbContext.ProductImages
            .Remove(image);
        return true;
    }

    public async Task<bool> RemoveByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var rowAffected = await _dbContext.ProductImages
            .Where(img => img.ProductId == productId)
            .ExecuteDeleteAsync(cancellationToken);

        return rowAffected > 0;
    }

    public async Task<bool> SetPrimaryAsync(Guid productId, Guid imageId, CancellationToken cancellationToken = default)
    {
        int updatedRows = await _dbContext.ProductImages
            .Where(img => img.ProductId == productId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(img => img.IsPrimary, img => img.Id == imageId),
                cancellationToken);

        return updatedRows > 0;
    }

    public async Task<bool> UpdateAsync(ProductImage productImage, CancellationToken cancellationToken = default)
    {
        _dbContext.ProductImages.Update(productImage);
        return true;
    }

    public async Task<byte> GetCountByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return (byte) await _dbContext.ProductImages
            .Where(img => img.ProductId == productId)
            .CountAsync(cancellationToken);
    }
}