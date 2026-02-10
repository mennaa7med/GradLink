namespace GradLink.Domain.Entities;

public class TeamMemberTask
{
    public int Id { get; set; }

    // Task Information
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, InProgress, Completed, Cancelled
    public string? Priority { get; set; } = "Medium"; // Low, Medium, High, Critical
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }

    // System / Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Foreign Key
    public int TeamMemberId { get; set; }
    public TeamMember? TeamMember { get; set; }
}

