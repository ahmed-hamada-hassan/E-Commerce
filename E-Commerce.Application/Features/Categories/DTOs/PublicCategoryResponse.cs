namespace E_Commerce.Application.Features.Categories.DTOs;

public record PublicCategoryResponse(string Name, string? Description, string? ParentCategoryName, string? ImageUrl);