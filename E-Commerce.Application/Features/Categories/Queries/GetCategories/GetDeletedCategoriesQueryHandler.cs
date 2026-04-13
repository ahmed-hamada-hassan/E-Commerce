using E_Commerce.Application.Features.Categories.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Categories.Queries.GetCategories;

internal sealed class GetDeletedCategoriesQueryHandler : 
    IRequestHandler<GetDeletedCategoriesQuery, Result<CursorPagedResult<DeletedCategoryResponse, Guid>>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetDeletedCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<CursorPagedResult<DeletedCategoryResponse, Guid>>> Handle(GetDeletedCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.DeletedCategoriesAsync(request.Cursor,
            request.Size, cancellationToken);

        var response = categories.Items.Select(c => c.ToDeletedCategoryResponse()).ToList();

        var pagedResult = new CursorPagedResult<DeletedCategoryResponse, Guid>(response, categories.NextCursor);

        return Result<CursorPagedResult<DeletedCategoryResponse, Guid>>.Success(pagedResult);
    }
}
