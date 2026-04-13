using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Products.Queries.Admin_Get_Product;

internal class AdminGetProductQueryHandler : IRequestHandler<AdminGetProductQuery, Result<AdminProductResponse>>
{
    private readonly IProductRepository _productRepository;

    public AdminGetProductQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<AdminProductResponse>> Handle(AdminGetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetAdminProductByIdAsync(request.ProductId, cancellationToken);
        if (product == null) 
            return Result<AdminProductResponse>.Failure(ProductErrors.ProductNotFound);

        return Result<AdminProductResponse>.Success(product.ToAdminProductResponse());
    }
}