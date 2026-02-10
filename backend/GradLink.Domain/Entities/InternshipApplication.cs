using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradLink.Domain.Entities;

public class InternshipApplication
{
    public int Id { get; set; }

    [Required]
    public int InternshipId { get; set; }

    [ForeignKey(nameof(InternshipId))]
    public Internship? Internship { get; set; }

    [Required]
    public string ApplicantId { get; set; } = string.Empty;

    [ForeignKey(nameof(ApplicantId))]
    public ApplicationUser? Applicant { get; set; }

    public int? ResumeId { get; set; }

    [ForeignKey(nameof(ResumeId))]
    public Resume? Resume { get; set; }

    [MaxLength(2000)]
    public string? CoverLetter { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "Pending"; // Pending, Reviewed, Shortlisted, Rejected, Accepted

    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ReviewedAt { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }
}

















