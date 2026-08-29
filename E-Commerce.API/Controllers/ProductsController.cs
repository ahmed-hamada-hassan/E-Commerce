using E_Commerce.API.Contracts;
using E_Commerce.Application.Features.Products.DTOs;
using E_Commerce.Application.Features.Products.Queries.Customer_Get_Product;
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
public class ProductsController : BaseApiController
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [EnableRateLimiting("SearchProducts")]
    public async Task<ActionResult<OffsetPagedResult<CustomerProductDetailsResponse>>> Products([FromQuery] CustomerProductsRequest productRequest, CancellationToken ct)
    {
        var result = await _mediator.Send(productRequest.ToGetProductQuery(null), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("batch")]
    [EnableRateLimiting("PublicBrowsing")]
    public async Task<ActionResult<IEnumerable<CustomerProductDetailsResponse>>> ProductsByIds([FromQuery] List<string> ids, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetProductsByIdsQuery(ids), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    [EnableRateLimiting("PublicBrowsing")]
    public async Task<ActionResult<CustomerProductDetailsResponse>> Product(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetProductQuery(id), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }
}
