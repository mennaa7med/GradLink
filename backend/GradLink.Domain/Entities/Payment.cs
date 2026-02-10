namespace GradLink.Domain.Entities;

/// <summary>
/// Represents a payment transaction
/// </summary>
public class Payment
{
    public int Id { get; set; }
    public string PaymentIntentId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "usd";
    public string Status { get; set; } = "pending"; // pending, processing, succeeded, failed, cancelled
    public string? Description { get; set; }
    public int? SponsorshipId { get; set; }
    public int? ProjectId { get; set; }
    public string PayerId { get; set; } = string.Empty;
    public string? ReceiptUrl { get; set; }
    public string? FailureReason { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    public virtual SponsoredProject? Sponsorship { get; set; }
    public virtual Project? Project { get; set; }
    public virtual ApplicationUser? Payer { get; set; }
}















