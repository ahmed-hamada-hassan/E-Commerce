using E_Commerce.Application.Features.Users.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Users.Queries.Admin_Get_Customer;

public record AdminGetCustomerQuery(Guid UserId) : IRequest<Result<AdminCustomerResponse>>;
