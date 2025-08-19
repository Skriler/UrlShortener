using System.IdentityModel.Tokens.Jwt;
using UrlShortener.Models.Entities;

namespace UrlShortener.Services.Auth.Generators;

public interface IJwtTokenGenerator
{
    JwtSecurityToken Generate(ApplicationUser user, string role);
}
