using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Products.Command.RestoreProduct;

internal sealed class RestoreProductCommandHandler : IRequestHandler<RestoreProductCommand, Result<bool>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RestoreProductCommandHandler> _logger;

    public RestoreProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, ILogger<RestoreProductCommandHandler> logger)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(RestoreProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetVendorArchivedProductAsync(request.ProductId, cancellationToken);

        if(product?.VendorId != request.VendorId)
        {
            _logger.LogWarning("Vendor {VendorId} attempted to restore product {ProductId} which they do not own.", request.VendorId, request.ProductId);
            return Result<bool>.Failure(ProductErrors.AccessDenied);
        }

        if (product is null) return Result<bool>.Failure(ProductErrors.ProductNotFound);

        if(product.DeletedByAdmin)
        {
            _logger.LogWarning("Vendor {VendorId} attempted to restore product {ProductId} which is deleted by admin.", 
                request.VendorId, request.ProductId);
            return Result<bool>.Failure(ProductErrors.AccessDenied);
        }

        product.RestoreByVendor();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true);
    }
}