namespace E_Commerce.Application.Features.Feedbacks.DTOs;

public record ProductFeedbackResponse(
    Guid FeedbackId,
    Guid UserId,
    string? ImageUrl,
    string UserName,
    byte Rating,
    string? Comment,
    DateTimeOffset CreatedDate,
    bool IsVerifiedPurchase);