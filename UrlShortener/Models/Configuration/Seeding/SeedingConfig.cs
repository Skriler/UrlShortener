namespace UrlShortener.Models.Configuration.Seeding;

public record SeedingConfig
{
    public SeedUserConfig SystemAdminUser { get; init; } = default!;
}
