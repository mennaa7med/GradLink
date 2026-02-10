using System.ComponentModel.DataAnnotations;

namespace GradLink.Application.DTOs.Teams;

public class UpdateTeamMemberTaskRequest
{
    [MaxLength(200)]
    public string? Title { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(20)]
    public string? Status { get; set; } // Pending, InProgress, Completed, Cancelled

    [MaxLength(20)]
    public string? Priority { get; set; } // Low, Medium, High, Critical

    public DateTime? DueDate { get; set; }
}

