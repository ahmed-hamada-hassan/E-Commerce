using E_Commerce.API.Contracts;
using E_Commerce.Application.Features.Users.Commands.UpdateUser;
using E_Commerce.Application.Features.Users.DTOs;
using E_Commerce.Application.Features.Users.Queries.Get_Customer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace E_Commerce.API.Controllers;

[Route("api/customer/profile")]
[ApiController]
[Authorize(Policy = "Customer-Only")]
[EnableRateLimiting("UserRateLimit")]
public class CustomerProfileController : BaseApiController
{
    private readonly IMediator _mediator;

    public CustomerProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<CustomerProfileResponse>> GetMyProfile(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetCustomerQuery(CurrentUserId), ct);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateMyProfile([FromBody] UpdateCustomerInfoRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(request.ToUpdateCustomerCommand(CurrentUserId), ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }

    [HttpPut("image")]
    public async Task<ActionResult> UpdateMyProfileImage([FromForm] FormFile? image, CancellationToken ct)
    {
        var result = await _mediator.Send(new UpdateUserImageCommand(CurrentUserId, image), ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }
}
