using E_Commerce.Application.Common;
using Microsoft.Extensions.Options;

namespace E_Commerce.Application.Features.Users.Queries.Admin_Get_Customer;

internal sealed class AdminGetCustomersQueryValidator : CursorPaginationValidator<AdminGetCustomersQuery, Guid>
{
    public AdminGetCustomersQueryValidator(IOptionsSnapshot<PaginationSettings> paginationSettings) : base(paginationSettings)
    {
    }
}
