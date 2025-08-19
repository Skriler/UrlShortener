namespace UrlShortener.Models.Configuration;

public record JwtConfig
{
    public string Secret { get; set; } = string.Empty;

    public int ExpirationMinutes { get; set; }
}
