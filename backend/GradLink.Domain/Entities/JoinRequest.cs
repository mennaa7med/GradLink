namespace GradLink.Domain.Entities;

public class JoinRequest
{
    public int Id { get; set; }
    
    // Team Request Reference
    public int TeamRequestId { get; set; }
    public TeamRequest? TeamRequest { get; set; }
    
    // Applicant Information
    public string ApplicantId { get; set; } = string.Empty;
    public ApplicationUser? Applicant { get; set; }
    public string ApplicantName { get; set; } = string.Empty;
    public string ApplicantEmail { get; set; } = string.Empty;
    public string ApplicantSkills { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    
    // Status
    public string Status { get; set; } = "Pending"; // Pending, Accepted, Rejected
    
    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReviewedAt { get; set; }
}






