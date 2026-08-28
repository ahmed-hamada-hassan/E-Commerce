using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Products.Queries.GetProducts;

internal sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<OffsetPagedResult<CustomerProductDetailsResponse>>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<OffsetPagedResult<CustomerProductDetailsResponse>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.FilteredAvailableProductsAsync(
            request.CategoryId, request.SearchTerm, request.MinPrice, request.MaxPrice, request.SortBy, request.Page, request.Size, cancellationToken);
        var response = products.Items
            .Select(p => p.Product.ToCustomerProductDetailsResponse(p.Rating, p.TotalReviews)).ToList();
        var pagedResult = new OffsetPagedResult<CustomerProductDetailsResponse>(response, request.Page,
            request.Size, products.TotalCount, products.TotalPages);
        return Result<OffsetPagedResult<CustomerProductDetailsResponse>>.Success(pagedResult);
    }
}