namespace GradLink.Domain.Entities;

/// <summary>
/// Sponsor Message - Communication between sponsors and students
/// </summary>
public class SponsorMessage
{
    public int Id { get; set; }

    // Message Content
    public required string Content { get; set; }
    public string? Attachments { get; set; } // JSON array of attachment URLs
    public bool IsRead { get; set; } = false;

    // Sender/Receiver
    public required string SenderId { get; set; }
    public ApplicationUser? Sender { get; set; }
    public required string ReceiverId { get; set; }
    public ApplicationUser? Receiver { get; set; }
    public bool IsSponsorSender { get; set; } // true if sponsor sent, false if student sent

    // Timestamps
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReadAt { get; set; }

    // Foreign Keys
    public int SponsorProfileId { get; set; }
    public SponsorProfile? SponsorProfile { get; set; }

    public int? SponsoredProjectId { get; set; }
    public SponsoredProject? SponsoredProject { get; set; }
}

