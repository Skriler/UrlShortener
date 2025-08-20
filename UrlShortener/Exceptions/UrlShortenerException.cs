namespace UrlShortener.Exceptions;

public abstract class UrlShortenerException : Exception
{
    public virtual int StatusCode { get; } = StatusCodes.Status500InternalServerError;

    public virtual string ErrorTitle { get; } = "Url Shortener exception";

    protected UrlShortenerException(string message, Exception? innerException = null)
        : base(message, innerException)
    { }
}
