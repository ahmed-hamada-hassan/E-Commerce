using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.ProductImages.Commands.ReplaceProductImage;

internal sealed class ReplaceProductImageCommandHandler : IRequestHandler<ReplaceProductImageCommand, Result<bool>>
{
    private readonly IProductRepository _productRepository;
    private readonly IFileService _fileService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductImageRepository _productImageRepository;
    private readonly ILogger<ReplaceProductImageCommandHandler> _logger;

    public ReplaceProductImageCommandHandler(IProductRepository productRepository, IFileService fileService, 
        IUnitOfWork unitOfWork, IProductImageRepository productImageRepository, ILogger<ReplaceProductImageCommandHandler> logger)
    {
        _productRepository = productRepository;
        _fileService = fileService;
        _unitOfWork = unitOfWork;
        _productImageRepository = productImageRepository;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(ReplaceProductImageCommand request, CancellationToken cancellationToken)
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

        var existingImage = await _productImageRepository.GetAsync(request.ImageId, cancellationToken);
        if(existingImage is null || existingImage.ProductId != request.ProductId) 
            return Result<bool>.Failure(ProductImageErrors.NotFound);

        var newImageUrl = await _fileService.UploadImageAsync(request.NewImage);
        if (string.IsNullOrWhiteSpace(newImageUrl))
        {
            _logger.LogError("INFRASTRUCTURE ERROR: Image upload failed during replacing image {ImageId} for Product: {ProductId}",
                request.ImageId, request.ProductId);

            return Result<bool>.Failure(ProductImageErrors.UploadFaild);
        }
        else
        {
            var deleted = await _fileService.DeleteImageAsync(existingImage.ImageUrl);
            if (!deleted)
            {
                _logger.LogWarning("Failed to delete old image from cloud during replacement. ImageUrl: {OldImageUrl}", existingImage.ImageUrl);
            }
        }

        existingImage.UpdateUrl(newImageUrl);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}