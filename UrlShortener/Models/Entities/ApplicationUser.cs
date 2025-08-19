using Microsoft.AspNetCore.Identity;

namespace UrlShortener.Models.Entities;

public class ApplicationUser : IdentityUser
{
    public DateTime CreatedAt { get; set; }

    public List<ShortUrl> ShortUrls { get; set; } = [];
}
