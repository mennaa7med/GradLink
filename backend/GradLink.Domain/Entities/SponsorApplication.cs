namespace GradLink.Domain.Entities;

public class SponsorApplication
{
    public int Id { get; set; }

    // Applicant Information
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string CompanyName { get; set; }

    // Sponsorship Details
    public required string SponsorshipType { get; set; } // Funding, Mentorship, Resources
    public string? Message { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

    // System Information
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Optional: Link to user if applicant is registered
    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }
}

