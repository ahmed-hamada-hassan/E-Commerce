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
    public async Task<ActionResult> GetMyProfile(CancellationToken ct)
    {
    }
}
