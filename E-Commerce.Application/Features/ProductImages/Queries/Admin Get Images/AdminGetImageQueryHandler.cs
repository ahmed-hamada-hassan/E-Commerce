using E_Commerce.Application.Features.ProductImages.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.ProductImages.Queries.Admin_Get_Images;

internal sealed class AdminGetImageQueryHandler : IRequestHandler<AdminGetImageQuery, Result<AdminImageDetailsResponse>>
{
    private readonly IProductImageRepository _productImageRepository;
    private readonly IProductRepository _productRepository;
    public AdminGetImageQueryHandler(IProductImageRepository productImageRepository, IProductRepository productRepository)
    {
        _productImageRepository = productImageRepository;
        _productRepository = productRepository;
    }

    public async Task<Result<AdminImageDetailsResponse>> Handle(AdminGetImageQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if (product is null)
            return Result<AdminImageDetailsResponse>.Failure(ProductErrors.ProductNotFound);

        var image = await _productImageRepository.GetAsync(request.ImageId, cancellationToken);

        if (image?.ProductId != request.ProductId)
            return Result<AdminImageDetailsResponse>.Failure(ProductImageErrors.NotFound);

        if (image is null)
            return Result<AdminImageDetailsResponse>.Failure(ProductImageErrors.NotFound);

        return Result<AdminImageDetailsResponse>.Success(new AdminImageDetailsResponse(ImageUrl: image.ImageUrl));
    }
}