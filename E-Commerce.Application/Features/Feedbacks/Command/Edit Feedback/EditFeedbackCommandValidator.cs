using FluentValidation;

namespace E_Commerce.Application.Features.Feedbacks.Command.Edit_Feedback;

internal sealed class EditFeedbackCommandValidator : AbstractValidator<EditFeedbackCommand>
{
    public EditFeedbackCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
        RuleFor(x => x.FeedbackId)
            .NotEmpty().WithMessage("FeedbackId is required.");
        RuleFor(x => x.Rating)
            .InclusiveBetween((byte)1, (byte)5).When(x => x.Rating.HasValue).WithMessage("Rating must be between 1 and 5.");
        RuleFor(x => x.Comment)
            .MaximumLength(1000).When(x => !string.IsNullOrEmpty(x.Comment)).WithMessage("Comment cannot exceed 1000 characters.");
    }
}
