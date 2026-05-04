using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string ShippingAddress { get; private set; } = null!;
    public Guid ShippingAddressId { get; private set; }
    public DateTime OrderedDate { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal TotalAmount { get; private set; }
    public decimal ShippingCost { get; private set; }
    public Cancellation? Cancellation { get; private set; }
    public Payment? Payment { get; private set; }

    private readonly List<OrderItem> _orderItems = new();
    private readonly List<Refund> _refunds = new();
    private readonly List<ReturnRequest> _returnRequests = new();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();
    public IReadOnlyCollection<Refund> Refunds => _refunds.AsReadOnly();
    public IReadOnlyCollection<ReturnRequest> ReturnRequests => _returnRequests.AsReadOnly();

    private Order(Guid id, Guid userId, Guid shippingAddressId, string shippingAddress, DateTime orderedDate, OrderStatus status, decimal shippingCost)
    {
        Id = id;
        UserId = userId;
        ShippingAddressId = shippingAddressId;
        ShippingAddress = shippingAddress;
        OrderedDate = orderedDate;
        Status = status;
        TotalAmount = shippingCost;
        ShippingCost = shippingCost;
    }

    protected Order() { }

    public static Result<Order> Create(Guid userId, Guid shippingAddressId, string shippingAddress, decimal shippingCost)
    {
        if (userId == Guid.Empty)
            return Result<Order>.Failure(OrderErrors.EmptyUserId);
        if (shippingAddressId == Guid.Empty)
            return Result<Order>.Failure(OrderErrors.EmptyShippingAddressId);

        var order = new Order(Guid.NewGuid(), userId, shippingAddressId, shippingAddress, DateTime.UtcNow, OrderStatus.Pending, shippingCost);
        return Result<Order>.Success(order);
    }

    public void AddOrderItem(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        var orderItemResult = OrderItem.Create(Id, productId, productName, quantity, unitPrice);

        if (orderItemResult.IsSuccess)
        {
            _orderItems.Add(orderItemResult.Value!);
            TotalAmount += (unitPrice * quantity);
        }
    }

    public void Confirm()
    {
        Status = OrderStatus.Processing;
    }

    public Result<bool> Cancel(Guid userId, string reason)
    {
        if(UserId != userId)
            return Result<bool>.Failure(OrderErrors.AccessDenied);
        if(Status != OrderStatus.Pending)
            return Result<bool>.Failure(OrderErrors.CancellationWindowClosed);
        if(OrderedDate.AddHours(24) < DateTime.UtcNow)
            return Result<bool>.Failure(OrderErrors.CancellationWindowClosed);
        if(string.IsNullOrEmpty(reason))
            return Result<bool>.Failure(CancellationErrors.EmptyReason);

        var cancellationResult = Cancellation.Create(Id, userId, reason);
        if(cancellationResult.IsFailure)
            return Result<bool>.Failure(cancellationResult.Error);

        Status = OrderStatus.Cancelled;
        Cancellation = cancellationResult.Value;

        return Result<bool>.Success(true);
    }

    public void MarkAsShipped()
    {
        Status = OrderStatus.Shipped;
    }
    public void MarkAsDelivered()
    {
        Status = OrderStatus.Delivered;
    }

    public void AddPayment(Payment payment)
    {
        Payment = payment;
    }

    public Result<Guid> ApplyRefund(Guid adminId, List<(Guid productId, int quantity)> itemsToRefund, string reason)
    {
        decimal totalCalculatedRefund = 0;

        foreach(var item in itemsToRefund)
        {
            var orderItem = _orderItems.FirstOrDefault(oi => oi.ProductId == item.productId);
            if(orderItem is null)
                return Result<Guid>.Failure(RefundErrors.ItemNotFound);
            if(item.quantity > orderItem.Quantity)
                return Result<Guid>.Failure(RefundErrors.InvalidQuantity);

            totalCalculatedRefund += (orderItem.UnitPrice * item.quantity);
            orderItem.MarkAsRefunded(item.quantity);
        }

        var refundResult = Refund.Create(Id, adminId, Payment!.Id, totalCalculatedRefund, reason);

        if(refundResult.IsFailure)
            return Result<Guid>.Failure(refundResult.Error);

        _refunds.Add(refundResult.Value!);

        return Result<Guid>.Success(refundResult.Value!.Id);
    }

    public Result<Guid> AddReturnRequest(Guid productId, byte quantity, string reason)
    {
        var result =  ReturnRequest.Create(Id, productId, quantity, reason);
        if(result.IsFailure)
            return Result<Guid>.Failure(result.Error);

        _returnRequests.Add(result.Value!);

        return Result<Guid>.Success(result.Value!.Id);
    }

    public void UpdateStatusAfterReturn()
    {
        var totalOrdered = _orderItems.Sum(oi => oi.Quantity);
        var totalReturned = _returnRequests.Where(rr => rr.Status == ReturnStatus.Approved || rr.Status == ReturnStatus.Completed)
            .Sum(rr => rr.Quantity);

        if(totalReturned == 0) return;

        if(totalReturned == totalOrdered)
            Status = OrderStatus.Returned;
        else
            Status = OrderStatus.PartiallyReturned;
    }
}
