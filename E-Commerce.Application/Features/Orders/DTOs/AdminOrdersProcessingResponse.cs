using E_Commerce.Domain.Shared;

namespace E_Commerce.Application.Features.Orders.DTOs;

public class AdminOrdersProcessingResponse : CursorPagedResult<AdminProcessingOrderSummaryResponse, Guid>
{
    public int TotalProcessingCount { get; init; }
    public int DayProcessingCount { get; init; }

    public AdminOrdersProcessingResponse(
        IReadOnlyCollection<AdminProcessingOrderSummaryResponse> items,
        Guid? nextCursor,
        int totalProcessingCount,
        int dayProcessingCount)
        : base(items, nextCursor)
    {
        TotalProcessingCount = totalProcessingCount;
        DayProcessingCount = dayProcessingCount;
    }
}