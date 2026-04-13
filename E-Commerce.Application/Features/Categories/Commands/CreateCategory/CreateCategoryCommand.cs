using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace E_Commerce.Application.Features.Categories.Commands.CreateCategory;

public record CreateCategoryCommand (string Name, string? Description, Guid? ParentCategoryId, IFormFile? Image) : IRequest<Result<Guid>>;
