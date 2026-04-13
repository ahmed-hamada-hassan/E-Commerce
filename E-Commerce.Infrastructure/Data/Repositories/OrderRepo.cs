using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure.Data.Repositories;

internal sealed class OrderRepo : IOrderRepository
{
    private readonly AppDbContext _dbContext;

    public OrderRepo(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

     OrderStatus[] activeStatuses = new[] { OrderStatus.Pending, OrderStatus.Processing, OrderStatus.Shipped };
    public async Task<bool> HasActiveOrdersForProductAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders.AsNoTracking()
            .AnyAsync(o => o.OrderItems.Any(oi => oi.ProductId == productId) && activeStatuses.Contains(o.Status), cancellationToken);
    }

    public async Task<bool> HasActiveOrdersForUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders.AsNoTracking()
            .AnyAsync(o => o.UserId == userId && activeStatuses.Contains(o.Status), cancellationToken);
    }
}
