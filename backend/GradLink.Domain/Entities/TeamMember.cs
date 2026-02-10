namespace GradLink.Domain.Entities;

public class TeamMember
{
    public int Id { get; set; }

    // Identity Information
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? University { get; set; }
    public int? GraduationYear { get; set; }

    // Professional Information
    public required string Role { get; set; }
    public string? Skills { get; set; }
    public string? Availability { get; set; }
    public decimal? Rating { get; set; }
    public int ProjectContributions { get; set; } = 0;

    // Task Statistics (computed from TeamMemberTasks)
    public int TotalTasks { get; set; } = 0;
    public int TasksCompleted { get; set; } = 0;

    // System / Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Foreign Keys
    public int ProjectId { get; set; }
    public Project? Project { get; set; }

    // Optional: Link to ApplicationUser if the member is a registered user
    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }

    // Navigation properties
    public ICollection<TeamMemberTask> Tasks { get; set; } = new List<TeamMemberTask>();
}

