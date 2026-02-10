using System.ComponentModel.DataAnnotations;

namespace GradLink.Application.DTOs.Sponsors;

public class CreateSponsorApplicationRequest
{
    [Required]
    [MaxLength(100)]
    public required string FullName { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [MaxLength(200)]
    public required string CompanyName { get; set; }

    [Required]
    [MaxLength(50)]
    public required string SponsorshipType { get; set; } // Funding, Mentorship, Resources

    [MaxLength(1000)]
    public string? Message { get; set; }
}

