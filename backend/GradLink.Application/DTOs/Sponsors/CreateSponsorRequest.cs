using System.ComponentModel.DataAnnotations;

namespace GradLink.Application.DTOs.Sponsors;

public class CreateSponsorRequest
{
    [Required]
    [MaxLength(200)]
    public required string Name { get; set; }

    [MaxLength(100)]
    public string? Field { get; set; }

    [MaxLength(500)]
    public string? SupportedProject { get; set; }

    [Url]
    public string? Link { get; set; }

    public string? Logo { get; set; }
}

