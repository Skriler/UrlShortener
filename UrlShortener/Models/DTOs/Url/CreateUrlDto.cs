using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models.DTOs.Url;

public record CreateUrlDto
{
    [Required]
    [Url]
    [MaxLength(2048)]
    public string OriginalUrl { get; set; } = string.Empty;
}
