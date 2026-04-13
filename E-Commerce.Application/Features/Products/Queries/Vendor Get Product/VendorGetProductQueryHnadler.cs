using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Products.Queries.Vendor_Get_Product;

internal sealed class VendorGetProductQueryHnadler : IRequestHandler<VendorGetProductQuery, Result<VendorProductResponse>>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<VendorGetProductQueryHnadler> _logger;

    public VendorGetProductQueryHnadler(IProductRepository productRepository, ILogger<VendorGetProductQueryHnadler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<Result<VendorProductResponse>> Handle(VendorGetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId,cancellationToken);
        if(product?.VendorId != request.VendorId)
        {
            _logger.LogWarning("Vendor {VendorId} attempted to access product {ProductId} which does not belong to them.", request.VendorId, request.ProductId);
            return Result<VendorProductResponse>.Failure(ProductErrors.AccessDenied);
        }
        if(product is null)
            return Result<VendorProductResponse>.Failure(ProductErrors.ProductNotFound);

        return Result<VendorProductResponse>.Success(product.ToVendorProductResponse());
    }
}