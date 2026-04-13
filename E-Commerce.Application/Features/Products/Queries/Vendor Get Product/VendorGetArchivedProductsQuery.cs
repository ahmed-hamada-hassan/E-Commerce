using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Products.Queries.Vendor_Get_Product;

public record VendorGetArchivedProductsQuery(Guid VendorId, Guid? Cursor, int Size) :
    IRequest<Result<CursorPagedResult<VendorArchivedProductResponse, Guid>>>, ICursorPaginationRequest<Guid>;