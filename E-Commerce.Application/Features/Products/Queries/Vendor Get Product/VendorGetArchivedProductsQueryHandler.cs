using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Products.Queries.Vendor_Get_Product;

internal sealed class VendorGetArchivedProductsQueryHandler :
    IRequestHandler<VendorGetArchivedProductsQuery, Result<CursorPagedResult<VendorArchivedProductResponse, Guid>>>
{
    private readonly IProductRepository _productRepository;

    public VendorGetArchivedProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<CursorPagedResult<VendorArchivedProductResponse, Guid>>> Handle(VendorGetArchivedProductsQuery request, CancellationToken cancellationToken)
    {
        var archivedProducts = 
            await _productRepository.GetVendorArchivedProductsAsync(request.VendorId, request.Cursor, request.Size, cancellationToken);

        var pagedResult = new CursorPagedResult<VendorArchivedProductResponse, Guid>(
            archivedProducts.Items.Select(p => p.ToVendorArchivedProductResponse()).ToList(), archivedProducts.NextCursor);

        return Result<CursorPagedResult<VendorArchivedProductResponse, Guid>>.Success(pagedResult);
    }
}