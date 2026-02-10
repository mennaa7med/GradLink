namespace GradLink.Application.DTOs.Projects;

public class ProjectDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public required string OwnerId { get; set; }
    public string? OwnerName { get; set; }
}

