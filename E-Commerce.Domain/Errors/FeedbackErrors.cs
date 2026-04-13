using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Errors;

public class FeedbackErrors
{
    public static readonly Error EmptyUserId = new("Feedback.EmptyUserId", "User ID cannot be empty.");
    public static readonly Error EmptyProductId = new("Feedback.EmptyProductId", "Product ID cannot be empty.");
    public static readonly Error InvalidRating = new("Feedback.InvalidRating", "Rating must be between 1 and 5.");
}
