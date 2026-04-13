using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Errors;

public class RefundErrors
{
    public static readonly Error EmptyOrderId = new("Refund.EmptyOrderId", "Order ID cannot be empty.");
    public static readonly Error EmptyPaymentId = new("Refund.EmptyPaymentId", "Payment ID cannot be empty.");
    public static readonly Error InvalidAmount = new("Refund.InvalidAmount", "Refund amount must be greater than zero.");
    public static readonly Error EmptyReason = new("Refund.EmptyReason", "Refund reason cannot be empty.");
}
