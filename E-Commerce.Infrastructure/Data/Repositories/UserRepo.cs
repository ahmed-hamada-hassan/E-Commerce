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
}
