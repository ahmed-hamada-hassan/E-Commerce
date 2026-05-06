using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Feedbacks.Command.Approve_Feedback;

internal sealed class ApproveFeedbackCommandHandler : IRequestHandler<ApproveFeedbackCommand, Result<bool>>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ApproveFeedbackCommandHandler> _logger;

    public ApproveFeedbackCommandHandler(IFeedbackRepository feedbackRepository, IUnitOfWork unitOfWork, ILogger<ApproveFeedbackCommandHandler> logger)
    {
        _feedbackRepository = feedbackRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(ApproveFeedbackCommand request, CancellationToken cancellationToken)
    {
        var feedback = await _feedbackRepository.GetByIdAsync(request.FeedbackId, cancellationToken);
        if(feedback is null)
        {
            _logger.LogWarning("Feedback with id {FeedbackId} not found", request.FeedbackId);
            return Result<bool>.Failure(FeedbackErrors.NotFound);
        }

        feedback.Approve(request.AdminId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true);
    }
}
