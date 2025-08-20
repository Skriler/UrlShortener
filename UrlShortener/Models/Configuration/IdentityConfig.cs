namespace UrlShortener.Models.Configuration;

public record IdentityConfig
{
    public bool PasswordRequireDigit { get; init; }

    public int PasswordRequiredLength { get; init; }

    public bool SignInRequireConfirmedAccount { get; init; }

    public List<string> Roles { get; init; } = default!;

    public string AdminRole { get; init; } = string.Empty;

    public string DefaultRole { get; init; } = string.Empty;
}
