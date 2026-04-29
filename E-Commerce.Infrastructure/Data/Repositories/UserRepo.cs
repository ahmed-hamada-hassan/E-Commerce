using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure.Data.Repositories;

internal sealed class UserRepo : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepo(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public Task<ApplicationUser?> GetDeletedByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Users.IgnoreQueryFilters().AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id && u.IsDeleted, cancellationToken);
    }

    public Task<ApplicationUser?> GetActiveCustomerWithAddressesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
            return Task.FromResult<ApplicationUser?>(null);

        return _dbContext.Users
            .AsNoTracking()
            .Include(u => u.Addresses)
            .Where(u => u.Id == userId)
            .Where(u => u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow)
            .Where(u => _dbContext.UserRoles
                .Join(_dbContext.Roles,
                    ur => ur.RoleId,
                    role => role.Id,
                    (ur, role) => new { ur.UserId, role.Name })
                .Any(x => x.UserId == u.Id && x.Name == "Customer"))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<ApplicationUser?> GetCustomerForAdminByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
            return Task.FromResult<ApplicationUser?>(null);

        return _dbContext.Users
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Include(u => u.Addresses)
            .Where(u => u.Id == userId)
            .Where(u => _dbContext.UserRoles
                .Join(_dbContext.Roles,
                    ur => ur.RoleId,
                    role => role.Id,
                    (ur, role) => new { ur.UserId, role.Name })
                .Any(x => x.UserId == u.Id && x.Name == "Customer"))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
