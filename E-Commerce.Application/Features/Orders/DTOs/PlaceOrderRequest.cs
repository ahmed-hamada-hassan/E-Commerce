using E_Commerce.Domain.Enums;

namespace E_Commerce.Application.Features.Orders.DTOs;

public record PlaceOrderRequest(bool? UseDefaultShippingAddress, Guid? AddressId, PlaceOrderAddress? NewAddress, PaymentMethod PaymentMethod);