namespace E_Commerce.Application.Features.Orders.DTOs;

public record AdminOverviewResponse(
    decimal TotalRevenue,
    int TotalOrders,
    int PendingOrders,
    int ProcessingOrders,
    int ShippedOrders,
    int DeliveredOrders,
    int CancelledOrders,
    int PendingReturnedOrders,
    int ApprovedReturnedOrders,
    int RejectedReturnedOrders,
    int CompletedReturnedOrders,
    int TotalReturnedOrders,
    int PendingRefundOrders,
    int SucceededRefundOrders,
    int FailedRefundOrders,
    int TotalRefundedOrders,
    decimal TotalRefundAmount,
    List<LowStockProductResponse> LowStockProducts  
);
