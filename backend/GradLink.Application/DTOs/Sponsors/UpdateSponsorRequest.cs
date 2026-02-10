using System.ComponentModel.DataAnnotations;

namespace GradLink.Application.DTOs.Sponsors;

public class UpdateSponsorRequest
{
    [MaxLength(200)]
    public string? Name { get; set; }

    [MaxLength(100)]
    public string? Field { get; set; }

    [MaxLength(500)]
    public string? SupportedProject { get; set; }

    [Url]
    public string? Link { get; set; }

    public string? Logo { get; set; }
}

