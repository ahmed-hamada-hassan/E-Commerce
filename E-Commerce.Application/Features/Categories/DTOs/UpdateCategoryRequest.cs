using Microsoft.AspNetCore.Http;

namespace E_Commerce.Application.Features.Categories.DTOs;

public record UpdateCategoryRequest(string? Name, string? Description, Guid? ParentId, IFormFile? Image);
