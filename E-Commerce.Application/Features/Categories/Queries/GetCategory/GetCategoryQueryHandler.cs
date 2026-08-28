using E_Commerce.Application.Features.Categories.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Categories.Queries.GetCategory;

internal sealed class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, Result<PublicCategoryResponse>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<PublicCategoryResponse>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null) return Result<PublicCategoryResponse>.Failure(CategoryErrors.NotFound);

        var response = category.ToPublicCategoryResponse();
        return Result<PublicCategoryResponse>.Success(response);
    }
}