using E_Commerce.Application.Features.ProductImages.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.ProductImages.Queries.Vendor_Get_Images;

internal sealed class VendorGetImageQueryHandler : IRequestHandler<VendorGetImageQuery, Result<VendorImageDetailsResponse>>
{
    private readonly IProductImageRepository _productImageRepository;
    private readonly IProductRepository _productRepository;
    private readonly ILogger<VendorGetImageQueryHandler> _logger;

    public VendorGetImageQueryHandler(IProductImageRepository productImageRepository, IProductRepository productRepository, 
        ILogger<VendorGetImageQueryHandler> logger)
    {
        _productImageRepository = productImageRepository;
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<Result<VendorImageDetailsResponse>> Handle(VendorGetImageQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if (product?.VendorId != request.VendorId)
        {
            _logger.LogWarning("SECURITY ALERT: IDOR attempt! VendorID: {VendorId} tried to modify images for ProductID: {ProductId} which belongs to another vendor.",
                request.VendorId, request.ProductId);

            return Result<VendorImageDetailsResponse>.Failure(ProductErrors.AccessDenied);
        }

        if (product is null)
            return Result<VendorImageDetailsResponse>.Failure(ProductErrors.ProductNotFound);

        var image = await _productImageRepository.GetAsync(request.ImageId, cancellationToken);

        if (image?.ProductId != request.ProductId)
        {
            _logger.LogWarning("SECURITY ALERT: IDOR attempt! VendorID: {VendorId} tried to access image {ImageId} for ProductID: {ProductId} which belongs to another vendor.",
                request.VendorId, request.ImageId, request.ProductId);

            return Result<VendorImageDetailsResponse>.Failure(ProductImageErrors.AccessDenied);
        }

        if (image is null)
            return Result<VendorImageDetailsResponse>.Failure(ProductImageErrors.NotFound);

        return Result<VendorImageDetailsResponse>.Success(image.ToGetVendorImageDetails());
    }
}