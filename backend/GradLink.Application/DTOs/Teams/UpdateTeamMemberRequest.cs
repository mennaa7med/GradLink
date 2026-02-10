using System.ComponentModel.DataAnnotations;

namespace GradLink.Application.DTOs.Teams;

public class UpdateTeamMemberRequest
{
    // Identity Information
    [MaxLength(100)]
    public string? FullName { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    public string? ProfileImageUrl { get; set; }

    [MaxLength(200)]
    public string? University { get; set; }

    [Range(1950, 2100)]
    public int? GraduationYear { get; set; }

    // Professional Information
    [MaxLength(100)]
    public string? Role { get; set; }

    [MaxLength(500)]
    public string? Skills { get; set; }

    [MaxLength(50)]
    public string? Availability { get; set; }

    [Range(0, 5)]
    public decimal? Rating { get; set; }

    [Range(0, int.MaxValue)]
    public int? ProjectContributions { get; set; }
}

