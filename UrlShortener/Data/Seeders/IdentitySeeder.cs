using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Data;
using System.Text.Json;
using UrlShortener.Models.Configuration;
using UrlShortener.Models.Configuration.Seeding;
using UrlShortener.Models.Entities;

namespace UrlShortener.Data.Seeders;

public class IdentitySeeder : BaseSeeder
{
    private const string UsersSeedDataFileName = "seed-users.json";

    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly IdentityConfig identityConfig;
    private readonly SeedingConfig seedingConfig;

    public IdentitySeeder(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<IdentityConfig> identityConfigOptions,
        IOptions<SeedingConfig> seedingConfigOptions,
        ILogger<IdentitySeeder> logger) : base(logger)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        identityConfig = identityConfigOptions.Value;
        seedingConfig = seedingConfigOptions.Value;
    }

    /// <summary>
    /// Performs seeding for roles, admin user, and default users.
    /// </summary>
    public override async Task SeedAsync()
    {
        try
        {
            await SeedRolesAsync();
            await SeedSystemAdminAsync();
            await SeedUsersAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error seeding identity data");
            throw;
        }
    }

    /// <summary>
    /// Seeds roles defined in configuration if they do not exist.
    /// </summary>
    private async Task SeedRolesAsync()
    {
        if (identityConfig.Roles == null || identityConfig.Roles.Count == 0)
        {
            logger.LogWarning("No roles configured for seeding.");
            return;
        }

        foreach (var role in identityConfig.Roles)
        {
            if (await roleManager.RoleExistsAsync(role))
                continue;

            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    /// <summary>
    /// Seeds the admin user defined in configuration if not already exists.
    /// </summary>
    private async Task SeedSystemAdminAsync()
    {
        var adminConfig = seedingConfig.SystemAdminUser;

        await CreateUserIfNotExists(adminConfig, identityConfig.AdminRole);
    }

    /// <summary>
    /// Seeds default users from JSON file if they do not exist.
    /// </summary>
    private async Task SeedUsersAsync()
    {
        var usersData = await ReadSeedFileAsync<List<SeedUserConfig>>(UsersSeedDataFileName);

        if (usersData == null || usersData.Count == 0)
        {
            logger.LogWarning("No users configured for seeding.");
            return;
        }

        foreach (var userData in usersData)
        {
            await CreateUserIfNotExists(userData, identityConfig.DefaultRole);
        }
    }

    /// <summary>
    /// Creates a user with given role if it does not already exist.
    /// </summary>
    private async Task CreateUserIfNotExists(SeedUserConfig config, string role)
    {
        var existingUser = await userManager.FindByNameAsync(config.Username);

        if (existingUser != null)
            return;

        var user = new ApplicationUser
        {
            UserName = config.Username,
            Email = config.Email,
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await userManager.CreateAsync(user, config.Password);

        if (!result.Succeeded)
        {
            logger.LogWarning(
                "Failed to create user: {Username}, Errors: {Errors}",
                config.Username,
                string.Join(", ", result.Errors.Select(e => e.Description)));

            return;
        }

        await userManager.AddToRoleAsync(user, role);
    }
}
