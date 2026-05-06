namespace E_Commerce.Application.Features.Feedbacks.DTOs;

public record ProductFeedbackResponse(
    Guid FeedbackId,
    string UserName,
    byte Rating,
    string? Comment,
    DateTime CreatedDate,
    bool IsVerifiedPurchase);