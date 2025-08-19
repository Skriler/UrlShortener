using UrlShortener.Data.Repositories;
using UrlShortener.Models.DTOs.Url;
using UrlShortener.Models.Entities;
using UrlShortener.Services.Url.Generators;

namespace UrlShortener.Services.Url.Core;

public class UrlService(
    ShortUrlRepository shortUrlRepository,
    IShortCodeGenerator shortCodeGenerator)
{
    public async Task<ShortUrlDto> CreateShortUrlAsync(CreateUrlDto dto, string userId)
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

    public async Task<List<ShortUrlDto>> GetAllUrlsAsync()
    {
        var urls = await shortUrlRepository.GetAllAsync();

        return urls.ConvertAll(url => new ShortUrlDto
        {
            Id = url.Id,
            OriginalUrl = url.OriginalUrl,
            ShortCode = url.ShortCode,
            CreatedBy = url.CreatedBy.UserName ?? string.Empty,
        });
    }

    public async Task<ShortUrlDetailsDto?> GetUrlDetailsAsync(long id)
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
}
