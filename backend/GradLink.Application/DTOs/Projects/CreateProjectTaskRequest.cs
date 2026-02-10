using System.ComponentModel.DataAnnotations;

namespace GradLink.Application.DTOs.Projects;

public class CreateProjectTaskRequest
{
    [Required]
    public required string Title { get; set; }

    public string? Description { get; set; }
    public string? Priority { get; set; }
    public DateTime? DueDate { get; set; }
}

