namespace GradLink.Domain.Entities;

public class Resume
{
    public int Id { get; set; }
    public required string FileName { get; set; }
    public required string FilePath { get; set; }
    public long FileSize { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    
    // Analysis results
    public string? AnalysisStatus { get; set; } = "Pending";
    public DateTime? AnalyzedAt { get; set; }
    public string? ExtractedText { get; set; }
    public string? Skills { get; set; } // JSON array
    public string? Education { get; set; }
    public string? Experience { get; set; }
    public string? Summary { get; set; }
    public double? QualityScore { get; set; }

    // Foreign key
    public required string UserId { get; set; }
    public ApplicationUser? User { get; set; }

    // Navigation properties
    public ICollection<Match> Matches { get; set; } = new List<Match>();
}

