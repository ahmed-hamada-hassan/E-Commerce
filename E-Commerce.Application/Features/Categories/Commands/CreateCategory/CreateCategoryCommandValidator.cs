using FluentValidation;

namespace E_Commerce.Application.Features.Categories.Commands.CreateCategory;

internal sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Category Name is required.")
            .MaximumLength(150).WithMessage("Category Name must not exceed 150 characters.");

        RuleFor(c => c.Description)
            .MaximumLength(1000).WithMessage("Category Description must not exceed 1000 characters.");

        RuleFor(x => x.ImageUrl)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .When(x => !string.IsNullOrEmpty(x.ImageUrl))
            .WithMessage("ImageUrl must be a valid URL.");

        RuleFor(c => c.ParentCategoryId)
            .NotEqual(Guid.Empty).WithMessage("Parent Category Id must be a valid GUID.")
            .When(c => c.ParentCategoryId.HasValue);
    }
}
