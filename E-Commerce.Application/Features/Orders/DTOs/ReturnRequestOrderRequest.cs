namespace E_Commerce.Application.Features.Orders.DTOs;

public record ReturnRequestOrderRequest(List<ReturnRequestItemsDto> Items, string Reason);