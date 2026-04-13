using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Products.Queries.Vendor_Get_Product;

internal sealed class VendorGetArchivedProductQueryHandler :
    IRequestHandler<VendorGetArchivedProductQuery, Result<VendorArchivedProductResponse>>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<VendorGetArchivedProductQueryHandler> _logger;

    public VendorGetArchivedProductQueryHandler(IProductRepository productRepository, ILogger<VendorGetArchivedProductQueryHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<Result<VendorArchivedProductResponse>> Handle(VendorGetArchivedProductQuery request, CancellationToken cancellationToken)
    {
        var archivedProduct = await _productRepository.GetVendorArchivedProductAsync(request.ProductId, cancellationToken);
        if(archivedProduct?.VendorId != request.VendorId)
        {
            _logger.LogWarning("Vendor with ID {VendorId} attempted to access archived product with ID {ProductId} that does not belong to them.", request.VendorId, request.ProductId);
            return Result<VendorArchivedProductResponse>.Failure(ProductErrors.AccessDenied);
        }
        if(archivedProduct is null)
            return Result<VendorArchivedProductResponse>.Failure(ProductErrors.ProductNotFound);

        return Result<VendorArchivedProductResponse>.Success(archivedProduct.ToVendorArchivedProductResponse());
    }
}
