using Microsoft.EntityFrameworkCore;
using UrlShortener.Models.Entities;

namespace UrlShortener.Data.Repositories;

public class ShortUrlRepository(UrlShortenerDbContext context)
{
    public async Task<bool> ShortCodeExistsAsync(string shortCode) =>
        await context.ShortenedUrls.AnyAsync(u => u.ShortCode == shortCode);

    public async Task<bool> OriginalUrlExistsAsync(string originalUrl) =>
        await context.ShortenedUrls.AnyAsync(u => u.OriginalUrl == originalUrl);

    public async Task<List<ShortUrl>> GetAllAsync()
    {
        return await context.ShortenedUrls
            .Include(u => u.CreatedBy)
            .OrderByDescending(u => u.CreatedAt)
            .ToListAsync();
    }

    public async Task<ShortUrl?> GetByIdAsync(long id)
    {
        return await context.ShortenedUrls
            .Include(u => u.CreatedBy)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<ShortUrl?> GetByShortCodeAsync(string shortCode)
    {
        return await context.ShortenedUrls
            .FirstOrDefaultAsync(u => u.ShortCode == shortCode);
    }

    public async Task<ShortUrl> AddAsync(ShortUrl shortUrl)
    {
        var entity = context.ShortenedUrls.Add(shortUrl);
        await context.SaveChangesAsync();

        await context.Entry(entity.Entity)
            .Reference(u => u.CreatedBy)
            .LoadAsync();

        return entity.Entity;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var shortUrl = await context.ShortenedUrls.FindAsync(id);
        if (shortUrl == null)
            return false;

        context.ShortenedUrls.Remove(shortUrl);
        await context.SaveChangesAsync();
        return true;
    }
}
