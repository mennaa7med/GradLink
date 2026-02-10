namespace GradLink.Application.DTOs.Jobs;

public class JobPostingDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public List<string>? Requirements { get; set; }
    public List<string>? Skills { get; set; }
    public string? Location { get; set; }
    public string? EmploymentType { get; set; }
    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    public string? CompanyName { get; set; }
    public string Status { get; set; } = "Active";
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public required string PostedById { get; set; }
    public string? PostedByName { get; set; }
}

