using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Products.Command.UpdateProduct;

internal sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<bool>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateProductCommandHandler> _logger;

    public UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, ILogger<UpdateProductCommandHandler> logger)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if (product is null)
            return Result<bool>.Failure(ProductErrors.ProductNotFound);

        if (product?.VendorId != request.VendorId)
        {
            _logger.LogWarning("Vendor {VendorId} attempted to update product {ProductId} which they do not own.", request.VendorId, request.ProductId);
            return Result<bool>.Failure(ProductErrors.AccessDenied);
        }

        if (!string.IsNullOrWhiteSpace(request.SKU) && product.SKU != request.SKU)
        {
            var isSKUExists = await _productRepository.IsSKUExistsAsync(request.SKU, request.ProductId, cancellationToken);
            if (isSKUExists)
            {
                _logger.LogWarning("Attempted to update product with a duplicate SKU. ProductId: {ProductId}, SKU: {SKU}",
                    request.ProductId, request.SKU);
                return Result<bool>.Failure(ProductErrors.DuplicateSKU);
            }
        }

        var updateResult = product.Update(request.CategoryId, request.Name,
            request.Description, request.Price, request.SKU, request.Barcode, request.Quantity);

        if (updateResult.IsFailure) return Result<bool>.Failure(updateResult.Error);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}