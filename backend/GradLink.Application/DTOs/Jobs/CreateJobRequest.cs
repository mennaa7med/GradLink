using System.ComponentModel.DataAnnotations;

namespace GradLink.Application.DTOs.Jobs;

public class CreateJobRequest
{
    [Required]
    public required string Title { get; set; }

    [Required]
    public required string Description { get; set; }

    public List<string>? Requirements { get; set; }
    public List<string>? Skills { get; set; }
    public string? Location { get; set; }
    public string? EmploymentType { get; set; }
    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    public string? CompanyName { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

