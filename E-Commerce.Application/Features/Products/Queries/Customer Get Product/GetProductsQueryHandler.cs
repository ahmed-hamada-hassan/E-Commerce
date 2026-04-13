using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Products.Queries.GetProducts;

internal sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<OffsetPagedResult<CustomerProductResponse>>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<OffsetPagedResult<CustomerProductResponse>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.FilteredAvailableProductsAsync(
            request.SearchTerm, request.MinPrice, request.MaxPrice, request.SortBy, request.Page, request.Size, cancellationToken);
        var response = products.Items.Select(p => p.ToCustomerProductResponse()).ToList();
        var pagedResult = new OffsetPagedResult<CustomerProductResponse>(response, request.Page, 
            request.Size, products.TotalCount, products.TotalPages);
        return Result<OffsetPagedResult<CustomerProductResponse>>.Success(pagedResult);
    }
}