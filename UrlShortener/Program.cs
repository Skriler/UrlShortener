using UrlShortener.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.LoadEnvironmentVariables();
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.ConfigureApplication();

app.Run();
