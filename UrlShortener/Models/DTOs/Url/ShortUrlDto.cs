namespace UrlShortener.Models.DTOs.Url;

public record ShortUrlDto
{
    public long Id { get; init; }

    public string OriginalUrl { get; init; } = string.Empty;

    public string ShortCode { get; init; } = string.Empty;

    public string CreatedBy { get; init; } = string.Empty;
}
