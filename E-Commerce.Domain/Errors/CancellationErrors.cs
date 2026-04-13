using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Errors;

public class CancellationErrors
{
    public static readonly Error EmptyReason = new("Cancellation.EmptyReason", "The cancellation reason cannot be empty.");
    public static readonly Error EmptyOrderId = new("Cancellation.EmptyOrderId", "The order ID cannot be empty.");
}
