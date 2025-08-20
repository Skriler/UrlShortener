using UrlShortener.Models.Entities;

namespace UrlShortener.Data.Repositories;

public interface IShortUrlRepository
{
    Task<bool> ShortCodeExistsAsync(string shortCode);

    Task<bool> OriginalUrlExistsAsync(string originalUrl);

    Task<List<ShortUrl>> GetAllAsync();

    Task<ShortUrl?> GetByIdAsync(long id);

    Task<ShortUrl?> GetByShortCodeAsync(string shortCode);

    Task<ShortUrl> AddAsync(ShortUrl shortUrl);

    Task DeleteAsync(ShortUrl shortUrl);
}
