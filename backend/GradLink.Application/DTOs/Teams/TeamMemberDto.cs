namespace GradLink.Application.DTOs.Teams;

public class TeamMemberDto
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
    public int ProjectContributions { get; set; }

    // Task Statistics
    public int TotalTasks { get; set; }
    public int TasksCompleted { get; set; }

    // System / Metadata
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Project Info
    public int ProjectId { get; set; }
    public string? ProjectTitle { get; set; }

    // User Info
    public string? UserId { get; set; }

    // Tasks
    public List<TeamMemberTaskDto>? Tasks { get; set; }
}

