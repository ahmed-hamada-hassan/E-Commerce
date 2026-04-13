namespace E_Commerce.Application.Features.Categories.DTOs;

public record CategoryResponse(Guid Id, string Name, string? Description, Guid? ParentId, string? ImageUrl);
