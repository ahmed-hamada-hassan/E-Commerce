using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure.Data.Repositories;

internal sealed class ReturnRequestRepo : IReturnRequestRepository
{
    private readonly AppDbContext _dbContext;

    public ReturnRequestRepo(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ReturnRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var returnRequest = await _dbContext.ReturnRequests
            .Include(rr => rr.Order)
            .ThenInclude(o => o.Payment)
            .FirstOrDefaultAsync(rr => rr.Id == id);

        return returnRequest;
    }
}
