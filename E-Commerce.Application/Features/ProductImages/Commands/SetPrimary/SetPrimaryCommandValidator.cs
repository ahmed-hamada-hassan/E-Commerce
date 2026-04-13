using FluentValidation;

namespace E_Commerce.Application.Features.ProductImages.Commands.SetPrimary;

internal sealed class SetPrimaryCommandValidator : AbstractValidator<SetPrimaryCommand>
{
    public SetPrimaryCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product ID is required.");
        RuleFor(x => x.VendorId).NotEmpty().WithMessage("Vendor ID is required.");
        RuleFor(x => x.ImageId).NotEmpty().WithMessage("Image ID is required.");
    }
}
