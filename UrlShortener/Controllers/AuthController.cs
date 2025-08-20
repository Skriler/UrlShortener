using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrlShortener.Models.DTOs.Auth;
using UrlShortener.Services.Auth.Core;

namespace UrlShortener.Controllers;

[ApiController]
[Route("api/auth")]
[Produces("application/json")]
public class AuthController(
    IAuthService authService,
    ILogger<AuthController> logger
    ) : ControllerBase
{
    /// <summary>
    /// Logs in a user with the provided credentials.
    /// </summary>
    /// <param name="dto">The login data</param>
    /// <returns>An action result containing the login result or an error message</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<LoginResult>> Login(
        [FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var loginResult = await authService.LoginAsync(dto);

        if (!loginResult.Success)
        {
            logger.LogWarning("Login failed for user {Username}: {Error}", dto.Username, loginResult.Error);
            return Unauthorized(loginResult.Error);
        }

        logger.LogInformation("User {Username} successfully logged in", dto.Username);
        return Ok(loginResult);
    }

    /// <summary>
    /// Gets the current user info.
    /// </summary>
    /// <returns>An action result containing сurrent user details</returns>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(UserInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<UserInfoDto> GetCurrentUser()
    {
        var userInfo = new UserInfoDto
        {
            UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty,
            Username = User.Identity?.Name ?? string.Empty,
            Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
        };

        logger.LogDebug("Retrieved current user info for {Username}", userInfo.Username);
        return Ok(userInfo);
    }

    /// <summary>
    /// Logs out the current user.
    /// </summary>
    /// <returns>An action result indicating successful logout</returns>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<string> Logout()
    {
        var username = User.Identity?.Name;
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        logger.LogInformation(
            "User {Username} (ID: {UserId}) successfully logged out",
            username ?? "Unknown",
            userId ?? "Unknown");
        return Ok("Logged out successfully");
    }
}
