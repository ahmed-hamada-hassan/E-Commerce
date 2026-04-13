using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.ProductImages.Commands.SetPrimary;

internal sealed class SetPrimaryCommandHandler : IRequestHandler<SetPrimaryCommand, Result<bool>>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductImageRepository _productImageRepository;
    private readonly ILogger<SetPrimaryCommandHandler> _logger;

    public SetPrimaryCommandHandler(IProductRepository productRepository, 
        IProductImageRepository productImageRepository, ILogger<SetPrimaryCommandHandler> logger)
    {
        _productRepository = productRepository;
        _productImageRepository = productImageRepository;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(SetPrimaryCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if (product?.VendorId != request.VendorId)
        {
            _logger.LogWarning("SECURITY ALERT: IDOR attempt! VendorID: {VendorId} tried to modify images for ProductID: {ProductId} which belongs to another vendor.",
                request.VendorId, request.ProductId);

            return Result<bool>.Failure(ProductErrors.AccessDenied);
        }

        if (product is null)
            return Result<bool>.Failure(ProductErrors.ProductNotFound);

        var isSuccess = await _productImageRepository.SetPrimaryAsync(product.Id, request.ImageId, cancellationToken);
        if (!isSuccess)
        {
            _logger.LogWarning("Failed to set primary image. ProductID: {ProductId}, ImageID: {ImageId}", request.ProductId, request.ImageId);
            return Result<bool>.Failure(ProductImageErrors.SetPrimaryFailed);
        }

        return Result<bool>.Success(true);
    }
}