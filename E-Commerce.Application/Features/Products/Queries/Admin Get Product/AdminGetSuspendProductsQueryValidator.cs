using E_Commerce.Application.Common;
using Microsoft.Extensions.Options;

namespace E_Commerce.Application.Features.Products.Queries.Admin_Get_Product;

internal sealed class AdminGetSuspendProductsQueryValidator : CursorPaginationValidator<AdminGetSuspendProductsQuery, Guid>
{
    public AdminGetSuspendProductsQueryValidator(IOptionsSnapshot<PaginationSettings> paginationSettings) : base(paginationSettings)
    {
    }
}
