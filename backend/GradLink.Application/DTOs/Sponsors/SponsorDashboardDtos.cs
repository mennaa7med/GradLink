using System.ComponentModel.DataAnnotations;

namespace GradLink.Application.DTOs.Sponsors;

// ==================== Sponsor Profile DTOs ====================

public class SponsorProfileDto
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? CompanyDescription { get; set; }
    public string? Logo { get; set; }
    public string? Website { get; set; }
    public string? Industry { get; set; }
    public string ContactPersonName { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string? ContactPhone { get; set; }
    public string? Address { get; set; }
    public List<string> FieldsOfInterest { get; set; } = new();
    public decimal? TotalBudget { get; set; }
    public decimal? RemainingBudget { get; set; }
    public int TotalProjectsSponsored { get; set; }
    public int ActiveProjects { get; set; }
    public int CompletedProjects { get; set; }
    public int PendingRequests { get; set; }
    public bool IsVerified { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateSponsorProfileRequest
{
    [Required]
    [StringLength(200)]
    public required string CompanyName { get; set; }

    [StringLength(2000)]
    public string? CompanyDescription { get; set; }

    public string? Logo { get; set; }

    [Url]
    public string? Website { get; set; }

    [StringLength(100)]
    public string? Industry { get; set; }

    [Required]
    [StringLength(100)]
    public required string ContactPersonName { get; set; }

    [Required]
    [EmailAddress]
    public required string ContactEmail { get; set; }

    [Phone]
    public string? ContactPhone { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }

    public string? FieldsOfInterest { get; set; } // Comma-separated
    public decimal? TotalBudget { get; set; }
}

public class UpdateSponsorProfileRequest
{
    [StringLength(200)]
    public string? CompanyName { get; set; }

    [StringLength(2000)]
    public string? CompanyDescription { get; set; }

    public string? Logo { get; set; }

    [Url]
    public string? Website { get; set; }

    [StringLength(100)]
    public string? Industry { get; set; }

    [StringLength(100)]
    public string? ContactPersonName { get; set; }

    [EmailAddress]
    public string? ContactEmail { get; set; }

    [Phone]
    public string? ContactPhone { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }

    public string? FieldsOfInterest { get; set; }
    public decimal? TotalBudget { get; set; }
}

// ==================== Sponsored Project DTOs ====================

public class SponsoredProjectDto
{
    public int Id { get; set; }
    public string ProjectTitle { get; set; } = string.Empty;
    public string? ProjectDescription { get; set; }
    public string? StudentName { get; set; }
    public string? TeamName { get; set; }
    public string? Category { get; set; }
    public string Status { get; set; } = "Pending";
    public string? RejectionReason { get; set; }
    public decimal? FundingAmount { get; set; }
    public decimal? FundingDelivered { get; set; }
    public int ProgressPercentage { get; set; }
    public string? CurrentMilestone { get; set; }
    public List<MilestoneDto> Milestones { get; set; } = new();
    public List<string> Documents { get; set; } = new();
    public DateTime RequestedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public int? ProjectId { get; set; }
    public string? StudentUserId { get; set; }
}

public class MilestoneDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }
}

public class SponsorProjectRequest
{
    [Required]
    public int ProjectId { get; set; }
    
    public decimal? FundingAmount { get; set; }
    
    [StringLength(1000)]
    public string? Message { get; set; }
}

public class ApproveRejectProjectRequest
{
    [Required]
    public int SponsoredProjectId { get; set; }
    
    public bool Approve { get; set; }
    
    [StringLength(1000)]
    public string? Reason { get; set; }
    
    public decimal? FundingAmount { get; set; }
}

public class UpdateProjectProgressRequest
{
    [Required]
    public int SponsoredProjectId { get; set; }
    
    [Range(0, 100)]
    public int? ProgressPercentage { get; set; }
    
    public string? CurrentMilestone { get; set; }
    
    public List<MilestoneDto>? Milestones { get; set; }
}

// ==================== Funding DTOs ====================

public class SponsorFundingDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string TransactionType { get; set; } = "Payment";
    public string Status { get; set; } = "Pending";
    public string? TransactionReference { get; set; }
    public string? Notes { get; set; }
    public string? ProjectTitle { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}

public class CreateFundingRequest
{
    [Required]
    public int SponsoredProjectId { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }
    
    [StringLength(100)]
    public string? TransactionReference { get; set; }
    
    [StringLength(500)]
    public string? Notes { get; set; }
}

// ==================== Message DTOs ====================

public class SponsorMessageDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public List<string> Attachments { get; set; } = new();
    public bool IsRead { get; set; }
    public string SenderId { get; set; } = string.Empty;
    public string? SenderName { get; set; }
    public string ReceiverId { get; set; } = string.Empty;
    public string? ReceiverName { get; set; }
    public bool IsSponsorSender { get; set; }
    public string? ProjectTitle { get; set; }
    public DateTime SentAt { get; set; }
    public DateTime? ReadAt { get; set; }
}

public class SendMessageRequest
{
    [Required]
    public required string ReceiverId { get; set; }
    
    [Required]
    [StringLength(5000)]
    public required string Content { get; set; }
    
    public int? SponsoredProjectId { get; set; }
    
    public List<string>? Attachments { get; set; }
}

// ==================== Dashboard Overview DTOs ====================

public class SponsorDashboardOverview
{
    public int TotalProjectsSponsored { get; set; }
    public int PendingRequests { get; set; }
    public int ApprovedProjects { get; set; }
    public int RejectedProjects { get; set; }
    public int CompletedProjects { get; set; }
    public int ActiveProjects { get; set; }
    public decimal TotalFundingProvided { get; set; }
    public decimal TotalFundingDelivered { get; set; }
    public decimal RemainingBudget { get; set; }
    public int UnreadMessages { get; set; }
    public List<CategoryStats> ProjectsByCategory { get; set; } = new();
    public List<MonthlyStats> MonthlyStats { get; set; } = new();
}

public class CategoryStats
{
    public string Category { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal TotalFunding { get; set; }
}

public class MonthlyStats
{
    public string Month { get; set; } = string.Empty;
    public int ProjectsApproved { get; set; }
    public int ProjectsRejected { get; set; }
    public decimal FundingProvided { get; set; }
}

// ==================== Sponsor Registration DTO ====================

public class SponsorRegistrationRequest
{
    // User Account Info
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public required string Password { get; set; }
    
    // Company Info
    [Required]
    [StringLength(200)]
    public required string CompanyName { get; set; }
    
    [StringLength(2000)]
    public string? CompanyDescription { get; set; }
    
    [StringLength(100)]
    public string? Industry { get; set; }
    
    [Url]
    public string? Website { get; set; }
    
    // Contact Info
    [Required]
    [StringLength(100)]
    public required string ContactPersonName { get; set; }
    
    [Phone]
    public string? ContactPhone { get; set; }
    
    [StringLength(500)]
    public string? Address { get; set; }
    
    // Preferences
    public string? FieldsOfInterest { get; set; }
    public decimal? InitialBudget { get; set; }
}

