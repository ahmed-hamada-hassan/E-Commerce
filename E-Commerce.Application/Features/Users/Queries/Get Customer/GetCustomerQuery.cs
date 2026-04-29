using E_Commerce.Application.Features.Users.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Users.Queries.Get_Customer;

public record GetCustomerQuery(Guid UserId) : IRequest<Result<CustomerProfileResponse>>;
