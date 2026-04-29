using E_Commerce.Application.Common;
using Microsoft.Extensions.Options;

namespace E_Commerce.Application.Features.Orders.Queries.Admin_Get_Order;

internal sealed class GetSummaryProcessingOrdersQueryValidator : CursorPaginationValidator<GetSummaryProcessingOrdersQuery, Guid>
{
    public GetSummaryProcessingOrdersQueryValidator(IOptionsSnapshot<PaginationSettings> paginationSettings) : base(paginationSettings)
    {
    }
}
