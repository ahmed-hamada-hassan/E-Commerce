using E_Commerce.Application.Features.Carts.Commands.AddToCart;
using E_Commerce.Application.Features.Carts.Commands.Buy_Now;
using E_Commerce.Application.Features.Carts.Commands.ClearCart;
using E_Commerce.Application.Features.Carts.Commands.RemoveItem;
using E_Commerce.Application.Features.Carts.Commands.UpdateItemQuantity;
using E_Commerce.Application.Features.Carts.DTOs;
using E_Commerce.Application.Features.Carts.Queries.Get_Buy_Now_Cart;
using E_Commerce.Application.Features.Carts.Queries.GetCart;
using E_Commerce.Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace E_Commerce.API.Controllers;

[Route("api/cart")]
[ApiController]
[EnableRateLimiting("GuestCartActions")]
public class CartController : BaseApiController
{
    private readonly IMediator _mediator;
    private readonly ICartContext _cartContext;

    public CartController(IMediator mediator, ICartContext cartContext)
    {
        _mediator = mediator;
        _cartContext = cartContext;
    }

    [HttpGet]
    public async Task<ActionResult<CartResponse>> Cart(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetCartQuery(_cartContext.CartId()), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("{cartId:guid}")]
    public async Task<ActionResult<BuyNowCartResponse>> BuyNowCart([FromRoute] Guid cartId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetByNowCartQuery(cartId), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpPost("items/{productId:guid}")]
    public async Task<ActionResult<CartSummaryResponse>> AddItem([FromRoute] Guid productId, [FromBody] int quantity, CancellationToken ct)
    {
        var result = await _mediator.Send(new AddToCartCommand(productId, _cartContext.CartId(), quantity), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpPost("buy-now/items/{productId:guid}")]
    public async Task<ActionResult<Guid>> BuyNow([FromRoute] Guid productId, [FromBody] byte quantity, CancellationToken ct)
    {
        var result = await _mediator.Send(new BuyNowCommand(productId, quantity), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpPut("items/{productId:guid}")]
    public async Task<ActionResult<CartSummaryResponse>> UpdateItem([FromRoute] Guid productId, [FromBody] int quantity, CancellationToken ct)
    {
        var result = await _mediator.Send(new UpdateItemQuantityCommand(productId, _cartContext.CartId(), quantity), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpDelete("items/{productId:guid}")]
    public async Task<ActionResult<CartSummaryResponse>> RemoveItem([FromRoute] Guid productId, CancellationToken ct)
    {
        var result = await _mediator.Send(new RemoveItemCommand(productId, _cartContext.CartId()), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpDelete]
    public async Task<ActionResult> ClearCart(CancellationToken ct)
    {
        var result = await _mediator.Send(new ClearCartCommand(_cartContext.CartId()), ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }
}
