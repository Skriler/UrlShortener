namespace UrlShortener.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        return app
            .ConfigureMiddleware()
            .ConfigureRouting();
    }

    private static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapStaticAssets();

        return app;
    }

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
}
