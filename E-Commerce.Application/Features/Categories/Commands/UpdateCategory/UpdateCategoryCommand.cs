using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace E_Commerce.Application.Features.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand(Guid Id, string? Name, string? Description, Guid? ParentId, IFormFile? Image) : IRequest<Result<bool>>;
