using E_Commerce.Application.Common;
using E_Commerce.Application.Features.Orders.Commands.Representative_Return_Request_Order;
using E_Commerce.Application.Features.Orders.DTOs;
using E_Commerce.Application.Features.Orders.Queries.Get_Approved_Return_Requests_For_Representative;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace E_Commerce.API.Controllers;

[Route("api/representative")]
[ApiController]
[Authorize(Policy = "Representative-SuperAdmin-Only")]
[EnableRateLimiting("RepresentativeOperations")]
public class RepresentativeOrderController : BaseApiController
{
    private readonly IMediator _mediator;

    public RepresentativeOrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("status/{returnReqId:guid}")]
    public async Task<ActionResult<bool>> CompleteRejectReturnRequest([FromRoute] Guid returnReqId, [FromBody] CompleteRejectReturnRequest request,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(new CompleteReturnRequestCommand(returnReqId, CurrentUserId, request.Status, request.Reason), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("returns/approved")]
    public async Task<ActionResult<CursorPagedResult<ApprovedReturnRequestResponse, Guid>>> GetApprovedReturnRequestsForRepresentative(
        [FromQuery] CursorPaginationParams<Guid> paginationParams, CancellationToken ct = default)
    {
        var query = new GetApprovedReturnRequestsForRepresentativeQuery(paginationParams.cursor, paginationParams.size);
        var result = await _mediator.Send(query, ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }
}
