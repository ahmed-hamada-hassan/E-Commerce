using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.ProductImages.Commands.RemoveImage;

internal sealed class AdminRemoveImageCommandHandler : IRequestHandler<AdminRemoveImageCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductImageRepository _productImageRepository;
    private readonly IFileService _fileService;
    private readonly ILogger<AdminRemoveImageCommandHandler> _logger;

    public AdminRemoveImageCommandHandler(IUnitOfWork unitOfWork, IProductImageRepository productImageRepository, 
        IFileService fileService, ILogger<AdminRemoveImageCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _productImageRepository = productImageRepository;
        _fileService = fileService;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(AdminRemoveImageCommand request, CancellationToken cancellationToken)
    {
        var image = await _productImageRepository.GetAsync(request.ImgaeId, cancellationToken);
        if (image == null) return Result<bool>.Failure(ProductImageErrors.NotFound);

        var imageCount = await _productImageRepository.GetCountByProductIdAsync(request.ProductId, cancellationToken);
        if(imageCount == 1)
            return Result<bool>.Failure(ProductImageErrors.CannotDeleteLastImage);

        var deletedFromCloud = await _fileService.DeleteImageAsync(image.ImageUrl);
        if(!deletedFromCloud)
        {
            _logger.LogError("Failed to delete image from cloud for product with id {ProductId}", request.ProductId);
            return Result<bool>.Failure(ProductImageErrors.DeleteFaild);
        }

        var isDeleted = await _productImageRepository.RemoveAsync(image, cancellationToken);
        if (!isDeleted)
        {
            _logger.LogError("Failed to delete product images for product with id {ProductId}", request.ProductId);
            return Result<bool>.Failure(ProductImageErrors.DeleteFaild);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true);
    }
}
