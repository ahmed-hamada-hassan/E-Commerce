using E_Commerce.Application.Features.Addresses.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Addresses.Queries.Get_Address;

public record GetAddressQuery(Guid UserId, Guid AddressId) : IRequest<Result<GetAddressInfo>>;