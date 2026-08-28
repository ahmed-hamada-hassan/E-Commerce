using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; } = null!;
    public string MainImageUrl { get; private set; } = null!;
    public int Quantity { get; private set; }
    public int RefundedQuantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    private OrderItem(Guid id, Guid orderId, Guid productId, string productName, string mainImageUrl, int quantity, decimal unitPrice)
    {
        Id = id;
        OrderId = orderId;
        ProductId = productId;
        ProductName = productName;
        MainImageUrl = mainImageUrl;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    protected OrderItem() { }

    public static Result<OrderItem> Create(Guid orderId, Guid productId, string productName, string mainImageUrl, int quantity, decimal unitPrice)
    {
        if(orderId == Guid.Empty)
            return Result<OrderItem>.Failure(OrderItemErrors.EmptyOrderId);
        if(productId == Guid.Empty)
            return Result<OrderItem>.Failure(OrderItemErrors.EmptyProductId);
        if(string.IsNullOrWhiteSpace(productName))
            return Result<OrderItem>.Failure(OrderItemErrors.EmptyProductName);
        if(string.IsNullOrWhiteSpace(mainImageUrl))
            return Result<OrderItem>.Failure(OrderItemErrors.EmptyMainImageUrl);
        if (quantity <= 0)
            return Result<OrderItem>.Failure(OrderItemErrors.QuantityMustBeGreaterThanZero);
        if(unitPrice < 0)
            return Result<OrderItem>.Failure(OrderItemErrors.UnitPriceCannotBeNegative);

        var orderItem = new OrderItem(Guid.NewGuid(), orderId, productId, productName, mainImageUrl, quantity, unitPrice);
        return Result<OrderItem>.Success(orderItem);
    }

    public void MarkAsRefunded(int quantity)
    {
        RefundedQuantity += quantity;
    }

    public int GetAvailableQuantityForRefund() => Quantity - RefundedQuantity;
}
