namespace GradLink.Application.DTOs.Resumes;

public class ResumeDto
{
    public int Id { get; set; }
    public required string FileName { get; set; }
    public long FileSize { get; set; }
    public DateTime UploadedAt { get; set; }
    public string? AnalysisStatus { get; set; }
    public DateTime? AnalyzedAt { get; set; }
    public string? ExtractedText { get; set; }
    public List<string>? Skills { get; set; }
    public string? Education { get; set; }
    public string? Experience { get; set; }
    public string? Summary { get; set; }
    public double? QualityScore { get; set; }
    public required string UserId { get; set; }
}

