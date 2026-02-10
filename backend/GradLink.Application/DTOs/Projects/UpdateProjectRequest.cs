using System.ComponentModel.DataAnnotations;

namespace GradLink.Application.DTOs.Projects;

public class UpdateProjectRequest
{
    [MaxLength(200)]
    public string? Title { get; set; }

    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? Status { get; set; }
}

