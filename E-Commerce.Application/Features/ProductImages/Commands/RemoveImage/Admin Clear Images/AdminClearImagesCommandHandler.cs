using E_Commerce.Application.Interfaces.Data;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.ProductImages.Commands.RemoveImage;

internal class AdminClearImagesCommandHandler : IRequestHandler<AdminClearImagesCommand, Result<bool>>
{
    private readonly IProductImageRepository _productImageRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;
    private readonly ILogger<AdminClearImagesCommandHandler> _logger;

    public AdminClearImagesCommandHandler(IProductImageRepository productImageRepository, 
        IUnitOfWork unitOfWork, IFileService fileService, ILogger<AdminClearImagesCommandHandler> logger)
    {
        _productImageRepository = productImageRepository;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(AdminClearImagesCommand request, CancellationToken cancellationToken)
    {
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
