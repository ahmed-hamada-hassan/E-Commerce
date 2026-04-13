using E_Commerce.Application.Common;
using Microsoft.Extensions.Options;

namespace E_Commerce.Application.Features.Categories.Queries.GetCategories;

internal sealed class GetCategoriesQueryValidator : CursorPaginationValidator<GetCategoriesQuery, Guid>
{
    public GetCategoriesQueryValidator(IOptionsSnapshot<PaginationSettings> paginationSettings) : base(paginationSettings)
    {
    }
}
