using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Products.Queries.Admin_Get_Product;

internal class AdminGetSuspendProductQueryHandler : IRequestHandler<AdminGetSuspendProductQuery, Result<AdminSuspendProductResponse>>
{
    private readonly IProductRepository _productRepository;

    public AdminGetSuspendProductQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<AdminSuspendProductResponse>> Handle(AdminGetSuspendProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetAdminSuspendProductByIdAsync(request.ProductId, cancellationToken);
        if (product == null) 
            return Result<AdminSuspendProductResponse>.Failure(ProductErrors.ProductNotFound);

        return Result<AdminSuspendProductResponse>.Success(product.ToAdminSuspendProductResponse());
    }
}