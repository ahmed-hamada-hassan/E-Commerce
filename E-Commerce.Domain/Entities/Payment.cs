using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Entities;

public class Payment
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; } 
    public decimal Amount { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public PaymentStatus PaymentStatus { get; private set; }
    public DateTimeOffset PaymentDate { get; private set; }
    public string? TransactionId { get; private set; }

    private readonly List<Refund> _refunds = new();
    public IReadOnlyCollection<Refund> Refunds => _refunds.AsReadOnly();

    private Payment(Guid id, Guid orderId, decimal amount, PaymentMethod paymentMethod,
        PaymentStatus paymentStatus, DateTimeOffset paymentDate, string? transactionId)
    {
        Id = id;
        OrderId = orderId;
        Amount = amount;
        PaymentMethod = paymentMethod;
        PaymentStatus = paymentStatus;
        PaymentDate = paymentDate;
        TransactionId = transactionId;
    }

    protected Payment() { } 

    public static Result<Payment> Create(Guid orderId, decimal amount, PaymentMethod paymentMethod)
    {
        if (orderId == Guid.Empty)
            return Result<Payment>.Failure(PaymentErrors.EmptyOrderId);

        if (amount < 0)
            return Result<Payment>.Failure(PaymentErrors.AmountCannotBeNegative);

        if (Enum.IsDefined(typeof(PaymentMethod), paymentMethod) == false)
            return Result<Payment>.Failure(PaymentErrors.InvalidPaymentMethod);

        var payment = new Payment(Guid.NewGuid(), orderId, amount, paymentMethod, PaymentStatus.Pending, DateTimeOffset.UtcNow, null);
        return Result<Payment>.Success(payment);
    }
}
