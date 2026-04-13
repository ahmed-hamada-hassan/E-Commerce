using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid ShippingAddressId { get; private set; }
    public DateTime OrderedDate { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal TotalAmount { get; private set; }
    public Cancellation? Cancellation { get; private set; }

    private readonly List<OrderItem> _orderItems = new();
    private readonly List<Payment> _payments = new();
    private readonly List<Refund> _refunds = new();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();
    public IReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();
    public IReadOnlyCollection<Refund> Refunds => _refunds.AsReadOnly();

    private Order(Guid id, Guid userId, Guid shippingAddressId, DateTime orderedDate, OrderStatus status, decimal totalAmount)
    {
        Id = id;
        UserId = userId;
        ShippingAddressId = shippingAddressId;
        OrderedDate = orderedDate;
        Status = status;
        TotalAmount = totalAmount;
    }

    protected Order() { }

    public static Result<Order> Create(Guid userId, Guid shippingAddressId, decimal totalAmount)
    {
        if (userId == Guid.Empty)
            return Result<Order>.Failure(OrderErrors.EmptyUserId);
        if (shippingAddressId == Guid.Empty)
            return Result<Order>.Failure(OrderErrors.EmptyShippingAddressId);

        var order = new Order(Guid.NewGuid(), userId, shippingAddressId, DateTime.UtcNow, OrderStatus.Pending, 0);
        return Result<Order>.Success(order);
    }
}
