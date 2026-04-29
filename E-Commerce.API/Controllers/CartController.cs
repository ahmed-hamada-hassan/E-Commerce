using E_Commerce.Application.Features.Carts.Commands.AddToCart;
using E_Commerce.Application.Features.Carts.Commands.ClearCart;
using E_Commerce.Application.Features.Carts.Commands.RemoveItem;
using E_Commerce.Application.Features.Carts.Commands.UpdateItemQuantity;
using E_Commerce.Application.Features.Carts.DTOs;
using E_Commerce.Application.Features.Carts.Queries.GetCart;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace E_Commerce.API.Controllers;

[Route("api/cart")]
[ApiController]
[Authorize(Policy = "Customer-Only")]
[EnableRateLimiting("UserRateLimit")]
public class CartController : BaseApiController
{
    private readonly IMediator _mediator;

    public CartController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<CartResponse>> Cart(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetCartQuery(CurrentUserId), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpPost("items/{productId:guid}")]
    public async Task<ActionResult> AddItem([FromRoute] Guid productId, [FromBody] int quantity, CancellationToken ct)
    {
        var result = await _mediator.Send(new AddToCartCommand(productId, CurrentUserId, quantity), ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }

    [HttpPut("items/{productId:guid}")]
    public async Task<ActionResult> UpdateItem([FromRoute] Guid productId, [FromBody] int quantity, CancellationToken ct)
    {
        var result = await _mediator.Send(new UpdateItemQuantityCommand(productId, CurrentUserId, quantity), ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }

    [HttpDelete("items/{productId:guid}")]
    public async Task<ActionResult> RemoveItem(Guid productId, CancellationToken ct)
    {
        var result = await _mediator.Send(new RemoveItemCommand(productId, CurrentUserId), ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> ClearCart(CancellationToken ct)
    {
        var result = await _mediator.Send(new ClearCartCommand(CurrentUserId), ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }
}
