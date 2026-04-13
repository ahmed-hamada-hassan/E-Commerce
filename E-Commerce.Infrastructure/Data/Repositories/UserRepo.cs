using E_Commerce.Application.Common;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Data.Repositories.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace E_Commerce.Infrastructure.Data.Repositories;

internal sealed class UserRepo : IUserRepository
{
    private readonly AppDbContext _dbContext;
    private readonly PaginationSettings _paginationSettings;

    public UserRepo(AppDbContext dbContext, IOptions<PaginationSettings> paginationSettings)
    {
        _dbContext = dbContext;
        _paginationSettings = paginationSettings.Value;
    }


    public Task<ApplicationUser?> GetDeletedByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return  _dbContext.Users.IgnoreQueryFilters().AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id && u.IsDeleted, cancellationToken);
    }
}
