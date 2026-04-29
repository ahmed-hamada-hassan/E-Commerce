using E_Commerce.Application.Features.Addresses.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Addresses.Commands.Update_Address;

public record UpdateAddressCommand(Guid UserId, Guid AddressId, UpdateAddressInfo AddressInfo) : IRequest<Result<bool>>;