using E_Commerce.Application.Common;
using Microsoft.Extensions.Options;

namespace E_Commerce.Application.Features.Orders.Queries.Get_Approved_Return_Requests_For_Representative;

internal sealed class GetApprovedReturnRequestsForRepresentativeQueryValidator :
    CursorPaginationValidator<GetApprovedReturnRequestsForRepresentativeQuery, Guid>
{
    public GetApprovedReturnRequestsForRepresentativeQueryValidator(IOptionsSnapshot<PaginationSettings> paginationSettings) : base(paginationSettings)
    {
    }
}
