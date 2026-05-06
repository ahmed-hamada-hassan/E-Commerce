using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Feedbacks.Command.CreateFeedback;

internal sealed class CreateFeedbackCommandHandler : IRequestHandler<CreateFeedbackCommand, Result<Guid>>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateFeedbackCommandHandler> _logger;

    public CreateFeedbackCommandHandler(IFeedbackRepository feedbackRepository, IOrderRepository orderRepository, 
        IUnitOfWork unitOfWork, ILogger<CreateFeedbackCommandHandler> logger)
    {
        _feedbackRepository = feedbackRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateFeedbackCommand request, CancellationToken cancellationToken)
    {
        var HasReviewedBefore = await _feedbackRepository.HasUserReviewedProductAsync(request.UserId, request.ProductId, cancellationToken);
        if(HasReviewedBefore)
        {
            _logger.LogWarning("User {UserId} has already reviewed product {ProductId}", request.UserId, request.ProductId);
            return Result<Guid>.Failure(FeedbackErrors.AlreadyReviewed);
        }

        var isVerified = await _orderRepository.HasUserPurchasedProductAsync(request.UserId, request.ProductId, cancellationToken);
        if(!isVerified)
        {
            _logger.LogWarning("User {UserId} has not purchased product {ProductId} and cannot leave feedback", request.UserId, request.ProductId);
            return Result<Guid>.Failure(FeedbackErrors.UserNotVerified);
        }

        var feedbackResult = Feedback.Create(request.UserId, request.ProductId, request.Rating, request.Comment, isVerified);
        if(feedbackResult.IsFailure)
        {
            _logger.LogWarning("Failed to create feedback for user {UserId} and product {ProductId}: {Error}", request.UserId, request.ProductId, feedbackResult.Error);
            return Result<Guid>.Failure(feedbackResult.Error);
        }

        var feedbackId = await _feedbackRepository.AddReviewAsync(feedbackResult.Value!, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(feedbackId);
    }
}
