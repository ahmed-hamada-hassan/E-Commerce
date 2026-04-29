using E_Commerce.API.Contracts;
using E_Commerce.Application.Features.Users.Commands.UpdateUser;
using E_Commerce.Application.Features.Users.DTOs;
using E_Commerce.Application.Features.Vendors.DTOs;
using E_Commerce.Application.Features.Vendors.Queries.Get_Vendor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace E_Commerce.API.Controllers;

[Route("api/vendor/profile")]
[ApiController]
[Authorize(Policy = "Vendor-Only")]
[EnableRateLimiting("UserRateLimit")]
public class VendorProfileController : BaseApiController
{
    private readonly IMediator _mediator;

    public VendorProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<VendorProfileResponse>> GetMyProfile(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetVendorQuery(CurrentVendorId), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpPut("store")]
    public async Task<ActionResult> UpdateStoreInfo([FromBody] UpdateVendorStoreRequest request, CancellationToken ct)
    {
        var command = request.ToUpdateVendorStoreCommand(CurrentVendorId);
        var result = await _mediator.Send(command, ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }

    [HttpPut("personal")]
    public async Task<ActionResult> UpdatePersonalInfo([FromBody] UpdateCustomerInfoRequest request, CancellationToken ct)
    {
        var command = request.ToUpdateCustomerCommand(CurrentUserId);
        var result = await _mediator.Send(command, ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }

    [HttpPut("image")]
    public async Task<ActionResult> UpdateMyProfileImage([FromForm] FormFile? image, CancellationToken ct)
    {
        var result = await _mediator.Send(new UpdateUserImageCommand(CurrentUserId, image), ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }
}
