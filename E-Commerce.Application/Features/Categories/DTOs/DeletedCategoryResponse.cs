namespace E_Commerce.Application.Features.Categories.DTOs;

public record DeletedCategoryResponse(Guid Id, string Name, string? Description, Guid? ParentId, string? ImageUrl, bool IsDeleted, DateTimeOffset? DeletedOn);
