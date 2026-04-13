using E_Commerce.Application.Features.ProductImages.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.ProductImages.Queries.Vendor_Get_Images;

internal sealed class VendorGetImagesQueryHandler : IRequestHandler<VendorGetImagesQuery, Result<IReadOnlyCollection<VendorImageDetailsResponse>>>
{
    private readonly IProductImageRepository _productImageRepository;
    private readonly IProductRepository _productRepository;
    private readonly ILogger<VendorGetImagesQueryHandler> _logger;

    public VendorGetImagesQueryHandler(IProductImageRepository productImageRepository, IProductRepository productRepository, 
        ILogger<VendorGetImagesQueryHandler> logger)
    {
        _productImageRepository = productImageRepository;
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyCollection<VendorImageDetailsResponse>>> Handle(VendorGetImagesQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if (product?.VendorId != request.VendorId)
        {
            _logger.LogWarning("SECURITY ALERT: IDOR attempt! VendorID: {VendorId} tried to access images for ProductID: {ProductId} which belongs to another vendor.",
                request.VendorId, request.ProductId);

            return Result<IReadOnlyCollection<VendorImageDetailsResponse>>.Failure(ProductErrors.AccessDenied);
        }

        if (product is null)
            return Result<IReadOnlyCollection<VendorImageDetailsResponse>>.Failure(ProductErrors.ProductNotFound);

        var images = await _productImageRepository.GetAllByProductIdAsync(request.ProductId, cancellationToken);

        var response = images.Select(i => i.ToGetVendorImageDetails()).ToList();

        return Result<IReadOnlyCollection<VendorImageDetailsResponse>>.Success(response);
    }
}