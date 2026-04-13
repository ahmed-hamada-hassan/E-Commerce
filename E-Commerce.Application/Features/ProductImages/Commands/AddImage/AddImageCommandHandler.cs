using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.ProductImages.Commands.AddImage;

internal sealed class AddImageCommandHandler : IRequestHandler<AddImageCommand, Result<List<Guid>>>
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

    public async Task<Result<List<Guid>>> Handle(AddImageCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if(product?.VendorId != request.VendorId)
            return Result<List<Guid>>.Failure(ProductErrors.AccessDenied);

        if (product is null)
            return Result<List<Guid>>.Failure(ProductErrors.ProductNotFound);

        byte currentCount = await _productImageRepository.GetCountByProductIdAsync(request.ProductId, cancellationToken);

        byte newImagesCount = (byte)request.Images.Count();
        byte totalCount = (byte)(currentCount + newImagesCount);

        if(totalCount > 7)
            return Result<List<Guid>>.Failure(new Error("ProductImage.LimitExceeded",
            $"Product already has {currentCount} images. You can only add {7 - currentCount} more."));

        var addedImagesId = new List<Guid>();

        foreach (var imageDto in request.Images)
        {
            var newImageUrl = await _fileService.UploadImageAsync(imageDto.Image);

            if (string.IsNullOrEmpty(newImageUrl))
            {
                _logger.LogError("Failed to upload image for product {ProductId}", request.ProductId);
                return Result<List<Guid>>.Failure(ProductImageErrors.UploadFaild);
            }

            var productImage = ProductImage.Create(request.ProductId, newImageUrl, imageDto.IsPrimary, imageDto.DisplayOrder);

            if (productImage.IsFailure)
            {
                _logger.LogError("Failed to create product image for product {ProductId}", request.ProductId);
                return Result<List<Guid>>.Failure(productImage.Error);
            }

            var addResult = await _productImageRepository.AddAsync(productImage.Value!, cancellationToken);
            addedImagesId.Add(addResult);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<List<Guid>>.Success(addedImagesId);
    }
}