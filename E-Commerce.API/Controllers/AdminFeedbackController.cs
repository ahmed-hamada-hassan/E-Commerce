using E_Commerce.Application.Common;
using E_Commerce.Application.Features.Feedbacks.Command.Approve_Feedback;
using E_Commerce.Application.Features.Feedbacks.DTOs;
using E_Commerce.Application.Features.Feedbacks.Queries.Admin_Get_Pending_Feedbacks;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace E_Commerce.API.Controllers;

[Route("api/admin/feedbacks")]
[ApiController]
[Authorize(Policy = "SuperAdmin-Only")]
[EnableRateLimiting("UserRateLimit")]
public class AdminFeedbackController : BaseApiController
{
    private readonly IMediator _mediator;

    public AdminFeedbackController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("pending")]
    public async Task<ActionResult<CursorPagedResult<PendingFeedbackResponse, Guid>>> GetPendingFeedbacks(
        [FromQuery] CursorPaginationParams<Guid> paginationParams, CancellationToken ct)
    {
        var result = 
            await _mediator.Send(new AdminGetPendingFeedbacksQuery(paginationParams.cursor, paginationParams.size), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpPatch("{feedbackId:guid}/approve")]
    public async Task<ActionResult> ApproveFeedback([FromRoute] Guid feedbackId, CancellationToken ct)
    {
        var result = await _mediator.Send(new ApproveFeedbackCommand(CurrentUserId, feedbackId), ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }
}
