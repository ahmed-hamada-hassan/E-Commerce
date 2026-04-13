using E_Commerce.Application.Common;
using Microsoft.Extensions.Options;

namespace E_Commerce.Application.Features.Categories.Queries.Public_Get_Categories;

internal class PublicGetCategoriesQueryValidator : CursorPaginationValidator<PublicGetCategoriesQuery, Guid>
{
    public PublicGetCategoriesQueryValidator(IOptionsSnapshot<PaginationSettings> paginationSettings) : base(paginationSettings)
    {
    }
}
