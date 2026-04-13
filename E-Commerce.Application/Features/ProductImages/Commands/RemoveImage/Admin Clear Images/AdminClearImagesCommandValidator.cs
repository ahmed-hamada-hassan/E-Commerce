using FluentValidation;

namespace E_Commerce.Application.Features.ProductImages.Commands.RemoveImage;

internal sealed class AdminClearImagesCommandValidator : AbstractValidator<AdminClearImagesCommand>
{
    public AdminClearImagesCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required.");
    }
}