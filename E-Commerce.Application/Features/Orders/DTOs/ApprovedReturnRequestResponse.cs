namespace E_Commerce.Application.Features.Orders.DTOs;

public record ApprovedReturnRequestResponse(
    Guid ReturnRequestId,
    Guid OrderId,
    string CustomerName,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal TotalRefundAmount,
    string Reason,
    DateTimeOffset RequestedDate
);