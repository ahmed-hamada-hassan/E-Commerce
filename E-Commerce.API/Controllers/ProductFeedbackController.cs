using E_Commerce.API.Contracts;
using E_Commerce.Application.Common;
using E_Commerce.Application.Features.Feedbacks.Command.CreateFeedback;
using E_Commerce.Application.Features.Feedbacks.Command.Delete_Feedback;
using E_Commerce.Application.Features.Feedbacks.DTOs;
using E_Commerce.Application.Features.Feedbacks.Queries.Get_Product_Feedbacks;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace E_Commerce.API.Controllers;

[Route("api/feedbacks")]
[ApiController]
[Authorize(Policy = "Customer-Only")]
[EnableRateLimiting("FeedbackOperations")]
public class ProductFeedbackController : BaseApiController
{
    private readonly IMediator _mediator;

    public ProductFeedbackController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("/api/products/{productId:guid}/feedbacks")]
    [AllowAnonymous]
    public async Task<ActionResult<CursorPagedResult<ProductFeedbackResponse, Guid>>> Feedbacks([FromRoute] Guid productId,
        [FromQuery] CursorPaginationParams<Guid> paginationParams, CancellationToken cancellationToken)
    {
        var query = new GetProductFeedbacksQuery(productId, paginationParams.cursor, paginationParams.size);
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpPost("/api/products/{productId:guid}/feedbacks")]
    public async Task<ActionResult<Guid>> AddFeedback([FromRoute] Guid productId, [FromBody] AddProductFeedbackRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateFeedbackCommand(CurrentUserId, productId, request.Rating, request.Comment);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpPut("{feedbackId:guid}")]
    public async Task<ActionResult> UpdateFeedback([FromRoute] Guid feedbackId, [FromBody] UpdateProductFeedbackRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToEditFeedbackCommand(CurrentUserId, feedbackId);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }

    [HttpDelete("{feedbackId:guid}")]
    public async Task<ActionResult> DeleteFeedback([FromRoute] Guid feedbackId, CancellationToken cancellationToken)
    {
        var command = new DeleteFeedbackCommand(CurrentUserId, feedbackId);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }
}
