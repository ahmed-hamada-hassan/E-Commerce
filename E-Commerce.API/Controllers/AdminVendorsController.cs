using E_Commerce.Application.Features.Users.Commands.Active_Vendor;
using E_Commerce.Application.Features.Users.Commands.Deactive_Vendor;
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
