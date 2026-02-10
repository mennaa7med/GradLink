namespace GradLink.Application.DTOs.Internships;

public class InternshipDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Requirements { get; set; }
    public string? Skills { get; set; }
    public string? Location { get; set; }
    public string? Duration { get; set; }
    public bool IsPaid { get; set; }
    public decimal? Stipend { get; set; }
    public string? CompanyName { get; set; }
    public string Status { get; set; } = "Active";
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public string PostedById { get; set; } = string.Empty;
    public string? PostedByName { get; set; }
    public int ApplicationsCount { get; set; }
}

public class CreateInternshipRequest
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? Requirements { get; set; }
    public string? Skills { get; set; }
    public string? Location { get; set; }
    public string? Duration { get; set; }
    public bool IsPaid { get; set; }
    public decimal? Stipend { get; set; }
    public string? CompanyName { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

public class UpdateInternshipRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Requirements { get; set; }
    public string? Skills { get; set; }
    public string? Location { get; set; }
    public string? Duration { get; set; }
    public bool? IsPaid { get; set; }
    public decimal? Stipend { get; set; }
    public string? CompanyName { get; set; }
    public string? Status { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

















