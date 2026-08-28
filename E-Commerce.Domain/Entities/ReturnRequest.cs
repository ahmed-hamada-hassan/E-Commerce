using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Entities;

public class ReturnRequest
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public Order Order { get; private set; } = null!;
    public ReturnStatus Status { get; private set; }
    public byte Quantity { get; private set; }
    public string Reason { get; private set; } = null!;
    public DateTimeOffset RequestedDate { get; private set; }

    protected ReturnRequest() { }

    private ReturnRequest(Guid id, Guid orderId, Guid productId, byte quantity, string reason)
    {
        Id = id;
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        Reason = reason;
        Status = ReturnStatus.Pending;
        RequestedDate = DateTimeOffset.UtcNow;
    }

    public static Result<ReturnRequest> Create(Guid orderId, Guid productId, byte quantity, string reason)
    {
        if (orderId == Guid.Empty)
            return Result<ReturnRequest>.Failure(ReturnRequestErrors.EmptyOrderId);
        if (productId == Guid.Empty)
            return Result<ReturnRequest>.Failure(ReturnRequestErrors.EmptyProductId);
        if (quantity == 0)
            return Result<ReturnRequest>.Failure(ReturnRequestErrors.InvalidQuantity);
        if (string.IsNullOrWhiteSpace(reason))
            return Result<ReturnRequest>.Failure(ReturnRequestErrors.EmptyReason);

        var returnRequest = new ReturnRequest(Guid.NewGuid(), orderId, productId, quantity, reason);

        return Result<ReturnRequest>.Success(returnRequest);
    }

    public void Approve()
    {
        Status = ReturnStatus.Approved;
    }
    public void Reject()
    {
        Status = ReturnStatus.Rejected;
    }
    public void Complete()
    {
        Status = ReturnStatus.Completed;
    }
}