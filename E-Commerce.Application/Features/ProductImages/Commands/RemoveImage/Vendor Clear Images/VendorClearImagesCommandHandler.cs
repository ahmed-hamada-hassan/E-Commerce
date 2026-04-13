using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.ProductImages.Commands.RemoveImage;

internal class VendorClearImagesCommandHandler : IRequestHandler<VendorClearImagesCommand, Result<bool>>
{
    private readonly IProductImageRepository _productImageRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;
    private readonly ILogger<VendorClearImagesCommandHandler> _logger;

    public VendorClearImagesCommandHandler(IProductImageRepository productImageRepository, IProductRepository productRepository,
        IUnitOfWork unitOfWork, IFileService fileService, ILogger<VendorClearImagesCommandHandler> logger)
    {
        _productImageRepository = productImageRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(VendorClearImagesCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if (product?.VendorId != request.VendorId)
        {
            _logger.LogWarning("SECURITY ALERT: IDOR attempt! VendorID: {VendorId} tried to modify images for ProductID: {ProductId} which belongs to another vendor.",
                request.VendorId, request.ProductId);

            return Result<bool>.Failure(ProductErrors.AccessDenied);
        }

        if (product is null) return Result<bool>.Failure(ProductErrors.ProductNotFound);

        var img = await _productImageRepository.GetAllByProductIdAsync(request.ProductId, cancellationToken);
        if (img is null || img.Count == 0) return Result<bool>.Failure(ProductImageErrors.NotFound);

        foreach (var image in img)
        {
            if (!string.IsNullOrWhiteSpace(image.ImageUrl))
                await _fileService.DeleteImageAsync(image.ImageUrl);
        }

        var isDeleted = await _productImageRepository.RemoveByProductIdAsync(request.ProductId, cancellationToken);
        if (!isDeleted)
        {
            _logger.LogError("Failed to delete product images for product with id {ProductId}", request.ProductId);
            return Result<bool>.Failure(ProductImageErrors.DeleteFaild);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true);
    }
}