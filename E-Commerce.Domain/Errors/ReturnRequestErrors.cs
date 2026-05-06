using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Errors;

public class ReturnRequestErrors
{
    public static readonly Error EmptyOrderId = new("ReturnRequest.EmptyOrderId", "Order ID cannot be empty.");
    public static readonly Error EmptyProductId = new("ReturnRequest.EmptyProductId", "Product ID cannot be empty.");
    public static readonly Error EmptyUserId = new("ReturnRequest.EmptyUserId", "User ID cannot be empty.");
    public static readonly Error InvalidQuantity = new("ReturnRequest.InvalidQuantity", "Quantity must be greater than zero.");
    public static readonly Error InvalidStatus = new("ReturnRequest.InvalidStatus", "Invalid return request status.");
    public static readonly Error EmptyReason = new("ReturnRequest.EmptyReason", "Reason for return cannot be empty.");
    public static readonly Error OrderNotFound = new("ReturnRequest.OrderNotFound", "The specified order was not found.");
    public static readonly Error AccessDenied = new("ReturnRequest.AccessDenied", "You are not authorized to request a return for this order.");
    public static readonly Error OrderItemNotFound = new("ReturnRequest.OrderItemNotFound", "One or more items in the return request were not found in the order.");
    public static readonly Error NotFound = new("ReturnRequest.NotFound", "The specified return request was not found.");
    public static readonly Error ItemNotFound = new("ReturnRequest.ItemNotFound", "The specified item was not found for return.");
}