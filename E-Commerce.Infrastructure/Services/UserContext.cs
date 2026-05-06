using E_Commerce.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace E_Commerce.Infrastructure.Services;

internal sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
    public Guid UserId => Guid.TryParse(User?.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : Guid.Empty;
    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
    public string? Email => User?.FindFirstValue(JwtRegisteredClaimNames.Email);
    public Guid? VendorId => Guid.TryParse(User?.FindFirstValue("vendor_id"), out var vendorId) ? vendorId : null;
    public bool IsInRole(string role) => User?.IsInRole(role) ?? false;
}
