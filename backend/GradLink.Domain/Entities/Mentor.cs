namespace GradLink.Domain.Entities;

public class Mentor
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
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Optional: Link to ApplicationUser if the mentor is a registered user
    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }
}

