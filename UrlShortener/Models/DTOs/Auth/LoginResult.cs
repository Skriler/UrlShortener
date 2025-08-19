namespace UrlShortener.Models.DTOs.Auth;

public record LoginResult
{
    public bool Success { get; init; }

    public string Error { get; init; } = string.Empty;

    public string Token { get; init; } = string.Empty;

    public DateTime Expiration { get; init; }

    public string Username { get; init; } = string.Empty;

    public string Role { get; init; } = string.Empty;
}
