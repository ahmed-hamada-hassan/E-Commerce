using E_Commerce.Application.Common;
using Microsoft.Extensions.Options;

namespace E_Commerce.Application.Features.Products.Queries.Vendor_Get_Product;

internal sealed class VendorGetArchivedProductsQueryValidator : CursorPaginationValidator<VendorGetArchivedProductsQuery, Guid>
{
    public VendorGetArchivedProductsQueryValidator(IOptionsSnapshot<PaginationSettings> paginationSettings) : base(paginationSettings)
    {
    }
}
