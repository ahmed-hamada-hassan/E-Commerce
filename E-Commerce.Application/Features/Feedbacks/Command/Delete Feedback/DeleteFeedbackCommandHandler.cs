using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Feedbacks.Command.Delete_Feedback;

internal sealed class DeleteFeedbackCommandHandler : IRequestHandler<DeleteFeedbackCommand, Result<bool>>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteFeedbackCommandHandler> _logger;

    public DeleteFeedbackCommandHandler(IFeedbackRepository feedbackRepository, IUnitOfWork unitOfWork, ILogger<DeleteFeedbackCommandHandler> logger)
    {
        _feedbackRepository = feedbackRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(DeleteFeedbackCommand request, CancellationToken cancellationToken)
    {
        var feedback = await _feedbackRepository.GetByIdAsync(request.FeedbackId, cancellationToken);
        if(feedback is null)
        {
            _logger.LogWarning("Feedback with ID {FeedbackId} not found.", request.FeedbackId);
            return Result<bool>.Failure(FeedbackErrors.NotFound);
        }
        if(feedback.UserId != request.UserId)
        {
            _logger.LogWarning("User with ID {UserId} is not authorized to delete feedback with ID {FeedbackId}.", request.UserId, request.FeedbackId);
            return Result<bool>.Failure(FeedbackErrors.AccessDenied);
        }

        feedback.Delete();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}