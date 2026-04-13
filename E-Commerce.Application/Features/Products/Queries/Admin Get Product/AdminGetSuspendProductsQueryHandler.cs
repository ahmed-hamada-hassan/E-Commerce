using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Products.Queries.Admin_Get_Product;

internal class AdminGetSuspendProductsQueryHandler : 
    IRequestHandler<AdminGetSuspendProductsQuery, Result<CursorPagedResult<AdminSuspendProductResponse, Guid>>>
{
    private readonly IProductRepository _productRepository;

    public AdminGetSuspendProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<CursorPagedResult<AdminSuspendProductResponse, Guid>>> Handle(AdminGetSuspendProductsQuery request, 
        CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAdminSuspendedProductsAsync(request.VendorId, request.Cursor, 
            request.Size, cancellationToken);

        var pagedResponse = new CursorPagedResult<AdminSuspendProductResponse, Guid>(
            products.Items.Select(p => p.ToAdminSuspendProductResponse()).ToList(),
            products.NextCursor);

        return Result<CursorPagedResult<AdminSuspendProductResponse, Guid>>.Success(pagedResponse);
    }
}