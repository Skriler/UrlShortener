namespace UrlShortener.Models.Configuration;

public record JwtConfig
{
    public string Secret { get; init; } = string.Empty;

    public int ExpirationMinutes { get; init; }
}
