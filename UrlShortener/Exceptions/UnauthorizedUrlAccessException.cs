namespace UrlShortener.Exceptions;

public class UnauthorizedUrlAccessException : UrlShortenerException
{
    public override int StatusCode { get; } = StatusCodes.Status403Forbidden;

    public override string ErrorTitle { get; } = "Forbidden";

    public long UrlId { get; set; }

    public UnauthorizedUrlAccessException(long urlId)
        : base($"Access to URL with ID {urlId} is forbidden.")
    {
        UrlId = urlId;
    }
}
