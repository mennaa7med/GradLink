namespace GradLink.Domain.Entities;

public class Sponsor
{
    public int Id { get; set; }

    // Basic Information
    public required string Name { get; set; }
    public string? Field { get; set; } // Software Development, Education, etc.
    public string? SupportedProject { get; set; }
    public string? Link { get; set; }
    public string? Logo { get; set; }

    // System Information
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

