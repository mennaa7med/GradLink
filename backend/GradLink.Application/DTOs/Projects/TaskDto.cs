namespace GradLink.Application.DTOs.Projects;

public class TaskDto
{
    public int Id { get; set; }
    public required string Title { get; set; } // Maps to Name in TaskItem
    public string? Description { get; set; }
    public string Status { get; set; } = "Pending";
    public string? Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int? ProjectId { get; set; }
}
