using E_Commerce.Application.Features.Addresses.Commands;
using E_Commerce.Application.Features.Addresses.Commands.Delete_Address;
using E_Commerce.Application.Features.Addresses.Commands.Set_Address_Default_Shipping;
using E_Commerce.Application.Features.Addresses.Commands.Update_Address;
using E_Commerce.Application.Features.Addresses.DTOs;
using E_Commerce.Application.Features.Addresses.Queries.Get_Address;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace E_Commerce.API.Controllers;

[Route("api/addresses")]
[ApiController]
[Authorize]
[EnableRateLimiting("UserRateLimit")]
public class AddressesController : BaseApiController
{
    private readonly IMediator _mediator;

    public AddressesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<GetAddressInfo>>> GetMyAddresses(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetMyAddressesQuery(CurrentUserId), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("{addressId:guid}")]
    public async Task<ActionResult<GetAddressInfo>> GetAddress(Guid addressId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAddressQuery(CurrentUserId, addressId), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<List<AddAddressResponse>>> AddAddresses([FromBody] List<AddAddressInfo> addresses, CancellationToken ct)
    {
        var result = await _mediator.Send(new AddAddressCommand(CurrentUserId, addresses), ct);
        return result.IsFailure ? HandleFailure(result) : CreatedAtAction(nameof(GetMyAddresses), result.Value);
    }

    [HttpPut("{addressId:guid}")]
    public async Task<ActionResult> UpdateAddress(Guid addressId, [FromBody] UpdateAddressInfo request, CancellationToken ct)
    {
        var result = await _mediator.Send(new UpdateAddressCommand(CurrentUserId, addressId, request), ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }

    [HttpPatch("{addressId:guid}/set-default")]
    public async Task<ActionResult> SetDefaultAddress(Guid addressId, CancellationToken ct)
    {
        var result = await _mediator.Send(new SetAddressDefaultShippingCommand(CurrentUserId, addressId), ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }

    [HttpDelete("{addressId:guid}")]
    public async Task<ActionResult> DeleteAddress(Guid addressId, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteAddressCommand(CurrentUserId, addressId), ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }
}
