using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UrlShortener.Models.Configuration;
using UrlShortener.Models.Entities;

namespace UrlShortener.Data.Repositories;

public class AboutContentRepository(
    UrlShortenerDbContext context
    ) : IAboutContentRepository
{
    /// <summary>
    /// Retrieves the most recently created about content entry from the database,
    /// including the updater info. Returns null if no entries exist.
    /// </summary>
    public async Task<AboutContent?> GetLatestAsync()
    {
        return await context.AboutContent
            .Include(a => a.UpdatedBy)
            .OrderByDescending(a => a.CreatedAt)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Adds a new about content to the db and loads the updater info.
    /// </summary>
    public async Task<AboutContent> UpdateContentAsync(string content, string? userId)
    {
        var aboutContent = new AboutContent
        {
            Content = content,
            CreatedAt = DateTime.UtcNow,
            UpdatedById = userId
        };

        context.AboutContent.Add(aboutContent);
        await context.SaveChangesAsync();

        await context.Entry(aboutContent)
            .Reference(u => u.UpdatedBy)
            .LoadAsync();

        return aboutContent;
    }
}
