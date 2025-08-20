namespace UrlShortener.Exceptions;

public class UrlAlreadyExistsException : UrlShortenerException
{
    public override int StatusCode { get; } = StatusCodes.Status409Conflict;

    public override string ErrorTitle { get; } = "Conflict";

    public string Url { get; set; }

    public UrlAlreadyExistsException(string url)
        : base($"URL '{url}' already exists.")
    {
        Url = url;
    }
}
