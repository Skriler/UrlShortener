using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using UrlShortener.Data.Repositories;
using UrlShortener.Models.Configuration;

namespace UrlShortener.Pages.About;

public class IndexModel(
    IAboutContentRepository aboutContentRepository,
    IOptions<IdentityConfig> identityConfigOptions
    ) : PageModel
{
    private readonly IdentityConfig identityConfig = identityConfigOptions.Value;

    [BindProperty]
    public string AboutContent { get; set; } = string.Empty;

    public bool IsAdmin { get; private set; }

    /// <summary>
    /// Handles GET requests to load the about page content and check admin privileges.
    /// </summary>
    public async Task<IActionResult> OnGetAsync()
    {
        await LoadContentAsync();
        SetAdminStatus();

        return Page();
    }

    /// <summary>
    /// Handles POST requests to update about content. Only accessible by administrators.
    /// </summary>
    public async Task<IActionResult> OnPostAsync()
    {
        SetAdminStatus();

        if (!IsAdmin)
        {
            return Forbid();
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        await aboutContentRepository.UpdateContentAsync(AboutContent, userId);

        TempData["SuccessMessage"] = "Content updated successfully.";
        return RedirectToPage();
    }

    /// <summary>
    /// Loads the latest about content from the db.
    /// Sets AboutContent to empty string if no content exists.
    /// </summary>
    private async Task LoadContentAsync()
    {
        var aboutContent = await aboutContentRepository.GetLatestAsync();
        AboutContent = aboutContent?.Content ?? GetDefaultAboutContent();
    }

    /// <summary>
    /// Determines if the current user has administrator privileges.
    /// Sets the IsAdmin property based on authentication status and role membership.
    /// </summary>
    private void SetAdminStatus()
    {
        IsAdmin = User.Identity?.IsAuthenticated is true &&
            User.IsInRole(identityConfig.AdminRole);
    }

    private static string GetDefaultAboutContent() =>
        "Used algorithm: Generates a unique short code by repeatedly creating random alphanumeric strings" +
        " until one is found that does not already exist in the db," +
        " with a maximum number of attempts to prevent infinite loops.";
}
