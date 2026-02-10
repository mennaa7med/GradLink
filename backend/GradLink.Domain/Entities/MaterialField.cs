namespace GradLink.Domain.Entities;

public class MaterialField
{
    public int Id { get; set; }

    public required string Name { get; set; }

    // System Information
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

