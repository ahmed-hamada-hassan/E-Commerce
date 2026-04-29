using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure.Data.Repositories;

internal sealed class VendorRepo : IVendorRepository
{
    private readonly AppDbContext _dbContex;

    public VendorRepo(AppDbContext dbContex)
    {
        _dbContex = dbContex;
    }

    public async Task<Guid> AddAsync(Vendor vendor, CancellationToken ct)
    {
        await _dbContex.Vendors.AddAsync(vendor, ct);
        return vendor.Id;
    }

    public async Task<Vendor?> GetByUserIdAsync(Guid userId, CancellationToken ct)
        => await _dbContex.Vendors.FirstOrDefaultAsync(v => v.UserId == userId);

    public Task<Vendor?> GetByIdAsync(Guid vendorId, CancellationToken ct) 
        =>_dbContex.Vendors.FirstOrDefaultAsync(v => v.Id == vendorId, ct);

    public async Task<bool> IsCommercialRegistrationNumberUniquenessAsync(string commercialRegistrationNumber, CancellationToken ct)
        => !await _dbContex.Vendors.AnyAsync(v => v.CommercialRegistrationNumber == commercialRegistrationNumber);

    public async Task<bool> IsStoreNameUniquenessAsync(string storyName, CancellationToken ct)
        => !await _dbContex.Vendors.AnyAsync(v => v.StoreName == storyName);

    public Task<Vendor?> GetVendorForAdminByIdAsync(Guid vendorId, CancellationToken cancellationToken = default)
    {
        if (vendorId == Guid.Empty)
            return Task.FromResult<Vendor?>(null);

        return _dbContex.Vendors
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Include(v => v.User)
            .ThenInclude(u => u.Addresses)
            .FirstOrDefaultAsync(v => v.Id == vendorId, cancellationToken);
    }
}
