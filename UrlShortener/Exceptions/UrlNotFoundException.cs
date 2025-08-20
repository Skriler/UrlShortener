namespace UrlShortener.Exceptions;

public class UrlNotFoundException : UrlShortenerException
{
    public override int StatusCode { get; } = StatusCodes.Status404NotFound;

    public override string ErrorTitle { get; } = "Not Found";

    public long UrlId { get; set; }

    public UrlNotFoundException(long urlId)
        : base($"URL with ID {urlId} was not found.")
    {
        UrlId = urlId;
    }
}
