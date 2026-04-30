using E_Commerce.Application.Interfaces.Dependency_Injection;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Shared;

namespace E_Commerce.Application.Interfaces.Services;

public interface IPaymentService : IScopedService
{
    PaymentMethod Method { get; }

    Task<Result<Guid>> ProcessPaymentAsync(Order order, decimal amount, CancellationToken ct);

    Task<Result<Guid>> RefundPaymentAsync(Order order, List<(Guid productId, int quantity)> itemsToRefund, string reason, CancellationToken ct);
}