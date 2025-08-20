using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UrlShortener.Models.Configuration;
using UrlShortener.Models.Entities;
using UrlShortener.Services.Auth;

namespace UrlShortener.Unit.Services.Auth;

[Trait("Category", "Services")]
[Trait("Component", "Auth")]
[Trait("SubComponent", "JwtTokenGenerator")]
public class JwtTokenGeneratorTests
{
    private const string DefaultUserRole = "User";
    private const string AdminRole = "Admin";

    private readonly JwtTokenGenerator generator;
    private readonly JwtConfig jwtConfig;

    public JwtTokenGeneratorTests()
    {
        jwtConfig = new JwtConfig
        {
            Secret = "ThisIsAVeryLongSecretKeyForTestingPurposes",
            ExpirationMinutes = 60
        };

        var options = Options.Create(jwtConfig);
        generator = new JwtTokenGenerator(options);
    }

    [Fact]
    public void Generate_WithValidUser_ReturnsValidToken()
    {
        // Arrange
        var user = new ApplicationUser
        {
            Id = "user123",
            UserName = "testuser",
            Email = "test@example.com"
        };

        // Act
        var token = generator.Generate(user, DefaultUserRole);

        // Assert
        Assert.NotNull(token);
        Assert.True(token.ValidTo > DateTime.UtcNow);
        Assert.Contains(token.Claims, c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == user.Id);
        Assert.Contains(token.Claims, c => c.Type == ClaimTypes.Name && c.Value == user.UserName);
        Assert.Contains(token.Claims, c => c.Type == ClaimTypes.Role && c.Value == DefaultUserRole);
        Assert.Contains(token.Claims, c => c.Type == ClaimTypes.Email && c.Value == user.Email);
    }

    [Fact]
    public void Generate_WithUserWithoutEmail_ExcludesEmailClaim()
    {
        // Arrange
        var user = new ApplicationUser
        {
            Id = "user123",
            UserName = "testuser",
            Email = null
        };

        // Act
        var token = generator.Generate(user, DefaultUserRole);

        // Assert
        Assert.NotNull(token);
        Assert.DoesNotContain(token.Claims, c => c.Type == ClaimTypes.Email);
    }

    [Fact]
    public void Generate_WithDifferentRoles_IncludesCorrectRole()
    {
        // Arrange
        var user = new ApplicationUser { Id = "user123", UserName = "testuser" };

        // Act
        var userToken = generator.Generate(user, DefaultUserRole);
        var adminToken = generator.Generate(user, AdminRole);

        // Assert
        Assert.Contains(userToken.Claims, c => c.Type == ClaimTypes.Role && c.Value == DefaultUserRole);
        Assert.Contains(adminToken.Claims, c => c.Type == ClaimTypes.Role && c.Value == AdminRole);
    }
}
