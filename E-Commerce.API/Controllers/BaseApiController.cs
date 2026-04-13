using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    private IUserContext? _userContext;
    protected IUserContext UserContext => _userContext ??= HttpContext.RequestServices.GetRequiredService<IUserContext>();
    protected Guid CurrentUserId => UserContext.UserId;
    protected Guid CurrentVendorId => UserContext.VendorId ?? Guid.Empty;
    protected bool IsSuperAdmin => UserContext.IsInRole(AppRoles.SuperAdmin);
    protected ActionResult HandleFailure<T>(Result<T> result)
    {
        return CreateErrorResponse(result.Error);
    }
    private ActionResult CreateErrorResponse(Error error)
    {
        if (error.Code.Contains("AccessDenied", StringComparison.OrdinalIgnoreCase) ||
            error.Code.Contains("AccountLocked", StringComparison.OrdinalIgnoreCase) ||
            error.Code.Contains("NotActive", StringComparison.OrdinalIgnoreCase))
            return StatusCode(StatusCodes.Status403Forbidden, error);

        if (error.Code.Contains("NotFound", StringComparison.OrdinalIgnoreCase))
            return StatusCode(StatusCodes.Status404NotFound, error);

        if (error.Code.Contains("Conflict", StringComparison.OrdinalIgnoreCase) ||
            error.Code.Contains("AlreadyExists", StringComparison.OrdinalIgnoreCase))
            return StatusCode(StatusCodes.Status409Conflict, error);

        if(error.Code.Contains("InvalidCredentials", StringComparison.OrdinalIgnoreCase) ||
            error.Code.Contains("InvalidToken", StringComparison.OrdinalIgnoreCase) ||
            error.Code.Contains("InvalidRefreshToken", StringComparison.OrdinalIgnoreCase))
            return StatusCode(StatusCodes.Status401Unauthorized, error);

        return StatusCode(StatusCodes.Status400BadRequest, error);
    }
}
