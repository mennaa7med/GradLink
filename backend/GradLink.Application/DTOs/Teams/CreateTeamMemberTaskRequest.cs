using System.ComponentModel.DataAnnotations;

namespace GradLink.Application.DTOs.Teams;

public class CreateTeamMemberTaskRequest
{
    [Required]
    [MaxLength(200)]
    public required string Title { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(20)]
    public string? Priority { get; set; } = "Medium"; // Low, Medium, High, Critical

    public DateTime? DueDate { get; set; }

    [Required]
    public int TeamMemberId { get; set; }
}

