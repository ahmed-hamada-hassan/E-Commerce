using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Infrastructure.Data.Repositories;

public class CancelRepo : ICancellationRepository
{
    private readonly AppDbContext _dbContext;

    public CancelRepo(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddAsync(Cancellation cancellation, CancellationToken ct = default)
    {
        await _dbContext.Cancellations.AddAsync(cancellation, ct);
        return cancellation.Id;
    }
}