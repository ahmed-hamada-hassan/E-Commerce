using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Entities;

public class Cancellation
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Order Order { get; private set; } = null!;
    public Guid? UserId { get; private set; }
    public string Reason { get; private set; } = null!;
    public DateTime CancellationDate { get; private set; }

    private Cancellation(Guid id, Guid orderId, Guid? userId, string reason, DateTime cancellationDate)
    {
        Id = id;
        OrderId = orderId;
        UserId = userId;
        Reason = reason;
        CancellationDate = cancellationDate;
    }
    protected Cancellation() { }

    public static Result<Cancellation> Create(Guid orderId, Guid? userId, string reason)
    {
        if(orderId == Guid.Empty)
            return Result<Cancellation>.Failure(CancellationErrors.EmptyOrderId);
        if (string.IsNullOrWhiteSpace(reason))
            return Result<Cancellation>.Failure(CancellationErrors.EmptyReason);

        var cancellation = new Cancellation(Guid.NewGuid(), orderId, userId, reason, DateTime.UtcNow);
        return Result<Cancellation>.Success(cancellation);
    }
}
