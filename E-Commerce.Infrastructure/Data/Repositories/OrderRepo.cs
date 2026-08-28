using E_Commerce.Application.Common;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums;
using E_Commerce.Infrastructure.Data.Repositories.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace E_Commerce.Infrastructure.Data.Repositories;

internal sealed class OrderRepo : IOrderRepository
{
    private readonly AppDbContext _dbContext;
    private readonly PaginationSettings _paginationSettings;

    public OrderRepo(AppDbContext dbContext, IOptionsSnapshot<PaginationSettings> paginationSettings)
    {
        _dbContext = dbContext;
        _paginationSettings = paginationSettings.Value;
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

    public async Task<Guid> AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        await _dbContext.Orders.AddAsync(order, cancellationToken);
        return order.Id;
    }

    public Task<Order?> GetAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return  _dbContext.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.Payment)
            .Include(o => o.Cancellation)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

    }

    public Task<List<Order>> GetPendingOrdersOver24HoursAsync(CancellationToken cancellationToken = default)
    {
        var cutoffDate = DateTime.UtcNow.AddHours(-24);
        return _dbContext.Orders.Include(o => o.OrderItems)
            .Where(o => o.Status == OrderStatus.Pending && o.OrderedDate < cutoffDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IReadOnlyCollection<Order> Items, Guid? NextCursor)> GetOrdersByUserAsync(Guid userId, int size, Guid? cursor, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Orders.AsNoTracking()
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderedDate);

        return await query.PaginateWithCursorAsync(o => o.Id, cursor, size, _paginationSettings.MaxSize, cancellationToken);
    }

    public async Task<(IReadOnlyCollection<Order> Items, Guid? NextCursor)> GetProcessingOrdersAsyncByDayAsync(int size, Guid? cursor, DateTimeOffset day, 
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Orders.AsNoTracking()
            .Where(o => o.Status == OrderStatus.Processing && o.OrderedDate.Date == day.Date)
            .OrderByDescending(o => o.OrderedDate);

        return await query.PaginateWithCursorAsync(o => o.Id, cursor, size, _paginationSettings.MaxSize, cancellationToken);
    }

    public async Task<(int TotalProcessing, int DayProcessing)> GetProcessingStatsAsync(DateTimeOffset day, CancellationToken ct)
    {
        var stats = await _dbContext.Orders
        .AsNoTracking()
        .Where(o => o.Status == OrderStatus.Processing)
        .GroupBy(o => 1) 
        .Select(g => new
        {
            Total = g.Count(),
            DayOnly = g.Count(o => o.OrderedDate.Date == day.Date)
        })
        .FirstOrDefaultAsync(ct);

        return (stats?.Total ?? 0, stats?.DayOnly ?? 0);
    }

    public Task<Order?> GetProcessingOrderById(Guid orderId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == orderId && o.Status == OrderStatus.Processing, cancellationToken);
    }

    public async Task<bool> HasUserPurchasedProductAsync(Guid userId, Guid productId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders.AsNoTracking()
            .AnyAsync(o => o.UserId == userId && o.OrderItems.Any(oi => oi.ProductId == productId) && 
                (o.Status == OrderStatus.Delivered), cancellationToken);
    }
}
