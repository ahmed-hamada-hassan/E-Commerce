using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Products.Command.DeleteProduct;

internal sealed class ArchiveProductCommandHandler : IRequestHandler<ArchiveProductCommand, Result<bool>>
{
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ArchiveProductCommandHandler> _logger;

    public ArchiveProductCommandHandler(IProductRepository productRepository, IOrderRepository orderRepository, 
        IUnitOfWork unitOfWork, ILogger<ArchiveProductCommandHandler> logger)
    {
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(ArchiveProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if(product?.VendorId != request.VendorId)
        {
            _logger.LogWarning("Vendor {VendorId} attempted to archive product {ProductId} which they do not own.", request.VendorId, request.ProductId);
            return Result<bool>.Failure(ProductErrors.AccessDenied);
        }

        if (product == null) return Result<bool>.Failure(ProductErrors.ProductNotFound);

        var hasActiveOrders = await _orderRepository.HasActiveOrdersForProductAsync(request.ProductId, cancellationToken);
        if (hasActiveOrders)
        {
            _logger.LogError("Attempt to archive product with active orders. ProductId: {ProductId}", request.ProductId);
            return Result<bool>.Failure(ProductErrors.HasActiveOrder);
        }

        product.ArchiveByVendor();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true);
    }
}