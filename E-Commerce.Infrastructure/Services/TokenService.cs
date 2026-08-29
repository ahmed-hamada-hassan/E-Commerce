using E_Commerce.Application.Common;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace E_Commerce.Infrastructure.Services;

internal sealed class TokenService : ITokenService
{
    private readonly JWTSettings _jwtSettings;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IVendorRepository _vendorRepository;

    public TokenService(IOptionsSnapshot<JWTSettings> jwtSettings, UserManager<ApplicationUser> userManager, IVendorRepository vendorRepository)
    {
        _jwtSettings = jwtSettings.Value;
        _userManager = userManager;
        _vendorRepository = vendorRepository;
    }

    public async Task<string> GenerateAccessToken(ApplicationUser user, CancellationToken ct)
    {
        var keyBytes = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey.Trim());
        var key = new SymmetricSecurityKey(keyBytes);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FullName),
            new Claim(JwtRegisteredClaimNames.Birthdate, user.DateOfBirth.ToString("yyyy-MM-dd")),
            new Claim(JwtRegisteredClaimNames.PhoneNumber, user.PhoneNumber!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var userRoles = await _userManager.GetRolesAsync(user);
        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        if(userRoles.Contains(AppRoles.Vendor))
        {
            var vendor = await _vendorRepository.GetByUserIdAsync(user.Id, ct);
            if(vendor is not null)
                claims.Add(new Claim("vendor_id", vendor.Id.ToString()));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationInMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {

        var keyBytes = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = _jwtSettings.Audience,

            ValidateIssuer = true,
            ValidIssuer = _jwtSettings.Issuer,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),

            ValidateLifetime = false // we want to get claims from expired tokens as well
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if(securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token algorithm");
        }

        return principal;
    }
}
