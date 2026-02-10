namespace GradLink.Application.DTOs.Matches;

public class MatchDto
{
    public int Id { get; set; }
    public double Score { get; set; }
    public string? Reason { get; set; }
    public DateTime MatchedAt { get; set; }
    public int ResumeId { get; set; }
    public string? ResumeFileName { get; set; }
    public int JobPostingId { get; set; }
    public string? JobTitle { get; set; }
}

