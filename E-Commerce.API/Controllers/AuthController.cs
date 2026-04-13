using E_Commerce.API.Contracts;
using E_Commerce.Application.Features.Auth.Command.Login;
using E_Commerce.Application.Features.Auth.Command.RefreshToken;
using E_Commerce.Application.Features.Auth.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace E_Commerce.API.Controllers;

[Route("api/Auth")]
[ApiController]
[EnableRateLimiting("IpRateLimit")]
public class AuthController : BaseApiController
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register-customer")]
    public async Task<ActionResult<AuthResponse>> RegisterCustomer([FromForm] RegisterCustomerRequest request)
    {
        var registerCustomer = request.ToRegisterCustomerCommand();
        var result = await _mediator.Send(registerCustomer);

        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpPost("register-vendor")]
    public async Task<ActionResult<AuthResponse>> RegisterVendor([FromForm] RegisterVendorRequest request)
    {
        var registerVendor = request.ToRegisterVendorCommand();
        var result = await _mediator.Send(registerVendor);

        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }


    [HttpPost("register")]
    [Authorize(Policy = "SuperAdmin-Only")]
    public async Task<ActionResult<AuthResponse>> Register([FromForm] RegisterRequest request)
    {
        var register = request.ToRegisterCommand();
        var result = await _mediator.Send(register);
        
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        var result = await _mediator.Send(new LoginCommand(request.Email, request.Password));
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await _mediator.Send(new RefreshTokenCommand(request.AccessToken, request.RefreshToken));
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }
}
