using E_Commerce.API.Contracts;
using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Application.Features.Products.Queries.GetProduct;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace E_Commerce.API.Controllers;

[Route("api/customer/products")]
[ApiController]
[AllowAnonymous]
[EnableRateLimiting("UserRateLimit")]
public class CustomerProductsController : BaseApiController
{
    private readonly IMediator _mediator;

    public CustomerProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<OffsetPagedResult<CustomerProductResponse>>> Products([FromQuery] CustomerProductsRequest productRequest, CancellationToken ct)
    {
        var result = await _mediator.Send(productRequest.ToGetProductQuery(), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustomerProductResponse>> Product(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetProductQuery(id), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }
}
