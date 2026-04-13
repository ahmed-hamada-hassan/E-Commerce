using E_Commerce.Application.Common;
using Microsoft.Extensions.Options;

namespace E_Commerce.Application.Features.Products.Queries.Admin_Get_Product;

internal sealed class AdminGetProductsQueryValidator : CursorPaginationValidator<AdminGetProductsQuery, Guid>
{
    public AdminGetProductsQueryValidator(IOptionsSnapshot<PaginationSettings> paginationSettings) : base(paginationSettings)
    {
    }
}
