using E_Commerce.Application.Common;
using Microsoft.Extensions.Options;

namespace E_Commerce.Application.Features.Orders.Queries.Get_My_Order;

internal sealed class GetMyOrderSummaryQueryValidator : CursorPaginationValidator<GetMyOrderSummaryQuery, Guid>
{
    public GetMyOrderSummaryQueryValidator(IOptionsSnapshot<PaginationSettings> paginationSettings) : base(paginationSettings)
    {
    }
}
