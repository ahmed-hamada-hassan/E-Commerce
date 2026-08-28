using E_Commerce.Domain.Enums;

namespace E_Commerce.Application.Features.Orders.DTOs;

public record PlaceOrderRequest(Guid? CartId, bool? UseDefaultShippingAddress, Guid? AddressId, PlaceOrderAddress? NewAddress, PaymentMethod PaymentMethod);