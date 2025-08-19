using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UrlShortener.Data;
using UrlShortener.Models.Configuration;
using UrlShortener.Models.Entities;
using UrlShortener.Services.Auth;
using UrlShortener.Services.Auth.Core;
using UrlShortener.Services.Auth.Generators;

namespace UrlShortener.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures all services for the application.
    /// </summary>
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .ConfigureDatabase(configuration)
            .ConfigureSecurityServices(configuration);


        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
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
    /// Configures all security services including Identity and JWT authentication
    /// </summary>
    public static IServiceCollection ConfigureSecurityServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .ConfigureIdentityServices(configuration)
            .ConfigureJwtAuthentication(configuration);

        return services;
    }

    /// <summary>
    /// Configures ASP.NET Core Identity
    /// </summary>
    private static IServiceCollection ConfigureIdentityServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var identityConfig = configuration.GetSection("Identity").Get<IdentityConfig>()!;

        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequiredLength = identityConfig.PasswordRequiredLength;
            options.Password.RequireDigit = identityConfig.PasswordRequireDigit;
            options.SignIn.RequireConfirmedAccount = identityConfig.SignInRequireConfirmedAccount;
        })
        .AddEntityFrameworkStores<UrlShortenerDbContext>()
        .AddDefaultTokenProviders();

        return services;
    }

    /// <summary>
    /// Configures JWT Bearer authentication
    /// </summary>
    private static IServiceCollection ConfigureJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtConfig>(configuration.GetSection("JwtConfig"));
        var jwtConfig = configuration.GetSection("JwtConfig").Get<JwtConfig>()!;

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtConfig.Secret))
            };
        });

        return services;
    }
}
