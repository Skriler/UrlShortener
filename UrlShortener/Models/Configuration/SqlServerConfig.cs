namespace UrlShortener.Models.Configuration;

public record SqlServerConfig
{
    public string Host { get; init; } = string.Empty;

    public int Port { get; init; }

    public string DatabaseName { get; init; } = string.Empty;

    public string Username { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;
}
