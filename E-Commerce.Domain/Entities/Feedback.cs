using E_Commerce.Domain.Common;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Entities;

public class Feedback : SoftDeletable
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid ProductId { get; private set; }
    public byte Rating { get; private set; }
    public string? Comment { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime? UpdatedDate { get; private set; }
    public bool IsApproved { get; private set; } = false;
    public bool IsVerifiedPurchase { get; private set; } = false;

    private Feedback(Guid id, Guid userId, Guid productId, byte rating, string? comment, 
        DateTime createdDate, DateTime? updatedDate, bool isApproved, bool isVerifiedPurchase)
    {
        Id = id;
        UserId = userId;
        ProductId = productId;
        Rating = rating;
        Comment = comment;
        CreatedDate = createdDate;
        UpdatedDate = updatedDate;
        IsApproved = isApproved;
        IsVerifiedPurchase = isVerifiedPurchase;
    }

    protected Feedback() { }

    public static Result<Feedback> Create(Guid userId, Guid productId, byte rating, string? comment, bool isVerifiedPurchase)
    {
        if (userId == Guid.Empty)
            return Result<Feedback>.Failure(FeedbackErrors.EmptyUserId);
        if (productId == Guid.Empty)
            return Result<Feedback>.Failure(FeedbackErrors.EmptyProductId);
        if(rating < 1 || rating > 5)
            return Result<Feedback>.Failure(FeedbackErrors.InvalidRating);

        var feedback = new Feedback(Guid.NewGuid(), userId, productId, rating, comment, DateTime.UtcNow, null, false, isVerifiedPurchase);
        return Result<Feedback>.Success(feedback);
    }
}
