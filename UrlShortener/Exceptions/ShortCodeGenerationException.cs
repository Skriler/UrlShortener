namespace UrlShortener.Exceptions;

public class ShortCodeGenerationException : UrlShortenerException
{
    public override int StatusCode { get; } = StatusCodes.Status500InternalServerError;

    public override string ErrorTitle { get; } = "Short code generation failed";

    public string OriginalUrl { get; set; }

    public int Attempts { get; set; }

    public ShortCodeGenerationException(string originalUrl, int attempts)
        : base($"Failed to generate a unique short code for URL '{originalUrl}' after {attempts} attempts.")
    {
        OriginalUrl = originalUrl;
        Attempts = attempts;
    }
}
