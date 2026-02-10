namespace GradLink.Domain.Entities;

/// <summary>
/// Sponsor Funding - Tracks financial transactions between sponsors and projects
/// </summary>
public class SponsorFunding
{
    public int Id { get; set; }

    // Transaction Details
    public decimal Amount { get; set; }
    public string TransactionType { get; set; } = "Payment"; // Payment, Refund, Adjustment
    public string Status { get; set; } = "Pending"; // Pending, Completed, Failed, Cancelled
    public string? TransactionReference { get; set; }
    public string? Notes { get; set; }

    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    // Foreign Keys
    public int SponsorProfileId { get; set; }
    public SponsorProfile? SponsorProfile { get; set; }

    public int SponsoredProjectId { get; set; }
    public SponsoredProject? SponsoredProject { get; set; }
}

