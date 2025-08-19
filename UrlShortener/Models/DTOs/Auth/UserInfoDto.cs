namespace UrlShortener.Models.DTOs.Auth;

public record UserInfoDto
{
    public string UserId { get; init; } = string.Empty;

    public string Username { get; init; } = string.Empty;

    public string Role { get; init; } = string.Empty;
}
