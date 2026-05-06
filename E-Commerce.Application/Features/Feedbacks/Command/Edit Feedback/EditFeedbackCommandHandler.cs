using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Feedbacks.Command.Edit_Feedback;

internal sealed class EditFeedbackCommandHandler : IRequestHandler<EditFeedbackCommand, Result<bool>>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EditFeedbackCommandHandler> _logger;

    public EditFeedbackCommandHandler(IFeedbackRepository feedbackRepository, IUnitOfWork unitOfWork, ILogger<EditFeedbackCommandHandler> logger)
    {
        _feedbackRepository = feedbackRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(EditFeedbackCommand request, CancellationToken cancellationToken)
    {
        var feedback = await _feedbackRepository.GetByIdAsync(request.FeedbackId, cancellationToken);
        if(feedback is null)
        {
            _logger.LogWarning("Feedback with ID {FeedbackId} not found.", request.FeedbackId);
            return Result<bool>.Failure(FeedbackErrors.NotFound);
        }

        if(feedback.UserId != request.UserId)
        {
            _logger.LogWarning("User with ID {UserId} attempted to edit feedback with ID {FeedbackId} which they do not own.", request.UserId, request.FeedbackId);
            return Result<bool>.Failure(FeedbackErrors.AccessDenied);
        }

        var editResult = feedback.Edit(request.Rating, request.Comment);
        if(editResult.IsFailure)
        {
            _logger.LogWarning("Failed to edit feedback with ID {FeedbackId}. Error: {Error}", request.FeedbackId, editResult.Error);
            return Result<bool>.Failure(editResult.Error);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
