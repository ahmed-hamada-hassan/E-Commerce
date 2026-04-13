using E_Commerce.Application.Common;
using Microsoft.Extensions.Options;

namespace E_Commerce.Application.Features.Categories.Queries.GetCategories;

internal sealed class GetDeletedCategoriesQueryValidator : CursorPaginationValidator<GetDeletedCategoriesQuery, Guid>
{
    public GetDeletedCategoriesQueryValidator(IOptionsSnapshot<PaginationSettings> paginationSettings) : base(paginationSettings)
    {
    }
}
