using E_Commerce.Application.Features.Orders.DTOs;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;

namespace E_Commerce.Infrastructure.Services.Payments.Services;

public class CashOnDeliveryService : IPaymentService
{
    public PaymentMethod Method => PaymentMethod.CashOnDelivery;

    public async Task<Result<Guid>> ProcessPaymentAsync(Order order, decimal amount, CancellationToken ct)
    {
        var result = order.AddPayment(PaymentMethod.CashOnDelivery);

        if (result.IsFailure)
            return Result<Guid>.Failure(result.Error);

        return Result<Guid>.Success(result.Value!.Id);
    }

    public async Task<Result<Guid>> RefundPaymentAsync(Guid adminId, Order order, List<ReturnRequestItemsDto> itemsToRefund, string reason, CancellationToken ct)
    {
        if(order.Payment is null)
            return Result<Guid>.Failure(RefundErrors.PaymentNotFound);

        // Map List<ReturnRequestItemsDto> to List<(Guid productId, int quantity)>
        var itemsToRefundTuples = itemsToRefund
            .Select(x => (x.ProductId, (int)x.Quantity))
            .ToList();

        var refundResult = order.ApplyRefund(adminId, itemsToRefundTuples, reason);

        if(refundResult.IsFailure)
            return Result<Guid>.Failure(refundResult.Error);

        return Result<Guid>.Success(refundResult.Value);
    }
}