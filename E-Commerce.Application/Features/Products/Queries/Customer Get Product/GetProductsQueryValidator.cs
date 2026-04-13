using E_Commerce.Application.Common;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace E_Commerce.Application.Features.Products.Queries.GetProducts;

internal sealed class GetProductsQueryValidator : OffsetPaginationValidator<GetProductsQuery>
{
    public GetProductsQueryValidator(IOptionsSnapshot<PaginationSettings> paginationSettings) : base(paginationSettings)
    {
        RuleFor(x => x.SearchTerm)
            .MaximumLength(100).WithMessage("Search term must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm));

        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum price cannot be less than zero.")
            .When(x => x.MinPrice.HasValue);

        RuleFor(x => x.MaxPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Maximum price cannot be less than zero.")
            .GreaterThanOrEqualTo(x => x.MinPrice)
            .WithMessage("Maximum price must be greater than or equal to minimum price.")
            .When(x => x.MaxPrice.HasValue && x.MinPrice.HasValue);

        RuleFor(x => x.SortBy)
            .Must(sortBy => string.IsNullOrEmpty(sortBy) || new[] { "price_asc", "price_desc", "name_asc", "name_desc" }.Contains(sortBy))
            .WithMessage("SortBy must be one of the following: price_asc, price_desc, name_asc, name_desc.")
            .When(x => !string.IsNullOrWhiteSpace(x.SortBy));
    }
}
