namespace UrlShortener.Models.DTOs.Url;

public record ShortUrlDetailsDto
{
    public long Id { get; init; }

    public string OriginalUrl { get; init; } = string.Empty;

    public string ShortUrl { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; }

    public string CreatedBy { get; init; } = string.Empty;

    public string CreatedById { get; init; } = string.Empty;
}
