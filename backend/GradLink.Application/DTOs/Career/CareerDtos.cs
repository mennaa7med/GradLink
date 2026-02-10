namespace GradLink.Application.DTOs.Career;

/// <summary>
/// Represents a single opportunity (job, internship, or project) for the public career page
/// </summary>
public class OpportunityDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public List<string>? Skills { get; set; }
    public List<string>? Requirements { get; set; }
    public string? Type { get; set; } // Job, Internship, Project
    public string? EmploymentType { get; set; } // Full-time, Part-time, Contract, Remote
    public string? Duration { get; set; }
    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    public bool? IsPaid { get; set; }
    public decimal? Stipend { get; set; }
    public string? Budget { get; set; }
    public string Status { get; set; } = "Active";
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

/// <summary>
/// Represents a company with all its opportunities grouped
/// </summary>
public class CompanyOpportunitiesDto
{
    public required string CompanyId { get; set; }
    public required string CompanyName { get; set; }
    public string? CompanyLogo { get; set; }
    public string? Industry { get; set; }
    public string? Location { get; set; }
    public string? Website { get; set; }
    public string? Description { get; set; }
    public bool IsVerified { get; set; }
    
    // Opportunities grouped by type
    public List<OpportunityDto> Jobs { get; set; } = new();
    public List<OpportunityDto> Internships { get; set; } = new();
    public List<OpportunityDto> Projects { get; set; } = new();
    
    // Summary counts
    public int TotalJobs => Jobs.Count;
    public int TotalInternships => Internships.Count;
    public int TotalProjects => Projects.Count;
    public int TotalOpportunities => Jobs.Count + Internships.Count + Projects.Count;
}

/// <summary>
/// Response for the career page with all companies and their opportunities
/// </summary>
public class CareerPageResponse
{
    public List<CompanyOpportunitiesDto> Companies { get; set; } = new();
    
    // Summary statistics
    public int TotalCompanies => Companies.Count;
    public int TotalJobs => Companies.Sum(c => c.TotalJobs);
    public int TotalInternships => Companies.Sum(c => c.TotalInternships);
    public int TotalProjects => Companies.Sum(c => c.TotalProjects);
    public int TotalOpportunities => Companies.Sum(c => c.TotalOpportunities);
}

/// <summary>
/// Filter options for the career page
/// </summary>
public class CareerFilterDto
{
    public string? Search { get; set; }
    public string? Location { get; set; }
    public string? Type { get; set; } // Job, Internship, Project, or null for all
    public string? EmploymentType { get; set; }
    public string? Industry { get; set; }
    public bool? IsPaid { get; set; }
    public int? Page { get; set; } = 1;
    public int? PageSize { get; set; } = 20;
}


















