using E_Commerce.Application.Interfaces.Dependency_Injection;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Interfaces.Repositories;

public interface IOrderRepository : IScopedService
{
    Task<bool> HasActiveOrdersForProductAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<bool> HasActiveOrdersForUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Guid> AddAsync(Order order, CancellationToken cancellationToken = default);
    Task<Order?> GetAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<List<Order>> GetPendingOrdersOver24HoursAsync(CancellationToken cancellationToken = default);
    Task<(IReadOnlyCollection<Order> Items, Guid? NextCursor)> GetOrdersByUserAsync(Guid userId, int size, Guid? cursor,
        CancellationToken cancellationToken = default);
    Task<(IReadOnlyCollection<Order> Items, Guid? NextCursor)> GetProcessingOrdersAsyncByDayAsync(int size, Guid? cursor, DateTimeOffset day,
        CancellationToken cancellationToken = default);
    Task<(int TotalProcessing, int DayProcessing)> GetProcessingStatsAsync(DateTimeOffset day, CancellationToken cancellationToken);
    Task<Order?> GetProcessingOrderById(Guid orderId, CancellationToken cancellationToken = default);
    Task<bool> HasUserPurchasedProductAsync(Guid userId, Guid productId, CancellationToken cancellationToken = default);
}
