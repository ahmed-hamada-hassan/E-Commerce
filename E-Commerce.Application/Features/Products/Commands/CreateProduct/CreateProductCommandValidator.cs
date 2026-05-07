using E_Commerce.Application.Interfaces.Repositories;
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

        //RuleFor(x => x.Images)
        //    .Cascade(CascadeMode.Stop)
        //    .NotEmpty().WithMessage("At least one product image is required.")
        //    .Must(imgs => imgs.Count(i => i.IsPrimary) == 1)
        //    .WithMessage("Exactly one primary image must be specified.")
        //    .Must(imgs => imgs.Select(i => i.DisplayOrder).Distinct().Count() == imgs.Count())
        //    .WithMessage("Each image must have a unique display order.")
        //    .Must(imgs => imgs.Count() <= 7)
        //    .WithMessage("A maximum of 7 images can be uploaded.");

        //RuleForEach(x => x.Images).ChildRules(image =>
        //{
        //    image.RuleFor(i => i.Image)
        //        .Cascade(CascadeMode.Stop)
        //        .NotNull().WithMessage("Image file is required.")
        //        .Must(img => img.Length <= 5 * 1024 * 1024).WithMessage("Image size must not exceed 5MB.")
        //        .Must(img => img.ContentType.StartsWith("image/")).WithMessage("Invalid file format. Please upload an image.")
        //        .Must(img =>
        //        {
        //            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        //            var extension = Path.GetExtension(img.FileName).ToLowerInvariant();
        //            return allowedExtensions.Contains(extension);
        //        }).WithMessage("Only JPG, JPEG, PNG, and WEBP formats are allowed.");

        //    image.RuleFor(i => i.IsPrimary)
        //        .NotNull().WithMessage("IsPrimary must be specified.");

        //    image.RuleFor(i => (int)i.DisplayOrder)
        //        .InclusiveBetween(1, 254).WithMessage("Display order must be between 1 and 254.");
        //});
    }
}
