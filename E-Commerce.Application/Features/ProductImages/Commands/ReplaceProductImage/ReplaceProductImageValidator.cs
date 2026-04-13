using FluentValidation;

namespace E_Commerce.Application.Features.ProductImages.Commands.ReplaceProductImage;

internal sealed class ReplaceProductImageValidator : AbstractValidator<ReplaceProductImageCommand>
{
    public ReplaceProductImageValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");

        RuleFor(x => x.VendorId)
            .NotEmpty().WithMessage("VendorId is required.");

        RuleFor(x => x.ImageId)
            .NotEmpty().WithMessage("ImageId is required.");

        RuleFor(x => x.NewImage)
            .NotNull().WithMessage("NewImage is required.")
            .Must(file => file.Length > 0).WithMessage("NewImage cannot be empty.");
    }
}