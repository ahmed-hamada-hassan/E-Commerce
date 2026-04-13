using E_Commerce.Application.Interfaces.Dependency_Injection;
using E_Commerce.Domain.Entities;
using System.Security.Claims;

namespace E_Commerce.Application.Interfaces.Services;

public interface ITokenService : IScopedService
{
    Task<string> GenerateAccessToken(ApplicationUser user, CancellationToken ct);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
