using E_Commerce.Application.Features.Wishlists.Commands.Add_To_Wishlist;
using E_Commerce.Application.Features.Wishlists.Commands.Remove_From_Wishlist;
using E_Commerce.Application.Features.Wishlists.DTOs;
using E_Commerce.Application.Features.Wishlists.Queries.Get_Wishlist;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace E_Commerce.API.Controllers;

[ApiController]
[Authorize]
[Route("api/Wishlists")]
[EnableRateLimiting("UserActions")]
public class WishlistController : BaseApiController
{
    private readonly IMediator _mediator;
    public WishlistController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("items/{productId:guid}")]
    public async Task<ActionResult<Guid>> Wishlist([FromRoute] Guid productId, CancellationToken ct)
    {
        var result = await _mediator.Send(new AddItemToWishlistCommand(CurrentUserId, productId), ct);
        return result.IsFailure ? HandleFailure(result) : CreatedAtAction(nameof(GetWishlist), null, result.Value);
    }

    [HttpGet]
    public async Task<ActionResult<WishlistResponse>> GetWishlist(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetUserWishlistQuery(CurrentUserId), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpDelete("items/{productId:guid}")]
    public async Task<ActionResult<bool>> RemoveItemFromWishlist([FromRoute] Guid productId, CancellationToken ct)
    {
        var result = await _mediator.Send(new RemoveItemFromWishlistCommand(CurrentUserId, productId), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }
}