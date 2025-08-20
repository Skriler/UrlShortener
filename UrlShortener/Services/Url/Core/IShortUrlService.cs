using UrlShortener.Models.DTOs.Url;

namespace UrlShortener.Services.Url.Core;

public interface IShortUrlService
{
    Task<List<ShortUrlDto>> GetAllAsync();

    Task<ShortUrlDetailsDto?> GetDetailsAsync(long id);

    Task<ShortUrlDto> CreateAsync(CreateUrlDto dto, string userId);

    Task DeleteAsync(long id, string userId, bool isAdmin);
}
