namespace GradLink.Domain.Entities;

public class Project
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? Status { get; set; } = "Active";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Foreign key
    public required string OwnerId { get; set; }
    public ApplicationUser? Owner { get; set; }

    // Navigation properties
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    public ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
}

