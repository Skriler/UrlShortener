using Microsoft.EntityFrameworkCore;
using UrlShortener.Models.Entities;

namespace UrlShortener.Data.Repositories;

public class ShortUrlRepository(UrlShortenerDbContext context)
{
    /// <summary>
    /// Checks if a short code already exists in the db.
    /// </summary>
    public async Task<bool> ShortCodeExistsAsync(string shortCode) =>
        await context.ShortUrls.AnyAsync(u => u.ShortCode == shortCode);

    /// <summary>
    /// Checks if an original URL already exists in the db.
    /// </summary>
    public async Task<bool> OriginalUrlExistsAsync(string originalUrl) =>
        await context.ShortUrls.AnyAsync(u => u.OriginalUrl == originalUrl);

    /// <summary>
    /// Retrieves all short URLs from the db including creator info,
    /// ordered by creation date descending.
    /// </summary>
    public async Task<List<ShortUrl>> GetAllAsync()
    {
        return await context.ShortUrls
            .Include(u => u.CreatedBy)
            .OrderByDescending(u => u.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves a specific short URL by its id including creator info.
    /// </summary>
    public async Task<ShortUrl?> GetByIdAsync(long id)
    {
        return await context.ShortUrls
            .Include(u => u.CreatedBy)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    /// <summary>
    /// Retrieves a short URL by its short code.
    /// </summary>
    public async Task<ShortUrl?> GetByShortCodeAsync(string shortCode)
    {
        return await context.ShortUrls
            .FirstOrDefaultAsync(u => u.ShortCode == shortCode);
    }

    /// <summary>
    /// Adds a new short URL to the db and loads the creator info.
    /// </summary>
    public async Task<ShortUrl> AddAsync(ShortUrl shortUrl)
    {
        var entity = context.ShortUrls.Add(shortUrl);
        await context.SaveChangesAsync();

        await context.Entry(entity.Entity)
            .Reference(u => u.CreatedBy)
            .LoadAsync();

        return entity.Entity;
    }

    /// <summary>
    /// Deletes a short URL from the db by entity.
    /// </summary>
    public async Task DeleteAsync(ShortUrl shortUrl)
    {
        context.ShortUrls.Remove(shortUrl);
        await context.SaveChangesAsync();
    }
}
