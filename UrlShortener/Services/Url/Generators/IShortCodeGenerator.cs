namespace UrlShortener.Services.Url.Generators;

public interface IShortCodeGenerator
{
    Task<string> GenerateAsync(string OriginalUrl);
}
