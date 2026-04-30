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
        var paymentResult = Payment.Create(order.Id, amount, Method);

        if (paymentResult.IsFailure)
            return Result<Guid>.Failure(paymentResult.Error);

        order.AddPayment(paymentResult.Value!);

        return Result<Guid>.Success(paymentResult.Value!.Id);
    }

    public async Task<Result<Guid>> RefundPaymentAsync(Order order, List<(Guid productId, int quantity)> itemsToRefund, string reason, CancellationToken ct)
    {
        if(order.Payment is null)
            return Result<Guid>.Failure(RefundErrors.PaymentNotFound);

        var refundResult = order.ApplyRefund(itemsToRefund, reason);

        if(refundResult.IsFailure)
            return Result<Guid>.Failure(refundResult.Error);

        return Result<Guid>.Success(refundResult.Value);
    }
}