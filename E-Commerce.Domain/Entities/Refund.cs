using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Entities;

public class Refund
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Order Order { get; private set; } = null!;
    public Guid PaymentId { get; private set; }
    public Payment Payment { get; private set; } = null!;
    public decimal Amount { get; private set; }
    public string Reason { get; private set; } = null!;
    public DateTime RefundDate { get; private set; }
    public RefundStatus RefundStatus { get; private set; }
    public string? RefundTransactionId { get; private set; }

    private Refund(Guid id, Guid orderId, Guid paymentId, decimal amount, string reason, 
        DateTime refundDate, RefundStatus refundStatus, string? refundTransactionId)
    {
        Id = id;
        OrderId = orderId;
        PaymentId = paymentId;
        Amount = amount;
        Reason = reason;
        RefundDate = refundDate;
        RefundStatus = refundStatus;
        RefundTransactionId = refundTransactionId;
    }

    protected Refund() { }

    public static Result<Refund> Create (Guid orderId, Guid paymentId, decimal amount, string reason)
    {
        if (orderId == Guid.Empty)
            return Result<Refund>.Failure(RefundErrors.EmptyOrderId);
        if (paymentId == Guid.Empty)
            return Result<Refund>.Failure(RefundErrors.EmptyPaymentId);
        if (amount <= 0)
            return Result<Refund>.Failure(RefundErrors.InvalidAmount);
        if (string.IsNullOrWhiteSpace(reason))
            return Result<Refund>.Failure(RefundErrors.EmptyReason);

        var refund = new Refund(Guid.NewGuid(), orderId, paymentId, amount, reason, 
            DateTime.UtcNow, RefundStatus.Pending, null);
        return Result<Refund>.Success(refund);
    }
}
