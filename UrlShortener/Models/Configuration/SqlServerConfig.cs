namespace UrlShortener.Models.Configuration;

public record SqlServerConfig
{
    public string Host { get; set; } = string.Empty;

    public int Port { get; set; }

    public string DatabaseName { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
