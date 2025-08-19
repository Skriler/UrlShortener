using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models.Configuration;

public record IdentityConfig
{
    public bool PasswordRequireDigit { get; set; }

    public int PasswordRequiredLength { get; set; }

    public bool SignInRequireConfirmedAccount { get; set; }
}
