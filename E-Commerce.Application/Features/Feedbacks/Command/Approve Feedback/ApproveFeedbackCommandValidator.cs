using FluentValidation;

namespace E_Commerce.Application.Features.Feedbacks.Command.Approve_Feedback;

internal sealed class ApproveFeedbackCommandValidator : AbstractValidator<ApproveFeedbackCommand>
{
    public ApproveFeedbackCommandValidator()
    {
        RuleFor(c => c.AdminId)
            .NotEmpty().WithMessage("AdminId is required.");

        RuleFor(x => x.FeedbackId)
            .NotEmpty().WithMessage("FeedbackId is required.");
    }
}