using System;
using UrlShortener.Data.Repositories;
using UrlShortener.Models.DTOs.Url;
using UrlShortener.Models.Entities;
using UrlShortener.Services.Url.Generators;

namespace UrlShortener.Services.Url.Core;

public class ShortUrlService(
    ShortUrlRepository shortUrlRepository,
    IShortCodeGenerator shortCodeGenerator
    ) : IShortUrlService
{
    /// <summary>
    /// Retrieves all short URLs from the system ordered by creation date descending.
    /// </summary>
    public async Task<List<ShortUrlDto>> GetAllAsync()
    {
        var urls = await shortUrlRepository.GetAllAsync();

        return urls.ConvertAll(url => new ShortUrlDto
        {
            Id = url.Id,
            OriginalUrl = url.OriginalUrl,
            ShortCode = url.ShortCode,
            CreatedBy = url.CreatedBy.Id ?? string.Empty,
        });
    }

    /// <summary>
    /// Retrieves detailed information about a specific short URL.
    /// </summary>
    public async Task<ShortUrlDetailsDto?> GetDetailsAsync(long id)
    {
        var url = await shortUrlRepository.GetByIdAsync(id);
        if (url == null)
            return null;

        return new ShortUrlDetailsDto
        {
            Id = url.Id,
            OriginalUrl = url.OriginalUrl,
            ShortUrl = url.ShortCode,
            CreatedAt = url.CreatedAt,
            CreatedBy = url.CreatedBy.UserName ?? string.Empty,
            CreatedById = url.CreatedById
        };
    }

    /// <summary>
    /// Creates a new short URL from the provided original URL, ensuring uniqueness and generating a short code.
    /// </summary>
    public async Task<ShortUrlDto> CreateAsync(CreateUrlDto dto, string userId)
    {
        if (await shortUrlRepository.OriginalUrlExistsAsync(dto.OriginalUrl))
        {
            throw new InvalidOperationException("URL already exists");
        }

        var shortCode = await shortCodeGenerator.GenerateAsync(dto.OriginalUrl);

        var shortUrl = new ShortUrl
        {
            OriginalUrl = dto.OriginalUrl,
            ShortCode = shortCode,
            CreatedAt = DateTime.UtcNow,
            CreatedById = userId
        };

        var savedUrl = await shortUrlRepository.AddAsync(shortUrl);

        return new ShortUrlDto
        {
            Id = savedUrl.Id,
            OriginalUrl = savedUrl.OriginalUrl,
            ShortCode = savedUrl.ShortCode,
            CreatedBy = savedUrl.CreatedBy.UserName ?? string.Empty,
        };
    }

    /// <summary>
    /// Deletes a short URL with authorization validation.
    /// </summary>
    public async Task<bool> DeleteAsync(
        long id,
        string userId,
        bool isAdmin)
    {
        var url = await shortUrlRepository.GetByIdAsync(id);

        if (url == null)
            return false;

        if (!isAdmin && url.CreatedById != userId)
            return false;

        await shortUrlRepository.DeleteAsync(url);
        return true;
    }
}
