using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.ProductImages.Commands.ReorderProductImage;

internal sealed class ReorderProductImageCommandHandler : IRequestHandler<ReorderProductImageCommand, Result<bool>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductImageRepository _productImageRepository;
    private readonly ILogger<ReorderProductImageCommandHandler> _logger;

    public ReorderProductImageCommandHandler(IProductRepository productRepository,
        IUnitOfWork unitOfWork, IProductImageRepository productImageRepository, ILogger<ReorderProductImageCommandHandler> logger)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _productImageRepository = productImageRepository;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(ReorderProductImageCommand request, CancellationToken cancellationToken)
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

        var existingImages = await _productImageRepository.GetAllByProductIdAsync(request.ProductId, cancellationToken);

        foreach (var newOrder in request.Images)
        {
            var imgToUpdate = existingImages.FirstOrDefault(img => img.Id == newOrder.imageId);
            if (imgToUpdate is null)
                return Result<bool>.Failure(ProductImageErrors.NotFound);
            else
                imgToUpdate.SetDisplayOrder(newOrder.displayOrder);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true);
    }
}