using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UrlShortener.Models.Configuration;
using UrlShortener.Models.Entities;
using UrlShortener.Services.Auth.Generators;

namespace UrlShortener.Services.Auth;

public class JwtTokenGenerator(
    IOptions<JwtConfig> jwtOptions
    ) : IJwtTokenGenerator
{
    private readonly JwtConfig jwtConfig = jwtOptions.Value;

    /// <summary>
    /// Generates a JWT token containing user information.
    /// </summary>
    public JwtSecurityToken Generate(ApplicationUser user, string role)
    {
        var authClaims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Role, role),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (!string.IsNullOrEmpty(user.Email))
        {
            authClaims.Add(new Claim(ClaimTypes.Email, user.Email));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret));

        return new JwtSecurityToken(
            claims: authClaims,
            expires: DateTime.UtcNow.AddMinutes(jwtConfig.ExpirationMinutes),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );
    }
}
