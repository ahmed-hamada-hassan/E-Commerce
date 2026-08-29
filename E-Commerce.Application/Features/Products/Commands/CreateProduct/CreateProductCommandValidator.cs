using FluentValidation;

namespace E_Commerce.Application.Features.Products.Command.CreateProduct;

internal sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.VendorId)
            .NotEmpty()
            .WithMessage("Vendor ID is required.")
            .NotEqual(Guid.Empty)
            .WithMessage("Vendor ID cannot be an empty GUID.");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category ID is required.")
            .NotEqual(Guid.Empty)
            .WithMessage("Category ID cannot be an empty GUID.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MinimumLength(3).WithMessage("Product name must be at least 3 characters.")
            .MaximumLength(150).WithMessage("Product name must not exceed 150 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .WithMessage("Product description must not exceed 2000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than zero.");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Stock quantity cannot be negative.");

        RuleFor(x => x.SKU)
            .NotEmpty().WithMessage("SKU is required.")
            .MinimumLength(3).WithMessage("Product SKU must be at least 3 characters.")
            .MaximumLength(50).WithMessage("SKU must not exceed 50 characters.");

        RuleFor(x => x.Barcode)
            .MinimumLength(3).WithMessage("Product barcode must be at least 3 characters.")
            .MaximumLength(100).WithMessage("Barcode must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Barcode));
    }
}
