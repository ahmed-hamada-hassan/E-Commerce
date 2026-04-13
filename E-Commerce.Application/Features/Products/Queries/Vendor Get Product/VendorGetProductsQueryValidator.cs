using E_Commerce.Application.Common;
using Microsoft.Extensions.Options;

namespace E_Commerce.Application.Features.Products.Queries.Vendor_Get_Product;

internal sealed class VendorGetProductsQueryValidator : CursorPaginationValidator<VendorGetProductsQuery, Guid>
{
    public VendorGetProductsQueryValidator(IOptionsSnapshot<PaginationSettings> paginationSettings) : base(paginationSettings)
    {
    }
}
