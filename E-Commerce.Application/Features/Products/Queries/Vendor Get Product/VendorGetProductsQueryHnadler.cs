using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Products.Queries.Vendor_Get_Product;

internal sealed class VendorGetProductsQueryHnadler : IRequestHandler<VendorGetProductsQuery, Result<CursorPagedResult<VendorProductResponse, Guid>>>
{
    private readonly IProductRepository _productRepository;

    public VendorGetProductsQueryHnadler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<CursorPagedResult<VendorProductResponse, Guid>>> Handle(VendorGetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetVendorAvailableProductsAsync(request.VendorId, request.Cursor, 
            request.Size, cancellationToken);

        var pagedResult = new CursorPagedResult<VendorProductResponse, Guid>(
            products.Items.Select(p => p.ToVendorProductResponse()).ToList(),
            products.NextCursor);

        return Result<CursorPagedResult<VendorProductResponse, Guid>>.Success(pagedResult);
    }
}