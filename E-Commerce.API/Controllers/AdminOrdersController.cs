using E_Commerce.Application.Common;
using E_Commerce.Application.Features.Orders.Commands.Admin_Return_Request_Order;
using E_Commerce.Application.Features.Orders.Commands.Representative_Return_Request_Order;
using E_Commerce.Application.Features.Orders.Commands.Shipped_Order;
using E_Commerce.Application.Features.Orders.DTOs;
using E_Commerce.Application.Features.Orders.Queries.Admin_Get_Order;
using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace E_Commerce.API.Controllers;

[Route("api/admin/orders")]
[ApiController]
[Authorize(Policy = "Admin-SuperAdmin-Only")]
[EnableRateLimiting("UserRateLimit")]
public class AdminOrdersController : BaseApiController
{
    private readonly IMediator _mediator;

    public AdminOrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("processing")]
    public async Task<ActionResult<AdminOrdersProcessingResponse>> GetProcessingOrders([FromQuery] DateTimeOffset day,
        [FromQuery] CursorPaginationParams<Guid> paginationParams, CancellationToken ct = default)
    {
        var result =
            await _mediator.Send(new GetSummaryProcessingOrdersQuery(paginationParams.size, paginationParams.cursor, day), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpPatch("{orderId:guid}/shipped")]
    public async Task<ActionResult<Result<bool>>> MarkOrderAsShipped([FromRoute] Guid orderId, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new ShippedOrderCommand(orderId), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("overview")]
    public async Task<ActionResult<AdminOverviewResponse>> GetOrdersOverview([FromQuery] OverviewDatesRequest request, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetAdminOverviewQuery(request.FromDate, request.ToDate), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpPost("{returnReqId:guid}/accept-reject-return-req")]
    public async Task<ActionResult<bool>> AcceptRejectReturnRequest([FromRoute] Guid returnReqId, [FromBody] ReturnStatus accept, 
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(new AdminReturnRequestOrderCommand(returnReqId, accept), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }
}
