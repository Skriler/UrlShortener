using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using UrlShortener.Models.Configuration;
using UrlShortener.Models.DTOs.Auth;
using UrlShortener.Models.Entities;
using UrlShortener.Services.Auth.Generators;

namespace UrlShortener.Services.Auth.Core;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    IJwtTokenGenerator jwtTokenGenerator,
    IOptions<IdentityConfig> identityConfigOptions
    ) : IAuthService
{
    private readonly UserManager<ApplicationUser> userManager = userManager;
    private readonly IJwtTokenGenerator jwtTokenGenerator = jwtTokenGenerator;
    private readonly IdentityConfig identityConfig = identityConfigOptions.Value;

    /// <summary>
    /// Authenticates the user by validating their credentials
    /// and generating a JWT token if successful.
    /// </summary>
    public async Task<LoginResult> LoginAsync(LoginDto dto)
    {
        var user = await userManager.FindByNameAsync(dto.Username);

        if (user == null || !await userManager.CheckPasswordAsync(user, dto.Password))
        {
            return CreateFailureResult("Invalid username or password.");
        }

        var userRoles = await userManager.GetRolesAsync(user);
        var userRole = userRoles.FirstOrDefault() ?? identityConfig.DefaultRole;

        var token = jwtTokenGenerator.Generate(user, userRole);

        return CreateSuccessResult(token, user, userRole);
    }

    /// <summary>
    /// Creates a failed login result.
    /// </summary>
    private static LoginResult CreateFailureResult(string error)
    {
        return new LoginResult
        {
            Success = false,
            Error = error,
        };
    }

    /// <summary>
    /// Creates a successful login result.
    /// </summary>
    private static LoginResult CreateSuccessResult(
        JwtSecurityToken token,
        ApplicationUser user,
        string role)
    {
        return new LoginResult
        {
            Success = true,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo,
            Username = user.UserName!,
            Role = role,
        };
    }
}
