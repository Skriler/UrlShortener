using System.Text.Json;

namespace UrlShortener.Data.Seeders;

public abstract class BaseSeeder(ILogger logger)
{
    protected readonly ILogger logger = logger;

    /// <summary>
    /// Returns the full path to a seed file relative to the application's root.
    /// </summary>
    protected static string GetSeedFilePath(string fileName) =>
        Path.Combine(Directory.GetCurrentDirectory(), "Data", "Seeders", "SeedData", fileName);

    /// <summary>
    /// Reads JSON content from a seed file and deserializes it into the specified type.
    /// </summary>
    protected async Task<T?> ReadSeedFileAsync<T>(string fileName)
    {
        var path = GetSeedFilePath(fileName);

        if (!File.Exists(path))
        {
            logger.LogWarning("Seed file not found: {FileName}", path);
            return default;
        }

        var json = await File.ReadAllTextAsync(path);
        return JsonSerializer.Deserialize<T>(json);
    }

    /// <summary>
    /// Performs the seeding logic for the derived seeder.
    /// </summary>
    public abstract Task SeedAsync();
}
