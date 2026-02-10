using System.ComponentModel.DataAnnotations;

namespace GradLink.Application.DTOs.Mentors;

public class CreateMentorRequest
{
    // Identity Information
    [Required]
    [MaxLength(100)]
    public required string FullName { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [MaxLength(100)]
    public string? JobTitle { get; set; }

    public string? ProfilePicture { get; set; }

    // Professional Information
    [MaxLength(100)]
    public string? Specialization { get; set; }

    [MaxLength(1000)]
    public string? Bio { get; set; }

    [MaxLength(500)]
    public string? Skills { get; set; }

    [Range(0, 50)]
    public int? ExperienceYears { get; set; }

    // Communication
    [MaxLength(20)]
    public string? WhatsApp { get; set; }

    [Url]
    public string? FacebookUrl { get; set; }

    [Url]
    public string? LinkedInUrl { get; set; }
}

