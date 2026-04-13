namespace E_Commerce.Application.Features.Categories.DTOs;

public record CreateCategoryRequest (string Name, string? Description, Guid? ParentId, string? ImageUrl);
