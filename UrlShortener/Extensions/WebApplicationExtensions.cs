using DotNetEnv;
using UrlShortener.Data.Seeders;
using UrlShortener.Middlewares;

namespace UrlShortener.Extensions;

public static class WebApplicationExtensions
{
    /// <summary>
    /// Loads environment variables from a .env.
    /// </summary>
    public static void LoadEnvironmentVariables(this WebApplicationBuilder builder)
    {
        Env.Load();
        builder.Configuration.AddEnvironmentVariables();
    }

    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        return app
            .ConfigureMiddleware()
            .ConfigureRouting();
    }

    /// <summary>
    /// Configures the application middleware pipeline.
    /// </summary>
    private static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        ConfigureSwagger(app);

        if (!app.Environment.IsDevelopment())
        {
            app.UseHsts();
        }

        app.UseErrorHandlingMiddleware()
            .UseHttpsRedirection()
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization();

        app.MapStaticAssets();

        return app;
    }

    /// <summary>
    /// Configures the application routing.
    /// </summary>
    private static WebApplication ConfigureRouting(this WebApplication app)
    {
        app.MapControllerRoute(
            name: "redirect",
            pattern: "{shortCode}",
            defaults: new { controller = "Redirect", action = "Index" });

        app.MapControllers();
        app.MapFallbackToFile("index.html");

        return app;
    }

    /// <summary>
    /// Configures swagger.
    /// </summary>
    private static void ConfigureSwagger(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(opt =>
        {
            opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Url Shortener v1");
            opt.RoutePrefix = "swagger";
        });
    }

    /// <summary>
    /// Initializes the database with seed data.
    /// </summary>
    public static async Task<IApplicationBuilder> InitializeDatabaseAsync(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var identitySeeder = scope.ServiceProvider.GetRequiredService<IdentitySeeder>();
            var urlSeeder = scope.ServiceProvider.GetRequiredService<UrlSeeder>();

            await identitySeeder.SeedAsync();
            await urlSeeder.SeedAsync();
        }

        return app;
    }
}
