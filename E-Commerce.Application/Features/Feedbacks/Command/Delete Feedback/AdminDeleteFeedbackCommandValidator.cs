using FluentValidation;

namespace E_Commerce.Application.Features.Feedbacks.Command.Delete_Feedback;

internal sealed class AdminDeleteFeedbackCommandValidator : AbstractValidator<AdminDeleteFeedbackCommand>
{
    public AdminDeleteFeedbackCommandValidator()
    {
        RuleFor(x => x.FeedbackId)
            .NotEmpty().WithMessage("FeedbackId is required.");
        RuleFor(x => x.AdminId)
            .NotEmpty().WithMessage("AdminId is required.");
    }
}
