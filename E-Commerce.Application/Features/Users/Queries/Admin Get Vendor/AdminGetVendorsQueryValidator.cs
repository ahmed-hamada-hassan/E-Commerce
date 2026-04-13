using E_Commerce.Application.Common;
using Microsoft.Extensions.Options;

namespace E_Commerce.Application.Features.Users.Queries.Admin_Get_Vendor;

internal sealed class AdminGetVendorsQueryValidator : CursorPaginationValidator<AdminGetVendorsQuery, Guid>
{
    public AdminGetVendorsQueryValidator(IOptionsSnapshot<PaginationSettings> paginationSettings) : base(paginationSettings)
    {
    }
}
