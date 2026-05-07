using FluentValidation;

namespace E_Commerce.Application.Features.Categories.Commands.UpdateCategory;

internal sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(c => c.Name)
            .MaximumLength(150).WithMessage("Category Name must not exceed 150 characters.")
            .When(c => !string.IsNullOrWhiteSpace(c.Name));

        RuleFor(c => c.Description)
            .MaximumLength(1000).WithMessage("Category Description must not exceed 1000 characters.")
            .When(c => !string.IsNullOrWhiteSpace(c.Description));

        RuleFor(c => c.ParentId)
            .NotEqual(Guid.Empty)
            .WithMessage("Parent Category Id cannot be an empty GUID.")
            .When(c => c.ParentId.HasValue);

        RuleFor(c => c.ParentId)
            .NotEqual(c => c.Id)
            .WithMessage("A category cannot be its own parent.")
            .When(c => c.ParentId.HasValue);
    }
}
