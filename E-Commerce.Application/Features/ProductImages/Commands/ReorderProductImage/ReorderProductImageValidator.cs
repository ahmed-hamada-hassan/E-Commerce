using FluentValidation;

namespace E_Commerce.Application.Features.ProductImages.Commands.ReorderProductImage;

internal sealed class ReorderProductImageValidator : AbstractValidator<ReorderProductImageCommand>
{
    public ReorderProductImageValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required.");

        RuleFor(x => x.VendorId)
            .NotEmpty()
            .WithMessage("Vendor ID is required.");

        RuleFor(x => x.Images)
            .NotEmpty()
            .WithMessage("At least one image order must be provided.")
            .Must(imgs => imgs.Select(i => i.displayOrder).Distinct().Count() == imgs.Count())
            .WithMessage("Display orders must be unique.");

        RuleForEach(x => x.Images).ChildRules(image =>
        {
            image.RuleFor(i => i.imageId)
                .NotEmpty()
                .WithMessage("Image ID is required.")
                .NotEqual(Guid.Empty)
                .WithMessage("Image ID cannot be an empty GUID.");

            image.RuleFor(i => i.displayOrder)
            .InclusiveBetween((byte)1, (byte)7)
            .WithMessage("Display order must be between 1 and 7.");
        });
    }
}
