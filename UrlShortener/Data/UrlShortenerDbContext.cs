using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Models.Entities;

namespace UrlShortener.Data;
public class UrlShortenerDbContext(
    DbContextOptions<UrlShortenerDbContext> options
    ) : IdentityDbContext(options)
{
    public DbSet<ShortUrl> ShortUrls { get; set; }

    public DbSet<AboutContent> AboutContent { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ShortUrl>()
            .HasOne(url => url.CreatedBy)
            .WithMany(user => user.ShortUrls)
            .HasForeignKey(url => url.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<AboutContent>()
            .HasOne(e => e.UpdatedBy)
            .WithMany()
            .HasForeignKey(e => e.UpdatedById)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
