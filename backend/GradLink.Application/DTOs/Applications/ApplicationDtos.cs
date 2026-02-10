namespace GradLink.Application.DTOs.Applications;

// Job Application DTOs
public class JobApplicationDto
{
    public int Id { get; set; }
    public int JobPostingId { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string? CompanyName { get; set; }
    public string ApplicantId { get; set; } = string.Empty;
    public string ApplicantName { get; set; } = string.Empty;
    public string? ApplicantEmail { get; set; }
    public int? ResumeId { get; set; }
    public string? ResumeName { get; set; }
    public string? CoverLetter { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime AppliedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? Notes { get; set; }
}

public class CreateJobApplicationRequest
{
    public required int JobPostingId { get; set; }
    public int? ResumeId { get; set; }
    public string? CoverLetter { get; set; }
}

public class UpdateJobApplicationStatusRequest
{
    public required string Status { get; set; } // Pending, Reviewed, Shortlisted, Rejected, Accepted
    public string? Notes { get; set; }
}

// Internship Application DTOs
public class InternshipApplicationDto
{
    public int Id { get; set; }
    public int InternshipId { get; set; }
    public string InternshipTitle { get; set; } = string.Empty;
    public string? CompanyName { get; set; }
    public string ApplicantId { get; set; } = string.Empty;
    public string ApplicantName { get; set; } = string.Empty;
    public string? ApplicantEmail { get; set; }
    public int? ResumeId { get; set; }
    public string? ResumeName { get; set; }
    public string? CoverLetter { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime AppliedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? Notes { get; set; }
}

public class CreateInternshipApplicationRequest
{
    public required int InternshipId { get; set; }
    public int? ResumeId { get; set; }
    public string? CoverLetter { get; set; }
}

// My Applications (for applicants to view their own applications)
public class MyApplicationDto
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty; // Job, Internship
    public int OpportunityId { get; set; }
    public string OpportunityTitle { get; set; } = string.Empty;
    public string? CompanyName { get; set; }
    public string? Location { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime AppliedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
}

















