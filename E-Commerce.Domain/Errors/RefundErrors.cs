using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Errors;

public class RefundErrors
{
    public static readonly Error EmptyOrderId = new("Refund.EmptyOrderId", "Order ID cannot be empty.");
    public static readonly Error EmptyAdminId = new("Refund.EmptyAdminId", "Admin ID cannot be empty.");
    public static readonly Error EmptyPaymentId = new("Refund.EmptyPaymentId", "Payment ID cannot be empty.");
    public static readonly Error InvalidAmount = new("Refund.InvalidAmount", "Refund amount must be greater than zero.");
    public static readonly Error EmptyReason = new("Refund.EmptyReason", "Refund reason cannot be empty.");
    public static readonly Error RefundAmountExceedsPayment = new("Refund.RefundAmountExceedsPayment", "The total refund amount cannot exceed the original payment amount.");
    public static readonly Error ItemNotFound = new("Refund.ItemNotFound", "The specified item was not found in the order.");
    public static readonly Error InvalidQuantity = new("Refund.InvalidQuantity", "Refund quantity must be greater than zero and cannot exceed the quantity ordered.");
    public static readonly Error PaymentNotFound = new("Refund.PaymentNotFound", "No payment associated with the order was found.");
    public static readonly Error OrderNotFound = new("Refund.OrderNotFound", "The specified order was not found.");
}
