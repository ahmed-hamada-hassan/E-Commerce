using FluentValidation;

namespace E_Commerce.Application.Features.ProductImages.Commands.RemoveImage;

internal sealed class VendorClearImagesCommandValidator : AbstractValidator<VendorClearImagesCommand>
{
    public VendorClearImagesCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required.");
        RuleFor(x => x.VendorId).NotEmpty().WithMessage("VendorId is required.");
    }
}
