using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Addresses.Commands.Set_Address_Default_Shipping;

public record SetAddressDefaultShippingCommand(Guid UserId, Guid AddressId) : IRequest<Result<bool>>;