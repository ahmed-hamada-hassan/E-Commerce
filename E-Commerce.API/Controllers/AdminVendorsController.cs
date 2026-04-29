using E_Commerce.Application.Common;
using E_Commerce.Application.Features.Vendors.Commands.Active_Vendor;
using E_Commerce.Application.Features.Vendors.Commands.Deactive_Vendor;
using E_Commerce.Application.Features.Vendors.DTOs;
using E_Commerce.Application.Features.Vendors.Queries.Admin_Get_Vendor;
using E_Commerce.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace E_Commerce.API.Controllers;

[Route("api/admin/vendors")]
[ApiController]
[Authorize(policy: "SuperAdmin-Only")]
[EnableRateLimiting("UserRateLimit")]
public class AdminVendorsController : BaseApiController
{
    private readonly IMediator _mediator;

    public AdminVendorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<AdminVendorResponse>> GetVendor([FromRoute] Guid vendorId, CancellationToken ct)
    {
        var result = await _mediator.Send(new AdminGetVendorQuery(vendorId), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet]
    public async Task<ActionResult<CursorPagedResult<AdminVendorResponse, Guid>>> GetVendors(
        [FromQuery] CursorPaginationParams<Guid> paginationParams,
        [FromQuery] string? searchTerm,
        [FromQuery] string? status,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(
            new AdminGetVendorsQuery(paginationParams.cursor, paginationParams.size, searchTerm, status), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpPatch("{vendorId:guid}/active")]
    public async Task<ActionResult> ActivateVendor([FromRoute] Guid vendorId, CancellationToken ct)
    {
        var command = await _mediator.Send(new ActiveVendorCommand(vendorId), ct);

        return command.IsFailure ? HandleFailure(command) : NoContent();
    }

    [HttpPatch("{vendorId:guid}/deactive")]
    public async Task<ActionResult> DeActivateVendor([FromRoute] Guid vendorId, CancellationToken ct)
    {
        var command = await _mediator.Send(new DeactiveVendorCommand(vendorId), ct);

        return command.IsFailure ? HandleFailure(command) : NoContent();
    }
}
