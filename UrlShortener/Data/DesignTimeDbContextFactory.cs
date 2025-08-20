using DotNetEnv;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using UrlShortener.Models.Configuration;

namespace UrlShortener.Data;

/// <summary>
/// Factory for creating UrlShortenerDbContext instances at design time.
/// This is used by Entity Framework Tools (like migrations) and avoids DI container dependencies.
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<UrlShortenerDbContext>
{
    public UrlShortenerDbContext CreateDbContext(string[] args)
    {
        Env.Load();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var config = configuration.GetSection("SqlServer").Get<SqlServerConfig>()!;
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = $"{config.Host},{config.Port}",
            InitialCatalog = config.DatabaseName,
            UserID = config.Username,
            Password = config.Password,
            TrustServerCertificate = true
        };

        var optionsBuilder = new DbContextOptionsBuilder<UrlShortenerDbContext>();
        optionsBuilder.UseSqlServer(builder.ConnectionString);

        return new UrlShortenerDbContext(optionsBuilder.Options);
    }
}
