using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models.Entities;

public class AboutContent
{
    [Key]
    public long Id { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public string? UpdatedById { get; set; }
    public virtual ApplicationUser UpdatedBy { get; set; } = default!;
}
