using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models.Entities;

public class ShortUrl
{
    [Key]
    public long Id { get; set; }

    [Required]
    [MaxLength(2048)]
    public string OriginalUrl { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string ShortCode { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    [Required]
    public string CreatedById { get; set; } = string.Empty;
    public ApplicationUser CreatedBy { get; set; } = default!;
}
