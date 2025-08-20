namespace UrlShortener.Models.Configuration.Seeding;

public record SeedUserConfig
{
    public string Username { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;
}
