using System.Text;
using UrlShortener.Data.Repositories;
using UrlShortener.Exceptions;

namespace UrlShortener.Services.Url.Generators;

public class ShortCodeGenerator(
    IShortUrlRepository shortUrlRepository
    ) : IShortCodeGenerator
{
    private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private const int DefaultLength = 6;
    private const int MaxAttempts = 100;

    /// <summary>
    /// Generates a unique short code by creating random strings until a unique one is found.
    /// </summary>
    public async Task<string> GenerateAsync(string originalUrl)
    {
        string shortCode;
        int attempts = 0;

        do
        {
            if (attempts >= MaxAttempts)
            {
                throw new ShortCodeGenerationException(originalUrl, attempts);
            }

            shortCode = GenerateRandomString(DefaultLength);
            ++attempts;
        }
        while (await shortUrlRepository.ShortCodeExistsAsync(shortCode));

        return shortCode;
    }

    /// <summary>
    /// Generates a random alphanumeric string of specified length.
    /// </summary>
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
