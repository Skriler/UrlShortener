using Microsoft.AspNetCore.Mvc;
using UrlShortener.Services.Url.Core;

namespace UrlShortener.Controllers;

public class RedirectController(
    IShortUrlService urlService,
    ILogger<RedirectController> logger
    ) : Controller
{
    /// <summary>
    /// Redirects to the original URL based on the short code.
    /// </summary>
    /// <param name="shortCode">The short code to redirect</param>
    /// <returns>A redirect response to the original URL</returns>
    [HttpGet("{shortCode}")]
    [ProducesResponseType(StatusCodes.Status302Found)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Index(
        [FromRoute] string shortCode)
    {
        var originalUrl = await urlService.GetOriginalUrlByShortCodeAsync(shortCode);

        if (originalUrl == null)
        {
            logger.LogWarning("Short code {ShortCode} not found", shortCode);

            return NotFound($"Short code '{shortCode}' not found");
        }

        logger.LogInformation(
            "Redirecting short code {ShortCode} to {OriginalUrl}",
            shortCode,
            originalUrl);

        return Redirect(originalUrl);
    }
}
