using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UrlShortener.Models.Configuration.Seeding;
using UrlShortener.Models.Entities;
using UrlShortener.Services.Url.Generators;

namespace UrlShortener.Data.Seeders;

public class UrlSeeder : BaseSeeder
{
    private const string UrlsSeedDataFileName = "seed-urls.json";

    private readonly UrlShortenerDbContext context;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SeedingConfig seedingConfig;
    private readonly IShortCodeGenerator shortCodeGenerator;

    public UrlSeeder(
        UrlShortenerDbContext context,
        UserManager<ApplicationUser> userManager,
        IShortCodeGenerator shortCodeGenerator,
        IOptions<SeedingConfig> seedingConfigOptions,
        ILogger<UrlSeeder> logger) : base(logger)
    {
        this.context = context;
        this.userManager = userManager;
        this.shortCodeGenerator = shortCodeGenerator;
        seedingConfig = seedingConfigOptions.Value;
    }

    /// <summary>
    /// Performs all seeding operations for short URLs.
    /// </summary>
    public override async Task SeedAsync()
    {
        try
        {
            await SeedShortUrlsAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error seeding urls data");
            throw;
        }
    }

    /// <summary>
    /// Seeds short URLs from JSON file if the database table is empty.
    /// </summary>
    public async Task SeedShortUrlsAsync()
    {
        if (await context.ShortUrls.AnyAsync())
            return;

        var urls = await ReadSeedFileAsync<List<string>>(UrlsSeedDataFileName);

        if (urls == null || urls.Count == 0)
        {
            logger.LogWarning("No urls configured for seeding.");
            return;
        }

        var adminUser = await userManager.FindByNameAsync(seedingConfig.SystemAdminUser.Username);

        if (adminUser == null)
            return;

        await CreateUrls(urls, adminUser.Id);
    }

    /// <summary>
    /// Creates short urls with specified user id.
    /// </summary>
    private async Task CreateUrls(List<string> urls, string userId)
    {
        foreach (var url in urls)
        {
            var shortCode = await shortCodeGenerator.GenerateAsync(url);

            var shortUrl = new ShortUrl
            {
                OriginalUrl = url,
                ShortCode = shortCode,
                CreatedAt = DateTime.UtcNow,
                CreatedById = userId
            };

            context.ShortUrls.Add(shortUrl);
        }

        await context.SaveChangesAsync();
    }
}
