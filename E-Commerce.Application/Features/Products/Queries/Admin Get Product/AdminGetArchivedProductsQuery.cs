using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Products.Queries.Admin_Get_Product;

public record AdminGetArchivedProductsQuery(Guid? VendorId, Guid? Cursor, int Size) : 
    IRequest<Result<CursorPagedResult<AdminArchivedProductResponse, Guid>>>, ICursorPaginationRequest<Guid>;