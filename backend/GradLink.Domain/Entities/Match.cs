namespace GradLink.Domain.Entities;

public class Match
{
    public int Id { get; set; }
    public double Score { get; set; }
    public string? Reason { get; set; }
    public DateTime MatchedAt { get; set; } = DateTime.UtcNow;

    // Foreign keys
    public int ResumeId { get; set; }
    public Resume? Resume { get; set; }

    public int JobPostingId { get; set; }
    public JobPosting? JobPosting { get; set; }
}

