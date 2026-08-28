using E_Commerce.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace E_Commerce.Infrastructure.Services;

internal sealed class CartContext : ICartContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CartContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid CartId()
    {
        var context = _httpContextAccessor.HttpContext;

        if(context is null) return Guid.NewGuid();

        if(context.User.Identity?.IsAuthenticated == true)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }
        }

        if (context.Request.Headers.TryGetValue("X-Cart-Id", out var headerId) &&
            Guid.TryParse(headerId, out var guestCartId))
        {
            return guestCartId;
        }

        return Guid.NewGuid();
    }
}
