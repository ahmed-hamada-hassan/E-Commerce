using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Products.Queries.Admin_Get_Product;

public record AdminGetProductsQuery(Guid? VendorId, Guid? Cursor, int Size) : 
    IRequest<Result<CursorPagedResult<AdminProductResponse, Guid>>>, ICursorPaginationRequest<Guid>;