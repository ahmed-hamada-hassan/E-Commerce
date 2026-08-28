using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Products.Queries.GetProduct;

internal sealed class GetProductQueryHandler : IRequestHandler<GetProductQuery, Result<CustomerProductDetailsResponse>>
{
    private readonly IProductRepository _productRepository;
    private readonly IFeedbackRepository _feedbackRepository;

    public GetProductQueryHandler(IProductRepository productRepository, IFeedbackRepository feedbackRepository)
    {
        _productRepository = productRepository;
        _feedbackRepository = feedbackRepository;
    }

    public async Task<Result<CustomerProductDetailsResponse>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product is null) return Result<CustomerProductDetailsResponse>.Failure(ProductErrors.ProductNotFound);

        var (averageRating, totalReviews) = await _feedbackRepository.GetProductRatingAsync(request.Id, cancellationToken);

        var response = product.ToCustomerProductDetailsResponse(averageRating, totalReviews);
        return Result<CustomerProductDetailsResponse>.Success(response);
    }
}