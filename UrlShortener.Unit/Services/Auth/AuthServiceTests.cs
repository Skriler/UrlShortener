using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using UrlShortener.Models.Configuration;
using UrlShortener.Models.DTOs.Auth;
using UrlShortener.Models.Entities;
using UrlShortener.Services.Auth.Core;
using UrlShortener.Services.Auth.Generators;

namespace UrlShortener.Unit.Services.Auth;

[Trait("Category", "Services")]
[Trait("Component", "Auth")]
[Trait("SubComponent", "AuthService")]
public class AuthServiceTests
{
    private const string DefaultUserRole = "User";

    private readonly Mock<UserManager<ApplicationUser>> userManagerMock;
    private readonly Mock<IJwtTokenGenerator> jwtTokenGeneratorMock;
    private readonly AuthService authService;

    public AuthServiceTests()
    {
        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        userManagerMock = new Mock<UserManager<ApplicationUser>>(
            userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();

        var identityConfig = new IdentityConfig { DefaultRole = DefaultUserRole };
        var options = Options.Create(identityConfig);

        authService = new AuthService(userManagerMock.Object, jwtTokenGeneratorMock.Object, options);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsSuccessResult()
    {
        // Arrange
        var user = new ApplicationUser { Id = "1", UserName = "testuser" };
        var dto = new LoginDto { Username = "testuser", Password = "password" };
        var token = new JwtSecurityToken();

        userManagerMock
            .Setup(x => x.FindByNameAsync("testuser"))
            .ReturnsAsync(user);

        userManagerMock
            .Setup(x => x.CheckPasswordAsync(user, "password"))
            .ReturnsAsync(true);

        userManagerMock
            .Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { DefaultUserRole });

        jwtTokenGeneratorMock
            .Setup(x => x.Generate(user, DefaultUserRole))
            .Returns(token);

        // Act
        var result = await authService.LoginAsync(dto);

        // Assert
        Assert.True(result.Success);
        Assert.NotEmpty(result.Token);
        Assert.Equal(dto.Username, result.Username);
        Assert.Equal(DefaultUserRole, result.Role);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidCredentials_ReturnsFailureResult()
    {
        // Arrange
        var dto = new LoginDto { Username = "testuser", Password = "wrongpassword" };

        userManagerMock
            .Setup(x => x.FindByNameAsync("testuser"))
            .ReturnsAsync((ApplicationUser)null!);

        // Act
        var result = await authService.LoginAsync(dto);

        // Assert
        Assert.False(result.Success);
        Assert.NotEmpty(result.Error);
        Assert.Empty(result.Token);
    }
}
