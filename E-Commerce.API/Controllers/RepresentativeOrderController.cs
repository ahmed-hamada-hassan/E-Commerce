using E_Commerce.Application.Features.Orders.Commands.Representative_Return_Request_Order;
using E_Commerce.Application.Features.Orders.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace E_Commerce.API.Controllers;

[Route("api/representative")]
[ApiController]
[Authorize(Policy = "Representative-SuperAdmin-Only")]
[EnableRateLimiting("UserRateLimit")]
public class RepresentativeOrderController : BaseApiController
{
    private readonly IMediator _mediator;

    public RepresentativeOrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{returnReqId:guid}/compelete-reject-return-req")]
    public async Task<ActionResult<bool>> CompleteRejectReturnRequest([FromRoute] Guid returnReqId, [FromBody] CompleteRejectReturnRequest request,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(new CompleteReturnRequestCommand(returnReqId, CurrentUserId, request.Status, request.Reason), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }
}
