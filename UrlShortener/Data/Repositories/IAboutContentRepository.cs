using UrlShortener.Models.Entities;

namespace UrlShortener.Data.Repositories;

public interface IAboutContentRepository
{
    Task<AboutContent?> GetLatestAsync();

    Task<AboutContent> UpdateContentAsync(string content, string? userId);
}
