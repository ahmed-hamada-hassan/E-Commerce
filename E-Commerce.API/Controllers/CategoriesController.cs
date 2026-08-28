using E_Commerce.API.Contracts;
using E_Commerce.Application.Common;
using E_Commerce.Application.Features.Categories.DTOs;
using E_Commerce.Application.Features.Categories.Queries.Public_Get_Categories;
using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers;

[Route("api/categories")]
[ApiController]
[AllowAnonymous]
//[EnableRateLimiting("IpRateLimit")]
public class CategoriesController : BaseApiController
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<CursorPagedResult<PublicCategoryResponse, Guid>>> Categories([FromQuery] CursorPaginationParams<Guid> paginationParams,
        CancellationToken ct)
    {
        var result = await _mediator.Send(
            new PublicGetCategoriesQuery(paginationParams.cursor, paginationParams.size), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("{categoryId:guid}/products")]
    public async Task<ActionResult<OffsetPagedResult<CustomerProductDetailsResponse>>> FilteredCategoryProducts([FromRoute] Guid categoryId,
        [FromQuery] CustomerProductsRequest productRequest,
        CancellationToken ct)
    {
        var result = await _mediator.Send(productRequest.ToGetProductQuery(categoryId), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }
}
