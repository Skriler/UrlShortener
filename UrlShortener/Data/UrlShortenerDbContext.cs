using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Data;
public class UrlShortenerDbContext : IdentityDbContext
{
    public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options)
        : base(options)
    {
    }
}
