using E_Commerce.API.Contracts;
using E_Commerce.Application.Features.Users.Commands.UpdateUser;
using E_Commerce.Application.Features.Users.DTOs;
using E_Commerce.Application.Features.Users.Queries.Get_Customer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace E_Commerce.API.Controllers;

[Route("api/user/profile")]
[ApiController]
[Authorize]
public class UserProfileController : BaseApiController
{
    private readonly IMediator _mediator;

    public UserProfileController(IMediator mediator)
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
    [EnableRateLimiting("ProfileManagement")]
    public async Task<ActionResult> UpdateMyProfile([FromBody] UpdateCustomerInfoRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(request.ToUpdateCustomerCommand(CurrentUserId), ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }

    [HttpPut("image")]
    [EnableRateLimiting("ProfileManagement")]
    public async Task<ActionResult> UpdateMyProfileImage([FromForm] CustomerImageRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new UpdateUserImageCommand(CurrentUserId, request.Image), ct);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }
}
