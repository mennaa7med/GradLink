namespace GradLink.Application.DTOs.Mentors;

public class MentorDto
{
    public int Id { get; set; }

    // Identity Information
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public string? JobTitle { get; set; }
    public string? ProfilePicture { get; set; }

    // Professional Information
    public string? Specialization { get; set; }
    public string? Bio { get; set; }
    public string? Skills { get; set; }
    public int? ExperienceYears { get; set; }

    // Communication
    public string? WhatsApp { get; set; }
    public string? FacebookUrl { get; set; }
    public string? LinkedInUrl { get; set; }

    // System Information
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // User link
    public string? UserId { get; set; }
}

