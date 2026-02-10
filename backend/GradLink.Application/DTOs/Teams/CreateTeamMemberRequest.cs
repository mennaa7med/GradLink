using System.ComponentModel.DataAnnotations;

namespace GradLink.Application.DTOs.Teams;

public class CreateTeamMemberRequest
{
    // Identity Information
    [Required]
    [MaxLength(100)]
    public required string FullName { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    public string? ProfileImageUrl { get; set; }

    [MaxLength(200)]
    public string? University { get; set; }

    [Range(1950, 2100)]
    public int? GraduationYear { get; set; }

    // Professional Information
    [Required]
    [MaxLength(100)]
    public required string Role { get; set; }

    [MaxLength(500)]
    public string? Skills { get; set; }

    [MaxLength(50)]
    public string? Availability { get; set; }

    // Project Link
    [Required]
    public int ProjectId { get; set; }
}

