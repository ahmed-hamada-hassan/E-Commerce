using E_Commerce.Application.Common;
using E_Commerce.Application.Features.Orders.Commands.Cancel_Order;
using E_Commerce.Application.Features.Orders.Commands.Place_Order;
using E_Commerce.Application.Features.Orders.Commands.Return_Request_Order;
using E_Commerce.Application.Features.Orders.DTOs;
using E_Commerce.Application.Features.Orders.Queries.Get_My_Order;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace E_Commerce.API.Controllers;

[Route("api/orders")]
[ApiController]
[Authorize(Policy = "Customer-Only")]
[EnableRateLimiting("UserRateLimit")]
public class OrdersController : BaseApiController
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> PlaceOrder([FromBody] PlaceOrderRequest orderRequest, CancellationToken ct)
    {
        var result = await _mediator.Send(new PlaceOrderCommand(CurrentUserId, orderRequest.UseDefaultShippingAddress,
            orderRequest.AddressId, orderRequest.NewAddress, orderRequest.PaymentMethod), ct);
        return result.IsFailure ? HandleFailure(result) : CreatedAtAction(nameof(GetDetailsOrder), new { orderId = result.Value }, result.Value);
    }

    [HttpPost("{orderId:guid}/cancel")]
    public async Task<ActionResult> CancelOrder([FromRoute] Guid orderId, [FromBody] CancelOrderRequest cancelOrderRequest, CancellationToken ct)
    {
        var result = await _mediator.Send(new CancelOrderCommand(orderId, CurrentUserId, cancelOrderRequest.Reason), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("{orderId:guid}")]
    public async Task<ActionResult<OrderDetailsResponse>> GetDetailsOrder([FromRoute] Guid orderId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetMyOrderDetailsQuery(orderId, CurrentUserId), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet]
    public async Task<ActionResult<CursorPagedResult<OrderSummaryResponse, Guid>>> Orders([FromQuery] CursorPaginationParams<Guid> paginationParams,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new GetMyOrderSummaryQuery(CurrentUserId,
            paginationParams.cursor, paginationParams.size), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpPost("{orderId:guid}/return-request")]
    public async Task<ActionResult> RequestReturn([FromRoute] Guid orderId, [FromBody] ReturnRequestOrderRequest returnRequest, CancellationToken ct)
    {
        var command = new ReturnRequestOrderCommand(orderId, CurrentUserId, returnRequest.Items, returnRequest.Reason);
        var result = await _mediator.Send(command, ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }
}
