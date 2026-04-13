using E_Commerce.Application.Common;
using E_Commerce.Application.Features.Users.Commands.BlockUser;
using E_Commerce.Application.Features.Users.Commands.DeleteUser;
using E_Commerce.Application.Features.Users.Commands.RestoreUser;
using E_Commerce.Application.Features.Users.Commands.UnBlockUser;
using E_Commerce.Application.Features.Users.DTOs;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace E_Commerce.API.Controllers;

[Route("api/admin/users")]
[ApiController]
[Authorize(policy: "SuperAdmin-Only")]
[EnableRateLimiting("UserRateLimit")]
public class AdminUsersController : BaseApiController
{
    private readonly IMediator _mediator;

    public AdminUsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPatch("{userId:guid}/block")]
    public async Task<ActionResult> BlockUser([FromRoute] Guid userId, CancellationToken ct)
    {
        var command = await _mediator.Send(new BlockUserCommand(userId), ct);
        return command.IsFailure ? HandleFailure(command) : NoContent();
    }

    [HttpPatch("{userId:guid}/unblock")]
    public async Task<ActionResult> UnBlockUser([FromRoute] Guid userId, CancellationToken ct)
    {
        var command = await _mediator.Send(new UnBlockUserCommand(userId), ct);
        return command.IsFailure ? HandleFailure(command) : NoContent();
    }

    [HttpDelete("{userId:guid}")]
    public async Task<ActionResult> DeleteUser([FromRoute] Guid userId, CancellationToken ct)
    {
        var command = await _mediator.Send(new DeleteUserCommand(userId), ct);
        return command.IsFailure ? HandleFailure(command) : NoContent();
    }

    [HttpPatch("{userId:guid}/restore")]
    public async Task<ActionResult> RestoreUser([FromRoute] Guid userId, CancellationToken ct)
    {
        var command = await _mediator.Send(new RestoreUserCommand(userId), ct);
        return command.IsFailure ? HandleFailure(command) : NoContent();
    }

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<UserResponse>> GetUser([FromRoute] Guid userId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetUserQuery(userId), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("{userId:guid}/blocked")]
    public async Task<ActionResult<BlockedUserResponse>> GetBlockedUser([FromRoute] Guid userId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetBlockedUserQuery(userId), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("{userId:guid}/deleted")]
    public async Task<ActionResult<DeletedUserResponse>> GetDeletedUser([FromRoute] Guid userId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetDeletedUserQuery(userId), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("blocked")]
    public async Task<ActionResult<CursorPagedResult<BlockedUserResponse, Guid>>> GetBlockedUsers(
        [FromQuery] CursorPaginationParams<Guid> paginationPrams, CancellationToken ct = default)
    {
        var result =
            await _mediator.Send(new GetBlockedUsersQuery(paginationPrams.cursor, paginationPrams.size), ct);

        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("deleted")]
    public async Task<ActionResult<CursorPagedResult<DeletedUserResponse, Guid>>> GetDeletedUsers(
        [FromQuery] CursorPaginationParams<Guid> paginationPrams, CancellationToken ct = default)
    {
        var result =
            await _mediator.Send(new GetDeletedUsersQuery(paginationPrams.cursor, paginationPrams.size), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet]
    public async Task<ActionResult<CursorPagedResult<UserResponse, Guid>>> GetUsers(
        [FromQuery] CursorPaginationParams<Guid> paginationPrams, CancellationToken ct = default)
    {
        var result =
            await _mediator.Send(new GetUsersQuery(paginationPrams.cursor, paginationPrams.size), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }
}
