namespace GradLink.Domain.Entities;

/// <summary>
/// CompanyProfile - Extended profile data specific to Companies
/// Contains company details, hiring preferences, and recruitment data
/// </summary>
public class CompanyProfile
{
    public int Id { get; set; }

    // ============================================
    // LINK TO USER
    // ============================================

    /// <summary>
    /// Foreign key to ApplicationUser
    /// </summary>
    public required string UserId { get; set; }
    public ApplicationUser? User { get; set; }

    // ============================================
    // COMPANY DETAILS
    // ============================================

    /// <summary>
    /// Company name
    /// </summary>
    public string? CompanyName { get; set; }

    /// <summary>
    /// Company description/about
    /// </summary>
    public string? CompanyDescription { get; set; }

    /// <summary>
    /// Industry sector
    /// </summary>
    public string? Industry { get; set; }

    /// <summary>
    /// Company size category (e.g., "1-10", "11-50", "51-200")
    /// </summary>
    public string? CompanySize { get; set; }

    /// <summary>
    /// Company website URL
    /// </summary>
    public string? Website { get; set; }

    /// <summary>
    /// Company logo URL
    /// </summary>
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Contact email for recruitment
    /// </summary>
    public string? ContactEmail { get; set; }

    /// <summary>
    /// Contact phone for recruitment
    /// </summary>
    public string? ContactPhone { get; set; }

    /// <summary>
    /// Company address
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Currently hiring status
    /// </summary>
    public bool HiringStatus { get; set; } = true;

    /// <summary>
    /// Company registration number
    /// </summary>
    public string? RegistrationNumber { get; set; }

    /// <summary>
    /// Year company was founded
    /// </summary>
    public int? FoundedYear { get; set; }

    /// <summary>
    /// Number of employees
    /// </summary>
    public string? EmployeeCount { get; set; } // e.g., "1-10", "11-50", "51-200", "201-500", "500+"

    /// <summary>
    /// Company type: Startup, SME, Enterprise, NGO, Government
    /// </summary>
    public string? CompanyType { get; set; }

    /// <summary>
    /// Headquarters location
    /// </summary>
    public string? Headquarters { get; set; }

    /// <summary>
    /// Office locations (JSON array)
    /// </summary>
    public string? OfficeLocations { get; set; }

    /// <summary>
    /// Company culture description
    /// </summary>
    public string? CultureDescription { get; set; }

    /// <summary>
    /// Company benefits offered (JSON array)
    /// </summary>
    public string? Benefits { get; set; }

    /// <summary>
    /// Technologies used by the company (comma-separated)
    /// </summary>
    public string? TechStack { get; set; }

    // ============================================
    // HIRING PREFERENCES
    // ============================================

    /// <summary>
    /// Currently hiring
    /// </summary>
    public bool IsHiring { get; set; } = false;

    /// <summary>
    /// Open positions count
    /// </summary>
    public int OpenPositionsCount { get; set; } = 0;

    /// <summary>
    /// Preferred skills (comma-separated)
    /// </summary>
    public string? PreferredSkills { get; set; }

    /// <summary>
    /// Preferred universities (comma-separated)
    /// </summary>
    public string? PreferredUniversities { get; set; }

    /// <summary>
    /// Minimum GPA requirement
    /// </summary>
    public decimal? MinimumGPA { get; set; }

    /// <summary>
    /// Offers internships
    /// </summary>
    public bool OffersInternships { get; set; } = false;

    /// <summary>
    /// Offers project-based jobs
    /// </summary>
    public bool OffersProjectJobs { get; set; } = false;

    /// <summary>
    /// Internship duration options (comma-separated)
    /// </summary>
    public string? InternshipDurations { get; set; }

    // ============================================
    // RECRUITMENT STATISTICS
    // ============================================

    /// <summary>
    /// Total hires made
    /// </summary>
    public int TotalHires { get; set; } = 0;

    /// <summary>
    /// Total applications received
    /// </summary>
    public int TotalApplicationsReceived { get; set; } = 0;

    /// <summary>
    /// Average response time (in days)
    /// </summary>
    public int? AverageResponseTime { get; set; }

    /// <summary>
    /// Company rating by applicants (1-5)
    /// </summary>
    public decimal? Rating { get; set; }

    /// <summary>
    /// Number of reviews
    /// </summary>
    public int ReviewsCount { get; set; } = 0;

    // ============================================
    // VERIFICATION & TRUST
    // ============================================

    /// <summary>
    /// Company is verified
    /// </summary>
    public bool IsVerified { get; set; } = false;

    /// <summary>
    /// Verification date
    /// </summary>
    public DateTime? VerifiedAt { get; set; }

    /// <summary>
    /// Verification documents uploaded (JSON array)
    /// </summary>
    public string? VerificationDocuments { get; set; }

    /// <summary>
    /// Trust score (computed)
    /// </summary>
    public int TrustScore { get; set; } = 0;

    // ============================================
    // SYSTEM DATA
    // ============================================

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}


