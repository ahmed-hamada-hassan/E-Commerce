using E_Commerce.API.Contracts;
using E_Commerce.Application.Common;
using E_Commerce.Application.Features.Auth.Command.Login;
using E_Commerce.Application.Features.Auth.Command.Logout;
using E_Commerce.Application.Features.Auth.Command.RefreshToken;
using E_Commerce.Application.Features.Auth.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;

namespace E_Commerce.API.Controllers;

[Route("api/Auth")]
[ApiController]
public class AuthController : BaseApiController
{
    private readonly IMediator _mediator;
    private readonly JWTSettings _jwtSettings;

    public AuthController(IMediator mediator, IOptionsSnapshot<JWTSettings> jwtSettings)
    {
        _mediator = mediator;
        _jwtSettings = jwtSettings.Value;
    }

    [HttpPost("register-customer")]
    [EnableRateLimiting("Signup")]
    public async Task<IActionResult> RegisterCustomer([FromForm] RegisterCustomerRequest request)
    {
        var registerCustomer = request.ToRegisterCustomerCommand();
        var result = await _mediator.Send(registerCustomer);

        if (result.IsFailure) return HandleFailure(result);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays)
        };

        Response.Cookies.Append("refreshToken", result.Value!.RefreshToken, cookieOptions);

        return Ok(new
        {
            AccessToken = result.Value.AccessToken
        });
    }

    [HttpPost("register-vendor")]
    [EnableRateLimiting("Signup")]
    public async Task<IActionResult> RegisterVendor([FromForm] RegisterVendorRequest request)
    {
        var registerVendor = request.ToRegisterVendorCommand();
        var result = await _mediator.Send(registerVendor);

        if (result.IsFailure) return HandleFailure(result);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays)
        };

        Response.Cookies.Append("refreshToken", result.Value!.RefreshToken, cookieOptions);

        return Ok(new
        {
            AccessToken = result.Value.AccessToken
        });
    }


    [HttpPost("register")]
    [Authorize(Policy = "SuperAdmin-Only")]
    [EnableRateLimiting("AdminSignup")]
    public async Task<IActionResult> Register([FromForm] RegisterRequest request)
    {
        var register = request.ToRegisterCommand();
        var result = await _mediator.Send(register);

        if(result.IsFailure) return HandleFailure(result);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays)
        };

        Response.Cookies.Append("refreshToken", result.Value!.RefreshToken, cookieOptions);

        return Ok(new
        {
            AccessToken = result.Value.AccessToken
        });
    }

    [HttpPost("login")]
    [EnableRateLimiting("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _mediator.Send(new LoginCommand(request.Email, request.Password));
        if (result.IsFailure) return HandleFailure(result);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/",
            Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays)
        };

        Response.Cookies.Append("refreshToken", result.Value!.RefreshToken, cookieOptions);

        return Ok(new
        {
            AccessToken = result.Value.AccessToken
        });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var command = new LogoutCommand(CurrentUserId);
        var result = await _mediator.Send(command);

        if (result.IsFailure) return HandleFailure(result);

        Response.Cookies.Delete("refreshToken");

        return Ok();
    }

    [HttpPost("refresh-token")]
    [EnableRateLimiting("RefreshToken")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if(string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized("Refresh token is missing.");
        }

        var command = new RefreshTokenCommand(refreshToken);
        var result = await _mediator.Send(command);
        
        if(result.IsFailure) return HandleFailure(result);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("refreshToken", result.Value!.RefreshToken, cookieOptions);

        return Ok(new
        {
            AccessToken = result.Value.AccessToken
        });
    }
}
