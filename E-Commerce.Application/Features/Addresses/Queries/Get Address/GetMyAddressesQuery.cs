using E_Commerce.Application.Features.Addresses.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Addresses.Queries.Get_Address;

public record GetMyAddressesQuery(Guid UserId) : IRequest<Result<List<GetAddressInfo>>>;