namespace GradLink.Application.DTOs.Sponsors;

public class SponsorApplicationDto
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string CompanyName { get; set; }
    public required string SponsorshipType { get; set; }
    public string? Message { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UserId { get; set; }
}

