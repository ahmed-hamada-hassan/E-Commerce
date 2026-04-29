using E_Commerce.Application.Interfaces.Repositories;
using FluentValidation;

namespace E_Commerce.Application.Features.Products.Command.UpdateProduct;

internal sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product Id is required.")
            .NotEqual(Guid.Empty).WithMessage("Product Id cannot be an empty GUID.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Category ID cannot be an empty GUID.")
            .NotEqual(x => x.ProductId).WithMessage("Category ID cannot be the same as Product ID.");

        RuleFor(x => x.Name)
            .MinimumLength(3).WithMessage("Product name must be at least 3 characters.")
            .MaximumLength(150).WithMessage("Product name must not exceed 150 characters.")
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Product description must not exceed 2000 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than zero.");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Stock quantity cannot be negative.");

        RuleFor(x => x.SKU)
            .MinimumLength(3).WithMessage("Product SKU must be at least 3 characters.")
            .MaximumLength(50).WithMessage("SKU must not exceed 50 characters.")
            .When(x => !string.IsNullOrEmpty(x.SKU));

        RuleFor(x => x.Barcode)
            .MinimumLength(3).WithMessage("Product barcode must be at least 3 characters.")
            .MaximumLength(100).WithMessage("Barcode must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.Barcode));
    }
}