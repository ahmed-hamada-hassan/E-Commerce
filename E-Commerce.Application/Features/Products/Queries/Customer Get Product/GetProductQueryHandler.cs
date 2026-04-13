using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Products.Queries.GetProduct;

internal sealed class GetProductQueryHandler : IRequestHandler<GetProductQuery, Result<CustomerProductResponse>>
{
    private readonly IProductRepository _productRepository;

    public GetProductQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<CustomerProductResponse>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product is null) return Result<CustomerProductResponse>.Failure(ProductErrors.ProductNotFound);

        var response = product.ToCustomerProductResponse();
        return Result<CustomerProductResponse>.Success(response);
    }
}