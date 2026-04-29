using E_Commerce.Application.Common;
using E_Commerce.Application.Features.Users.Commands.BlockUser;
using E_Commerce.Application.Features.Users.Commands.DeleteUser;
using E_Commerce.Application.Features.Users.Commands.RestoreUser;
using E_Commerce.Application.Features.Users.Commands.UnBlockUser;
using E_Commerce.Application.Features.Users.DTOs;
using E_Commerce.Application.Features.Users.Queries.Admin_Get_Customer;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace E_Commerce.API.Controllers;

[Route("api/admin/customers")]
[ApiController]
[Authorize(policy: "SuperAdmin-Only")]
[EnableRateLimiting("UserRateLimit")]
public class AdminCustomersController : BaseApiController
{
    private readonly IMediator _mediator;

    public AdminCustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<CursorPagedResult<AdminCustomersResponse, Guid>>> GetCustomers(
        [FromQuery] CursorPaginationParams<Guid> paginationParams,
        [FromQuery] string? searchTerm,
        [FromQuery] string? status,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(
            new AdminGetCustomersQuery(paginationParams.cursor, paginationParams.size, searchTerm, status),
            ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<AdminCustomerResponse>> GetCustomer([FromRoute] Guid userId, CancellationToken ct)
    {
        var result = await _mediator.Send(new AdminGetCustomerQuery(userId), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpPatch("{userId:guid}/block")]
    public async Task<ActionResult> BlockCustomer([FromRoute] Guid userId, CancellationToken ct)
    {
        var command = await _mediator.Send(new BlockUserCommand(userId), ct);
        return command.IsFailure ? HandleFailure(command) : NoContent();
    }

    [HttpPatch("{userId:guid}/unblock")]
    public async Task<ActionResult> UnblockCustomer([FromRoute] Guid userId, CancellationToken ct)
    {
        var command = await _mediator.Send(new UnBlockUserCommand(userId), ct);
        return command.IsFailure ? HandleFailure(command) : NoContent();
    }

    [HttpDelete("{userId:guid}")]
    public async Task<ActionResult> DeleteCustomer([FromRoute] Guid userId, CancellationToken ct)
    {
        var command = await _mediator.Send(new DeleteUserCommand(userId), ct);
        return command.IsFailure ? HandleFailure(command) : NoContent();
    }

    [HttpPatch("{userId:guid}/restore")]
    public async Task<ActionResult> RestoreCustomer([FromRoute] Guid userId, CancellationToken ct)
    {
        var command = await _mediator.Send(new RestoreUserCommand(userId), ct);
        return command.IsFailure ? HandleFailure(command) : NoContent();
    }
}
