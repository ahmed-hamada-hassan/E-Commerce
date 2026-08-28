namespace E_Commerce.Application.Features.Orders.DTOs;

public record CancellationResponse(
    DateTimeOffset CancellationDate,
    string Reason
);