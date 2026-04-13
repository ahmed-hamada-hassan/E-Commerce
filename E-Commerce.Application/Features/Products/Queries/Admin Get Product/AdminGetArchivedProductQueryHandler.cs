using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Products.Queries.Admin_Get_Product;

internal class AdminGetArchivedProductQueryHandler : IRequestHandler<AdminGetArchivedProductQuery, Result<AdminArchivedProductResponse>>
{
    private readonly IProductRepository _productRepository;

    public AdminGetArchivedProductQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<AdminArchivedProductResponse>> Handle(AdminGetArchivedProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetAdminArchivedProductByIdAsync(request.ProductId, cancellationToken);
        if (product == null) 
            return Result<AdminArchivedProductResponse>.Failure(ProductErrors.ProductNotFound);

        return Result<AdminArchivedProductResponse>.Success(product.ToAdminArchivedProductResponse());
    }
}