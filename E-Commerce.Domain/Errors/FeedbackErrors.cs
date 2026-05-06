using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Errors;

public class FeedbackErrors
{
    public static readonly Error EmptyUserId = new("Feedback.EmptyUserId", "User ID cannot be empty.");
    public static readonly Error EmptyProductId = new("Feedback.EmptyProductId", "Product ID cannot be empty.");
    public static readonly Error InvalidRating = new("Feedback.InvalidRating", "Rating must be between 1 and 5.");
    public static readonly Error AlreadyReviewed = new("Feedback.AlreadyReviewed", "User has already reviewed this product.");
    public static readonly Error UserNotVerified = new("Feedback.UserNotVerified", "User must have purchased the product to leave feedback.");
    public static readonly Error NotFound = new("Feedback.NotFound", "Feedback not found.");
    public static readonly Error AccessDenied = new("Feedback.AccessDenied", "User is not authorized to perform this action.");
}
