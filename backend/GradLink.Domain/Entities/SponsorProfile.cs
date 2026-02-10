namespace GradLink.Domain.Entities;

/// <summary>
/// Sponsor Profile - Contains detailed information about sponsors (companies/individuals)
/// </summary>
public class SponsorProfile
{
    public int Id { get; set; }

    // Company/Sponsor Information
    public required string CompanyName { get; set; }
    public string? CompanyDescription { get; set; }
    public string? Logo { get; set; }
    public string? Website { get; set; }
    public string? Industry { get; set; } // Software, Education, Finance, etc.

    // Contact Information
    public required string ContactPersonName { get; set; }
    public required string ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
    public string? Address { get; set; }

    // Sponsorship Preferences
    public string? FieldsOfInterest { get; set; } // AI, Web Development, Mobile, etc. (comma-separated)
    public decimal? TotalBudget { get; set; }
    public decimal? RemainingBudget { get; set; }

    // Statistics (computed)
    public int TotalProjectsSponsored { get; set; } = 0;
    public int ActiveProjects { get; set; } = 0;
    public int ApprovedProjects { get; set; } = 0;
    public int CompletedProjects { get; set; } = 0;
    public int PendingRequests { get; set; } = 0;

    // System Information
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsVerified { get; set; } = false;

    // Link to User Account
    public required string UserId { get; set; }
    public ApplicationUser? User { get; set; }

    // Navigation Properties
    public ICollection<SponsoredProject> SponsoredProjects { get; set; } = new List<SponsoredProject>();
    public ICollection<SponsorFunding> Fundings { get; set; } = new List<SponsorFunding>();
    public ICollection<SponsorMessage> Messages { get; set; } = new List<SponsorMessage>();
}

