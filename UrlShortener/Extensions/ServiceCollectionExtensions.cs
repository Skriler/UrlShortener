using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Models.Configuration;
using UrlShortener.Models.Entities;

namespace UrlShortener.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures all services for the application.
    /// </summary>
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .ConfigureDatabase(configuration)
            .ConfigureIdentityServices(configuration);
    }

    /// <summary>
    /// Configures and registers the DbContext for SqlServer based on configuration.
    /// </summary>
    private static IServiceCollection ConfigureDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var config = configuration.GetSection("SqlServer").Get<SqlServerConfig>()!;
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = $"{config.Host},{config.Port}",
            InitialCatalog = config.DatabaseName,
            UserID = config.Username,
            Password = config.Password,
            TrustServerCertificate = true
        };

        services.AddDbContext<UrlShortenerDbContext>(options => options.UseSqlServer(builder.ConnectionString));

        return services;
    }

    /// <summary>
    /// Configures identity services based on configuration.
    /// </summary>
    private static IServiceCollection ConfigureIdentityServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var config = configuration.GetSection("Identity").Get<IdentityConfig>()!;

        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequiredLength = config.PasswordRequiredLength;
            options.Password.RequireDigit = config.PasswordRequireDigit;
            options.SignIn.RequireConfirmedAccount = config.SignInRequireConfirmedAccount;
        })
            .AddEntityFrameworkStores<UrlShortenerDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}
