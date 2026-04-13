using E_Commerce.Application.Features.Categories.DTOs;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Shared;
using MediatR;

namespace E_Commerce.Application.Features.Categories.Queries.Public_Get_Categories;

public record PublicGetCategoriesQuery(Guid? Cursor, int Size) : 
    IRequest<Result<CursorPagedResult<PublicCategoryResponse, Guid>>>, ICursorPaginationRequest<Guid>;