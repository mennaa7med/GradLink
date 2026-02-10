using System.ComponentModel.DataAnnotations;

namespace GradLink.Application.DTOs.Projects;

public class CreateProjectRequest
{
    [Required]
    [MaxLength(200)]
    public required string Title { get; set; }

    public string? Description { get; set; }
    public string? Category { get; set; }
}

