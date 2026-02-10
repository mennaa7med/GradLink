namespace GradLink.Domain.Entities;

/// <summary>
/// Sponsored Project - Links sponsors to projects they support
/// </summary>
public class SponsoredProject
{
    public int Id { get; set; }

    // Project Information (denormalized for quick access)
    public required string ProjectTitle { get; set; }
    public string? ProjectDescription { get; set; }
    public string? StudentName { get; set; }
    public string? TeamName { get; set; }
    public string? Category { get; set; } // AI, Web, Mobile, etc.

    // Sponsorship Details
    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected, Completed
    public string? RejectionReason { get; set; }
    public decimal? FundingAmount { get; set; }
    public decimal? FundingDelivered { get; set; } = 0;

    // Progress Tracking
    public int ProgressPercentage { get; set; } = 0;
    public string? CurrentMilestone { get; set; }
    public string? Milestones { get; set; } // JSON array of milestones

    // Documents
    public string? Documents { get; set; } // JSON array of document URLs

    // Timestamps
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ApprovedAt { get; set; }
    public DateTime? RejectedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Foreign Keys
    public int SponsorProfileId { get; set; }
    public SponsorProfile? SponsorProfile { get; set; }

    public int? ProjectId { get; set; }
    public Project? Project { get; set; }

    public string? StudentUserId { get; set; }
    public ApplicationUser? StudentUser { get; set; }

    // Navigation Properties
    public ICollection<SponsorFunding> Fundings { get; set; } = new List<SponsorFunding>();
    public ICollection<SponsorMessage> Messages { get; set; } = new List<SponsorMessage>();
}

