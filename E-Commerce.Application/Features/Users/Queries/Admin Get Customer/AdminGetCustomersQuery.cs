using E_Commerce.Application.Features.Users.DTOs;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Users.Queries.Admin_Get_Customer;

public record AdminGetCustomersQuery(Guid? Cursor, int Size, string? SearchTerm, string? Status) :
    IRequest<Result<CursorPagedResult<AdminCustomersResponse, Guid>>>, ICursorPaginationRequest<Guid>;
