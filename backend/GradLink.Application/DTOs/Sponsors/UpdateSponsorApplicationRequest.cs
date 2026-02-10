using System.ComponentModel.DataAnnotations;

namespace GradLink.Application.DTOs.Sponsors;

public class UpdateSponsorApplicationRequest
{
    [MaxLength(100)]
    public string? FullName { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    [MaxLength(200)]
    public string? CompanyName { get; set; }

    [MaxLength(50)]
    public string? SponsorshipType { get; set; }

    [MaxLength(1000)]
    public string? Message { get; set; }

    [MaxLength(20)]
    public string? Status { get; set; } // Pending, Approved, Rejected
}

