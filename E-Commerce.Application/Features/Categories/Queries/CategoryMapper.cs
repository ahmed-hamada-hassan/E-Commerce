using E_Commerce.Application.Features.Categories.DTOs;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Features.Categories.Queries;

public static class CategoryMapper
{
    public static CategoryResponse ToCategoryResponse(this Category request)
    {
        return new CategoryResponse
        (
            Id: request.Id,
            Name: request.Name,
            Description: request.Description,
            ParentId: request.ParentCategoryId,
            ImageUrl: request.ImageUrl
        );
    }
    public static DeletedCategoryResponse ToDeletedCategoryResponse(this Category request)
    {
        return new DeletedCategoryResponse
        (
            Id: request.Id,
            Name: request.Name,
            Description: request.Description,
            ParentId: request.ParentCategoryId,
            ImageUrl: request.ImageUrl,
            IsDeleted: request.IsDeleted,
            DeletedOn: request.DeleteOn
        );
    }

    public static PublicCategoryResponse ToPublicCategoryResponse(this Category request)
    {
        return new PublicCategoryResponse(
            Name: request.Name,
            Description: request.Description,
            ParentCategoryName: request.ParentCategory?.Name,
            ImageUrl: request.ImageUrl
            );
    }
}
