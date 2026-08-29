using E_Commerce.API.Contracts;
using E_Commerce.Application.Features.Vendors.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace E_Commerce.API.Controllers;

[Route("api/vendor/store/profile")]
[ApiController]
[Authorize(Policy = "Vendor-Only")]
[EnableRateLimiting("ProfileManagement")]
public class VendorStoreProfileController : BaseApiController
{
    private readonly IMediator _mediator;

    public VendorStoreProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut]
    public async Task<ActionResult> UpdateStoreInfo([FromBody] UpdateVendorStoreRequest request, CancellationToken ct)
    {
        var command = request.ToUpdateVendorStoreCommand(CurrentVendorId);
        var result = await _mediator.Send(command, ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }
}
