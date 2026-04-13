using E_Commerce.Application.Features.ProductImages.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.ProductImages.Queries.Admin_Get_Images;

internal sealed class AdminGetImagesQueryHandler : IRequestHandler<AdminGetImagesQuery, Result<IReadOnlyCollection<AdminImageDetailsResponse>>>
{
    private readonly IProductImageRepository _productImageRepository;
    private readonly IProductRepository _productRepository;

    public AdminGetImagesQueryHandler(IProductImageRepository productImageRepository, IProductRepository productRepository)
    {
        _productImageRepository = productImageRepository;
        _productRepository = productRepository;
    }

    public async Task<Result<IReadOnlyCollection<AdminImageDetailsResponse>>> Handle(AdminGetImagesQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if (product is null)
            return Result<IReadOnlyCollection<AdminImageDetailsResponse>>.Failure(ProductErrors.ProductNotFound);

        var images = await _productImageRepository.GetAllByProductIdAsync(request.ProductId, cancellationToken);

        var response = images.Select(i => new AdminImageDetailsResponse(ImageUrl : i.ImageUrl)).ToList();

        return Result<IReadOnlyCollection<AdminImageDetailsResponse>>.Success(response);
    }
}
