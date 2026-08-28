using E_Commerce.Domain.Common;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;

namespace E_Commerce.Domain.Entities;

public class Feedback : SoftDeletable
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid ProductId { get; private set; }
    public Guid? AdminId { get; private set; }
    public byte Rating { get; private set; }
    public string? Comment { get; private set; }
    public DateTimeOffset CreatedDate { get; private set; }
    public DateTimeOffset? UpdatedDate { get; private set; }
    public DateTimeOffset? ApprovedAt { get; private set; }
    public bool IsApproved { get; private set; } = false;
    public bool IsVerifiedPurchase { get; private set; } = false;

    private Feedback(Guid id, Guid userId, Guid productId, Guid? adminId, byte rating, string? comment, 
        DateTimeOffset createdDate, DateTimeOffset? updatedDate, DateTimeOffset? approvedAt, bool isApproved, bool isVerifiedPurchase)
    {
        Id = id;
        UserId = userId;
        ProductId = productId;
        AdminId = adminId;
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

        var feedback = new Feedback(Guid.NewGuid(), userId, productId, null, rating, comment, DateTimeOffset.UtcNow, 
            null, null, false, isVerifiedPurchase);
        return Result<Feedback>.Success(feedback);
    }

    public Result<bool> Edit(byte? rating, string? comment)
    {
        if(rating.HasValue)
            Rating = rating.Value;
        if(!string.IsNullOrWhiteSpace(comment))
            Comment = comment;

        UpdatedDate = DateTimeOffset.UtcNow;
        return Result<bool>.Success(true);
    }

    public void Approve(Guid adminId)
    {
        IsApproved = true;
        AdminId = adminId;
        ApprovedAt = DateTimeOffset.UtcNow;
    }
}
