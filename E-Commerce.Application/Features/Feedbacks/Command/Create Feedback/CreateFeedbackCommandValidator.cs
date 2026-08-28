using FluentValidation;

namespace E_Commerce.Application.Features.Feedbacks.Command.CreateFeedback;

internal sealed class CreateFeedbackCommandValidator : AbstractValidator<CreateFeedbackCommand>
{
    public CreateFeedbackCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");
        RuleFor(x => x.Rating)
            .InclusiveBetween((byte)1, (byte)5).WithMessage("Rating must be between 1 and 5.");
        RuleFor(x => x.Comment)
            .MinimumLength(5).WithMessage("Comment must be at least 5 characters long.")
            .MaximumLength(1000).WithMessage("Comment cannot exceed 1000 characters.");
    }
}
