using E_Commerce.Application.Features.Categories.DTOs;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Categories.Queries.GetCategories;

public record GetDeletedCategoriesQuery(Guid? Cursor, int Size) : 
    IRequest<Result<CursorPagedResult<DeletedCategoryResponse, Guid>>>, ICursorPaginationRequest<Guid>;
