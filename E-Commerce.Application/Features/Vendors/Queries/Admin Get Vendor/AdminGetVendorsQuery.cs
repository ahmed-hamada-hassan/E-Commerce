using E_Commerce.Application.Features.Vendors.DTOs;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Vendors.Queries.Admin_Get_Vendor;

public record AdminGetVendorsQuery(Guid? Cursor, int Size, string? SearchTerm, string? Status) :
    IRequest<Result<CursorPagedResult<AdminVendorsResponse, Guid>>>, ICursorPaginationRequest<Guid>;
