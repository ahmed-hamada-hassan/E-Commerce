using E_Commerce.Application.Features.Categories.Commands.CreateCategory;
using E_Commerce.Application.Features.Categories.Commands.UpdateCategory;
using E_Commerce.Application.Features.Categories.DTOs;

namespace E_Commerce.API.Contracts;

public static class CategoryMappingExtensions
{
    public static CreateCategoryCommand ToCategoryCommand(this CreateCategoryRequest request)
    {
        return new CreateCategoryCommand(
            request.Name,
            string.IsNullOrWhiteSpace(request.Description) ? null : request.Description, request.ParentId, request.Image);
    }

    public static UpdateCategoryCommand ToUpdateCategoryCommand(this UpdateCategoryRequest request, Guid Id)
    {
        return new UpdateCategoryCommand(Id, string.IsNullOrWhiteSpace(request.Name) ? null : request.Name,
            string.IsNullOrWhiteSpace(request.Description) ? null : request.Description, request.ParentId, request.Image);
    }
}
