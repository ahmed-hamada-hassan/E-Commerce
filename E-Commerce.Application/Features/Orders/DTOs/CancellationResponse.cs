namespace E_Commerce.Application.Features.Orders.DTOs;

public record CancellationResponse(
    DateTime CancellationDate,
    string Reason
);