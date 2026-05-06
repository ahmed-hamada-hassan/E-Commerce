using FluentValidation;

namespace E_Commerce.Application.Features.Feedbacks.Command.Delete_Feedback;

internal sealed class DeleteFeedbackCommandValidator : AbstractValidator<DeleteFeedbackCommand>
{
    public DeleteFeedbackCommandValidator()
    {
        RuleFor(x => x.FeedbackId)
            .NotEmpty().WithMessage("FeedbackId is required.");
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
    }
}