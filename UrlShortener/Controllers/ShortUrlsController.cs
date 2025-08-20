using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using UrlShortener.Models.Configuration;
using UrlShortener.Models.DTOs.Url;
using UrlShortener.Services.Url.Core;

namespace UrlShortener.Controllers;

[ApiController]
[Route("api/shorturls")]
[Produces("application/json")]
public class ShortUrlsController(
    IShortUrlService urlService,
    ILogger<ShortUrlsController> logger,
    IOptions<IdentityConfig> identityConfigOptions
    ) : ControllerBase
{
    private readonly IdentityConfig identityConfig = identityConfigOptions.Value;

    /// <summary>
    /// Gets all short URLs.
    /// </summary>
    /// <returns>An action result containing the list of short URLs</returns>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<ShortUrlDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ShortUrlDto>>> GetAllUrls()
    {
        var urls = await urlService.GetAllAsync();

        return Ok(urls);
    }

    /// <summary>
    /// Gets detailed information about a specific short URL.
    /// </summary>
    /// <param name="id">The ID of the short URL</param>
    /// <returns>An action result containing the short URL details</returns>
    [HttpGet("{id:long}")]
    [Authorize]
    [ProducesResponseType(typeof(ShortUrlDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ShortUrlDetailsDto>> GetUrlDetails(
        [FromRoute] long id)
    {
        var urlDetails = await urlService.GetDetailsAsync(id);

        if (urlDetails == null)
        {
            return NotFound($"Url with ID {id} not found");
        }

        return Ok(urlDetails);
    }

    /// <summary>
    /// Creates a new short URL.
    /// </summary>
    /// <param name="dto">The URL creation data</param>
    /// <returns>An action result containing the created short URL details</returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ShortUrlDetailsDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ShortUrlDetailsDto>> CreateShortUrl(
        [FromBody] CreateUrlDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await urlService.CreateAsync(dto, userId);

        logger.LogInformation(
            "User with ID {UserId} successfully created short URL with ID {UrlId}",
            userId,
            result.Id);

        return CreatedAtAction(
            nameof(GetUrlDetails),
            new { id = result.Id },
            result);
    }

    /// <summary>
    /// Deletes a short URL.
    /// </summary>
    /// <param name="id">The ID of the short URL to delete</param>
    /// <returns>An action result indicating successful deletion</returns>
    [HttpDelete("{id:long}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteUrl(
        [FromRoute] long id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var isAdmin = User.IsInRole(identityConfig.AdminRole);

        await urlService.DeleteAsync(id, userId, isAdmin);

        logger.LogInformation(
            "Short url with ID {UrlId} successfully deleted by user with ID {UserId}",
            id,
            userId);

        return NoContent();
    }
}
