using E_Commerce.Application.Features.Categories.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Categories.Queries.GetCategory;

public record GetCategoryQuery(Guid Id) : IRequest<Result<CategoryResponse>>;
