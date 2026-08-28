using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure.Data.Repositories;

internal sealed class FeedbackRepo : IFeedbackRepository
{
    private readonly AppDbContext _dbContext;

    public FeedbackRepo(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddReviewAsync(Feedback feedback, CancellationToken cancellationToken)
    {
        _dbContext.Reviews.Add(feedback);
        return feedback.Id;
    }

    public async Task<bool> HasUserReviewedProductAsync(Guid userId, Guid productId, CancellationToken cancellationToken)
    {
        return await _dbContext.Reviews.AnyAsync(r => r.UserId == userId && r.ProductId == productId, cancellationToken);
    }

    public async Task<Feedback?> GetByIdAsync(Guid feedbackId, CancellationToken cancellationToken)
    {
        return await _dbContext.Reviews.FirstOrDefaultAsync(r => r.Id == feedbackId, cancellationToken);
    }

    public async Task<(double AverageRating, int TotalReviews)> GetProductRatingAsync(Guid productId, CancellationToken cancellationToken)
    {
        var ratingData = await _dbContext.Reviews.AsNoTracking()
        .Where(r => r.ProductId == productId && r.IsApproved)
        .GroupBy(r => r.ProductId) 
        .Select(g => new
        {
            TotalReviews = g.Count(),
            AverageRating = Math.Round(g.Average(x => (double?)x.Rating) ?? 0, 1)
        })
        .FirstOrDefaultAsync(cancellationToken);

        if (ratingData == null)
            return (0, 0);

        return (ratingData.AverageRating, ratingData.TotalReviews);
    }
}
