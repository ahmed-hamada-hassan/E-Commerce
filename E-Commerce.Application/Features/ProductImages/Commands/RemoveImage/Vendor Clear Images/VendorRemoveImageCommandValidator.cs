using FluentValidation;

namespace E_Commerce.Application.Features.ProductImages.Commands.RemoveImage;

internal sealed class VendorRemoveImageCommandValidator : AbstractValidator<VendorRemoveImageCommand>
{
    public VendorRemoveImageCommandValidator()
    {
        RuleFor(x => x.ImgaeId)
            .NotEmpty().WithMessage("Image ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Image ID cannot be an empty GUID.");

        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Product ID cannot be an empty GUID.");

        RuleFor(x => x.VendorId)
            .NotEmpty().WithMessage("Vendor ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Vendor ID cannot be an empty GUID.");
    }
}