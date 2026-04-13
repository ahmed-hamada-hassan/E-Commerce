using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Products.Queries.Admin_Get_Product;

internal class AdminGetProductsQueryHandler : 
    IRequestHandler<AdminGetProductsQuery, Result<CursorPagedResult<AdminProductResponse, Guid>>>
{
    private readonly IProductRepository _productRepository;

    public AdminGetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<CursorPagedResult<AdminProductResponse, Guid>>> Handle(AdminGetProductsQuery request, 
        CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAdminAvailableProductsAsync(request.VendorId, request.Cursor, 
            request.Size, cancellationToken);

        var pagedResponse = new CursorPagedResult<AdminProductResponse, Guid>(
            products.Items.Select(p => p.ToAdminProductResponse()).ToList(),
            products.NextCursor);

        return Result<CursorPagedResult<AdminProductResponse, Guid>>.Success(pagedResponse);
    }
}