using E_Commerce.Application.Features.Categories.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Categories.Queries.GetCategories;

internal sealed class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, Result<CursorPagedResult<CategoryResponse, Guid>>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<CursorPagedResult<CategoryResponse, Guid>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.CategoriesAsync(request.Cursor, request.Size, cancellationToken);

        var response = categories.Items.Select(c => c.ToCategoryResponse()).ToList();

        var pagedResult = new CursorPagedResult<CategoryResponse, Guid>(response, categories.NextCursor);

        return Result<CursorPagedResult<CategoryResponse, Guid>>.Success(pagedResult);
    }
}
