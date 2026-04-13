using FluentValidation;

namespace E_Commerce.Application.Features.ProductImages.Commands.AddImage;

internal sealed class AddImageCommandValidator : AbstractValidator<AddImageCommand>
{
    public AddImageCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product ID is required.");

        RuleFor(x => x.Images)
            .NotEmpty().WithMessage("At least one image is required.")
            .Must(imgs => imgs.Count() <= 7).WithMessage("You can upload a maximum of 7 images.");

        RuleForEach(x => x.Images).ChildRules(image =>
        {
            image.RuleFor(i => i.Image)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Image file is required.")
                .Must(img => img.Length <= 5 * 1024 * 1024).WithMessage("Image size must not exceed 5MB.")
                .Must(img => img.ContentType.StartsWith("image/")).WithMessage("Invalid file format. Please upload an image.")
                .Must(img =>
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                    var extension = Path.GetExtension(img.FileName).ToLowerInvariant();
                    return allowedExtensions.Contains(extension);
                }).WithMessage("Only JPG, JPEG, PNG, and WEBP formats are allowed.");

            image.RuleFor(i => i.IsPrimary)
                .NotNull().WithMessage("IsPrimary must be specified.");

            image.RuleFor(i => (int)i.DisplayOrder)
                .InclusiveBetween(1, 254).WithMessage("Display order must be between 1 and 254.");
        });
    }
}
