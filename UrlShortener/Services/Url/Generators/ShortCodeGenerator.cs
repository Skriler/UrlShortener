using System.Text;
using UrlShortener.Data.Repositories;

namespace UrlShortener.Services.Url.Generators;

public class ShortCodeGenerator(
    ShortUrlRepository shortUrlRepository
    ) : IShortCodeGenerator
{
    private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private const int DefaultLength = 6;

    public async Task<string> GenerateAsync(string OriginalUrl)
    {
        while (true)
        {
            var shortCode = GenerateRandomString(DefaultLength);

            if (!await shortUrlRepository.ShortCodeExistsAsync(shortCode))
            {
                return shortCode;
            }
        }
    }

    private static string GenerateRandomString(int length)
    {
        var random = new Random();
        var result = new StringBuilder(length);

        for (int i = 0; i < length; ++i)
        {
            result.Append(Characters[random.Next(Characters.Length)]);
        }

        return result.ToString();
    }
}
