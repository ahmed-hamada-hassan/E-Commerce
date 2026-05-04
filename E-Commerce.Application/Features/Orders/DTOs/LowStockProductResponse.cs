namespace E_Commerce.Application.Features.Orders.DTOs;

public record LowStockProductResponse(Guid Id, string Name, int CurrentStock);