using E_Commerce.Application.Features.Categories.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Errors;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Categories.Queries.GetCategory;

internal sealed class GetDeletedCategoryQueryHandler : IRequestHandler<GetDeletedCategoryQuery, Result<DeletedCategoryResponse>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetDeletedCategoryQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<DeletedCategoryResponse>> Handle(GetDeletedCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetDeletedByIdAsync(request.Id, cancellationToken);
        if (category == null) return Result<DeletedCategoryResponse>.Failure(CategoryErrors.NotFound);

        var response = category.ToDeletedCategoryResponse();
        return Result<DeletedCategoryResponse>.Success(response);
    }
}
