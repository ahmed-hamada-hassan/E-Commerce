using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Errors;

public class OrderErrors
{
    public static readonly Error EmptyUserId = new("Order.EmptyUserId", "User ID cannot be empty.");
    public static readonly Error EmptyShippingAddressId = new("Order.EmptyShippingAddressId", "Shipping Address ID cannot be empty.");
}
