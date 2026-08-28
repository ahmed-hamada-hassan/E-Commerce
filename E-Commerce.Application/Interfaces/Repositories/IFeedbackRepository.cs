using E_Commerce.Application.Interfaces.Dependency_Injection;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Interfaces.Repositories;

public interface IFeedbackRepository : IScopedService
{
    Task<Guid> AddReviewAsync(Feedback feedback, CancellationToken cancellationToken);
    Task<bool> HasUserReviewedProductAsync(Guid userId, Guid productId, CancellationToken cancellationToken);
    Task<Feedback?> GetByIdAsync(Guid feedbackId, CancellationToken cancellationToken);
    Task<(double AverageRating, int TotalReviews)> GetProductRatingAsync(Guid productId, CancellationToken cancellationToken);
}
