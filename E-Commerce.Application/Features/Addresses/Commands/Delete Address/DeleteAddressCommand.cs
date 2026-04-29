using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Addresses.Commands.Delete_Address;

public record DeleteAddressCommand(Guid UserId, Guid AddressId) : IRequest<Result<bool>>;