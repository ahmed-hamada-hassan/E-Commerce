using E_Commerce.Application.Features.ProductImages.DTOs;
using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.ProductImages.Commands.AddImage;

internal sealed class AddImageCommandHandler : IRequestHandler<AddImageCommand, Result<List<AddImageResponse>>>
{
    private readonly IProductImageRepository _productImageRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;
    private readonly ILogger<AddImageCommandHandler> _logger;

    public AddImageCommandHandler(IProductImageRepository productImageRepository, IProductRepository productRepository, 
        IUnitOfWork unitOfWork, IFileService fileService, ILogger<AddImageCommandHandler> logger)
    {
        _productImageRepository = productImageRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
        _logger = logger;
    }

    public async Task<Result<List<AddImageResponse>>> Handle(AddImageCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if(product?.VendorId != request.VendorId)
            return Result<List<AddImageResponse>>.Failure(ProductErrors.AccessDenied);

        if (product is null)
            return Result<List<AddImageResponse>>.Failure(ProductErrors.ProductNotFound);

        byte currentCount = await _productImageRepository.GetCountByProductIdAsync(request.ProductId, cancellationToken);

        byte newImagesCount = (byte)request.Images.Count();
        byte totalCount = (byte)(currentCount + newImagesCount);

        if(totalCount > 7)
            return Result<List<AddImageResponse>>.Failure(new Error("ProductImage.LimitExceeded",
            $"Product already has {currentCount} images. You can only add {7 - currentCount} more."));

        var imageResponses = new List<AddImageResponse>();
        var uploadedImageUrls = new List<string>();

        try
        {
            foreach (var imageDto in request.Images)
            {
                var newImageUrl = await _fileService.UploadImageAsync(imageDto.Image);

                if (string.IsNullOrEmpty(newImageUrl))
                {
                    _logger.LogError("Failed to upload image for product {ProductId}", request.ProductId);
                    return Result<List<AddImageResponse>>.Failure(ProductImageErrors.UploadFaild);
                }

                uploadedImageUrls.Add(newImageUrl);

                var productImage = ProductImage.Create(request.ProductId, newImageUrl, imageDto.IsPrimary, imageDto.DisplayOrder);

                if (productImage.IsFailure)
                {
                    await _fileService.DeleteImageAsync(newImageUrl);
                    _logger.LogError("Failed to create product image for product {ProductId}", request.ProductId);
                    return Result<List<AddImageResponse>>.Failure(productImage.Error);
                }

                var addResult = await _productImageRepository.AddAsync(productImage.Value!, cancellationToken);
                imageResponses.Add(new AddImageResponse(addResult, newImageUrl, imageDto.DisplayOrder, imageDto.IsPrimary));
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<List<AddImageResponse>>.Success(imageResponses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Critical: Database save failed for product {ProductId}. Starting rollback...", request.ProductId);

            foreach (var url in uploadedImageUrls)
            {
                try
                {
                    await _fileService.DeleteImageAsync(url);
                }
                catch (Exception deleteEx)
                {
                    _logger.LogWarning(deleteEx, "Rollback: Could not delete file at {Url}", url);
                }
            }
            return Result<List<AddImageResponse>>.Failure(ProductImageErrors.AddFaild);
        }
    }
}