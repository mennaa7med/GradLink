namespace GradLink.Application.DTOs.Sponsors;

public class SponsorDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Field { get; set; }
    public string? SupportedProject { get; set; }
    public string? Link { get; set; }
    public string? Logo { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

