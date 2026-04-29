using E_Commerce.Application.Features.Addresses.DTOs;
using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Addresses.Commands;

public record AddAddressCommand(Guid UserId, List<AddAddressInfo> Addresses) : IRequest<Result<List<Guid>>>;