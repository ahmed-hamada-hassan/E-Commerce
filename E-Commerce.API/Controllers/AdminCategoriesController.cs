using E_Commerce.API.Contracts;
using E_Commerce.Application.Common;
using E_Commerce.Application.Features.Categories.Commands.DeleteCategory;
using E_Commerce.Application.Features.Categories.Commands.RestoreCategory;
using E_Commerce.Application.Features.Categories.DTOs;
using E_Commerce.Application.Features.Categories.Queries.GetCategories;
using E_Commerce.Application.Features.Categories.Queries.GetCategory;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace E_Commerce.API.Controllers;

[Route("api/admin/categories")]
[ApiController]
[Authorize(Policy = "SuperAdmin-Only")]
[EnableRateLimiting("UserRateLimit")]
public class AdminCategoriesController : BaseApiController
{
    private readonly IMediator _mediator;

    public AdminCategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Category([FromBody] CreateCategoryRequest categoryRequest, CancellationToken ct)
    {
        var result = await _mediator.Send(categoryRequest.ToCategoryCommand(), ct);

        return result.IsFailure ? HandleFailure(result) : Created($"/api/admin/categories/{result.Value}", new { ID = result.Value });
    }

    [HttpGet]
    public async Task<ActionResult<CursorPagedResult<CategoryResponse, Guid>>> Categories([FromQuery] CursorPaginationParams<Guid> paginationParams,
        CancellationToken ct)
    {
        var result = await _mediator.Send(
            new GetCategoriesQuery(paginationParams.cursor, paginationParams.size), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("deleted")]
    public async Task<ActionResult<CursorPagedResult<DeletedCategoryResponse, Guid>>> DeletedCategories(
        [FromQuery] CursorPaginationParams<Guid> paginationParams, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new GetDeletedCategoriesQuery(paginationParams.cursor, paginationParams.size), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CategoryResponse>> Category([FromRoute] Guid Id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetCategoryQuery(Id), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("{id:guid}/deleted")]
    public async Task<ActionResult<DeletedCategoryResponse>> DeletedCategory([FromRoute] Guid Id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetDeletedCategoryQuery(Id), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Category([FromRoute] Guid Id, [FromBody] UpdateCategoryRequest categoryRequest, CancellationToken ct)
    {
        var result = await _mediator.Send(categoryRequest.ToUpdateCategoryCommand(Id), ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }

    [HttpPatch("{id:guid}/restore")]
    public async Task<ActionResult> RestoreCategory([FromRoute] Guid Id, CancellationToken ct)
    {
        var result = await _mediator.Send(new RestoreCategoryCommand(Id), ct);

        return result.IsFailure ? HandleFailure(result) : NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteCategory([FromRoute] Guid Id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteCategoryCommand(Id), ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }
}