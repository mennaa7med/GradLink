namespace GradLink.Application.Common.Interfaces;

public interface IAnalyzerService
{
    Task<ResumeAnalysisResult> AnalyzeResumeAsync(string filePath, CancellationToken cancellationToken = default);
}

public class ResumeAnalysisResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public string? ExtractedText { get; set; }
    public List<string> Skills { get; set; } = new();
    public string? Education { get; set; }
    public string? Experience { get; set; }
    public string? Summary { get; set; }
    public double QualityScore { get; set; }
}

