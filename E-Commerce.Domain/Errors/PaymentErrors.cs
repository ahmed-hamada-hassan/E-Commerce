using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Errors;

public class PaymentErrors
{
    public static readonly Error EmptyOrderId = new("Payment.EmptyOrderId", "Order ID cannot be empty.");
    public static readonly Error AmountCannotBeNegative = new("Payment.AmountCannotBeNegative", "Payment amount must be greater than zero.");
    public static readonly Error InvalidPaymentMethod = new("Payment.InvalidPaymentMethod", "The provided payment method is not supported or invalid.");
}
