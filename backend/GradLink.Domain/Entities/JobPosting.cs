namespace GradLink.Domain.Entities;

public class JobPosting
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public string? Requirements { get; set; } // JSON array
    public string? Skills { get; set; } // JSON array
    public string? Location { get; set; }
    public string? EmploymentType { get; set; } // Full-time, Part-time, Contract, etc.
    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    public string? CompanyName { get; set; }
    public string Status { get; set; } = "Active";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }

    // Foreign key
    public required string PostedById { get; set; }
    public ApplicationUser? PostedBy { get; set; }

    // Navigation properties
    public ICollection<Match> Matches { get; set; } = new List<Match>();
}

