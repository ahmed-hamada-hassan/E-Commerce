namespace E_Commerce.Application.Features.Orders.DTOs;

public record ReturnRequestItemsDto(Guid ProductId, byte Quantity);