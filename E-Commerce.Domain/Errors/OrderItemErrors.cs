using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Errors;

public class OrderItemErrors
{
    public static readonly Error EmptyOrderId = new("OrderItem.EmptyOrderId", "Order ID cannot be empty.");
    public static readonly Error EmptyProductId = new("OrderItem.EmptyProductId", "Product ID cannot be empty.");
    public static readonly Error QuantityMustBeGreaterThanZero = new("OrderItem.QuantityMustBeGreaterThanZero", "Quantity must be greater than zero.");
    public static readonly Error UnitPriceCannotBeNegative = new("OrderItem.UnitPriceMustBePositiveOrEqualsZero", "Unit price must be positive or equals zero.");
    public static readonly Error EmptyProductName = new("OrderItem.EmptyProductName", "Product name cannot be empty.");
    public static readonly Error NotFound = new("OrderItem.NotFound", "Order item not found.");
}
