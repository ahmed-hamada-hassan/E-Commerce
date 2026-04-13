using E_Commerce.Application.Features.Categories.DTOs;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Categories.Queries.Public_Get_Categories;

internal sealed class PublicGetCategoriesQueryHandler : IRequestHandler<PublicGetCategoriesQuery, Result<CursorPagedResult<PublicCategoryResponse, Guid>>>
{
    private readonly ICategoryRepository _categoryRepository;

    public PublicGetCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<CursorPagedResult<PublicCategoryResponse, Guid>>> Handle(PublicGetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var Categories =
            await _categoryRepository.PublicCategoriesAsync(request.Cursor, request.Size, cancellationToken);

        var pagedCategories = new CursorPagedResult<PublicCategoryResponse, Guid>(
            Categories.Items.Select(c => c.ToPublicCategoryResponse()).ToList(), Categories.NextCursor);

        return Result<CursorPagedResult<PublicCategoryResponse, Guid>>.Success(pagedCategories);
    }
}