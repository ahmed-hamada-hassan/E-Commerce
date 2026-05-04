using E_Commerce.Application.Features.Orders.DTOs;
using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Application.Features.Orders.Queries.Admin_Get_Order;

internal sealed class GetAdminOverviewQueryHandler : IRequestHandler<GetAdminOverviewQuery, Result<AdminOverviewResponse>>
{
    private readonly IAppDbContext _appDbContext;

    public GetAdminOverviewQueryHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Result<AdminOverviewResponse>> Handle(GetAdminOverviewQuery request, CancellationToken cancellationToken)
    {
        var baseQuery = _appDbContext.Orders
            .AsNoTracking()
            .OrderByDescending(o => o.OrderedDate)
            .AsQueryable();

        if (request.FromDate.HasValue)
            baseQuery = baseQuery.Where(o => o.OrderedDate >= request.FromDate.Value);

        if (request.ToDate.HasValue)
            baseQuery = baseQuery.Where(o => o.OrderedDate <= request.ToDate.Value);

        var statsList = await baseQuery.Select(o => new OverViewTable
        (
            o.Id,
            o.TotalAmount,
            o.Status,
            o.Payment!.PaymentStatus,
            o.Refunds.Count(r => r.RefundStatus == RefundStatus.Success),
            o.Refunds.Count(r => r.RefundStatus == RefundStatus.Failed),
            o.Refunds.Count(r => r.RefundStatus == RefundStatus.Pending),
            o.Refunds.Where(r => r.RefundStatus == RefundStatus.Pending).Sum(r => r.Amount),
            o.Refunds.Where(r => r.RefundStatus == RefundStatus.Success).Sum(r => r.Amount),
            o.ReturnRequests.Count(r => r.Status == ReturnStatus.Completed),
            o.ReturnRequests.Count(r => r.Status == ReturnStatus.Approved),
            o.ReturnRequests.Count(r => r.Status == ReturnStatus.Rejected),
            o.ReturnRequests.Count(r => r.Status == ReturnStatus.Pending)
        )).ToListAsync(cancellationToken);

        var lowStockProducts = await _appDbContext.Products
            .AsNoTracking()
            .Where(p => p.StockQuantity <= 5)
            .Select(p => new LowStockProductResponse(p.Id, p.Name, p.StockQuantity))
            .ToListAsync(cancellationToken);

        var paymentedOrders = statsList.Where(s => s.PaymentStatus == PaymentStatus.Completed)
            .Sum(s => s.TotalAmount);
        var refundedAmount = statsList.Sum(s => s.AlreadyRefundedAmount);

        var response = new AdminOverviewResponse(
            TotalRevenue: paymentedOrders - refundedAmount,
            TotalOrders: statsList.Count(),
            PendingOrders: statsList.Count(s => s.OrderStatus == OrderStatus.Pending),
            ProcessingOrders: statsList.Count(s => s.OrderStatus == OrderStatus.Processing),
            ShippedOrders: statsList.Count(s => s.OrderStatus == OrderStatus.Shipped),
            DeliveredOrders: statsList.Count(s => s.OrderStatus == OrderStatus.Delivered),
            CancelledOrders: statsList.Count(s => s.OrderStatus == OrderStatus.Cancelled),
            PendingReturnedOrders: statsList.Sum(s => s.PendingReturnRequests),
            ApprovedReturnedOrders: statsList.Sum(s => s.ApprovedReturnRequests),
            RejectedReturnedOrders: statsList.Sum(s => s.RejectedReturnRequests),
            CompletedReturnedOrders: statsList.Sum(s => s.CompletedReturnRequests),
            TotalReturnedOrders: statsList.Sum(s => s.PendingReturnRequests + s.ApprovedReturnRequests + s.RejectedReturnRequests + s.CompletedReturnRequests),
            PendingRefundOrders: statsList.Sum(s => s.PendingRefunds),
            SucceededRefundOrders: statsList.Sum(s => s.SuccessfulRefunds),
            FailedRefundOrders: statsList.Sum(s => s.FailedRefunds),
            TotalRefundedOrders: statsList.Sum(s => s.PendingRefunds + s.SuccessfulRefunds + s.FailedRefunds),
            TotalRefundAmount: refundedAmount,
            LowStockProducts: lowStockProducts
        );

        return Result<AdminOverviewResponse>.Success(response);
    }
}


public record OverViewTable(
    Guid OrderId,
    decimal TotalAmount,
    OrderStatus OrderStatus,
    PaymentStatus PaymentStatus,
    int SuccessfulRefunds,
    int FailedRefunds,
    int PendingRefunds,
    decimal PendingRefundAmount,
    decimal AlreadyRefundedAmount,
    int CompletedReturnRequests,
    int ApprovedReturnRequests,
    int RejectedReturnRequests,
    int PendingReturnRequests
);