using E_Commerce.Application.Features.Orders.DTOs;
using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Orders.Commands.Place_Order;

public record PlaceOrderCommand(Guid UserId, bool? UseDefaulShippingAddress, 
    Guid? AddressId, PlaceOrderAddress? NewAddress, PaymentMethod PaymentMethod) : IRequest<Result<Guid>>;