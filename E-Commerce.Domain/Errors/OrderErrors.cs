using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Errors;

public class OrderErrors
{
    public static readonly Error EmptyUserId = new("Order.EmptyUserId", "User ID cannot be empty.");
    public static readonly Error EmptyShippingAddressId = new("Order.EmptyShippingAddressId", "Shipping Address ID cannot be empty.");
    public static readonly Error AddressRequired = new("Order.AddressRequired", "A shipping address is required to place an order.");
    public static readonly Error NotFound = new("Order.NotFound", "Order not found.");
    public static readonly Error CancellationWindowClosed = new("Order.CancellationWindowClosed", "Order cannot be cancelled as the cancellation window has closed.");
    public static readonly Error AccessDenied = new("Order.AccessDenied", "You do not have permission to access this order.");
}
