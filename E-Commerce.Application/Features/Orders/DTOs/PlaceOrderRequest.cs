namespace E_Commerce.Application.Features.Orders.DTOs;

public record PlaceOrderRequest(bool? UseDefaultShippingAddress, Guid? AddressId, PlaceOrderAddress? NewAddress);