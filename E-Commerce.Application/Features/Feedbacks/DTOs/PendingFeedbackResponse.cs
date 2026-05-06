namespace E_Commerce.Application.Features.Feedbacks.DTOs;

public record PendingFeedbackResponse(
    Guid UserId,
    Guid FeedbackId,
    Guid ProductId,
    string ProductName,
    string UserName,
    byte Rating,
    string? Comment,
    DateTime CreatedDate);