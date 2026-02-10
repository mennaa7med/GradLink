namespace GradLink.Domain.Entities;

/// <summary>
/// StudentProfile - Extended profile data specific to Students
/// Contains graduation project details, academic achievements, and career preferences
/// </summary>
public class StudentProfile
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
    // GRADUATION PROJECT DATA
    // ============================================

    /// <summary>
    /// Graduation project title
    /// </summary>
    public string? ProjectTitle { get; set; }

    /// <summary>
    /// Graduation project description
    /// </summary>
    public string? ProjectDescription { get; set; }

    /// <summary>
    /// Project category: AI, Web, Mobile, IoT, etc.
    /// </summary>
    public string? ProjectCategory { get; set; }

    /// <summary>
    /// Project status: Idea, InProgress, Completed, Submitted
    /// </summary>
    public string? ProjectStatus { get; set; } = "Idea";

    /// <summary>
    /// Team name
    /// </summary>
    public string? TeamName { get; set; }

    /// <summary>
    /// Team size
    /// </summary>
    public int? TeamSize { get; set; }

    /// <summary>
    /// Supervisor/Advisor name
    /// </summary>
    public string? SupervisorName { get; set; }

    /// <summary>
    /// Supervisor email
    /// </summary>
    public string? SupervisorEmail { get; set; }

    /// <summary>
    /// Project start date
    /// </summary>
    public DateTime? ProjectStartDate { get; set; }

    /// <summary>
    /// Expected project end date
    /// </summary>
    public DateTime? ProjectEndDate { get; set; }

    /// <summary>
    /// Project technologies used (comma-separated)
    /// </summary>
    public string? ProjectTechnologies { get; set; }

    /// <summary>
    /// Project GitHub repository URL
    /// </summary>
    public string? ProjectGitHubUrl { get; set; }

    /// <summary>
    /// Project demo/live URL
    /// </summary>
    public string? ProjectDemoUrl { get; set; }

    /// <summary>
    /// Project documentation URL
    /// </summary>
    public string? ProjectDocumentationUrl { get; set; }

    // ============================================
    // ACADEMIC ACHIEVEMENTS
    // ============================================

    /// <summary>
    /// Academic honors/awards (JSON array)
    /// </summary>
    public string? AcademicAwards { get; set; }

    /// <summary>
    /// Relevant courses taken (JSON array)
    /// </summary>
    public string? RelevantCourses { get; set; }

    /// <summary>
    /// Academic projects completed (JSON array)
    /// </summary>
    public string? AcademicProjects { get; set; }

    /// <summary>
    /// Research papers/publications (JSON array)
    /// </summary>
    public string? Publications { get; set; }

    // ============================================
    // CAREER PREFERENCES
    // ============================================

    /// <summary>
    /// Career interests (comma-separated)
    /// </summary>
    public string? CareerInterests { get; set; }

    /// <summary>
    /// Preferred job types: Full-time, Part-time, Internship, Freelance
    /// </summary>
    public string? PreferredJobTypes { get; set; }

    /// <summary>
    /// Preferred work locations (comma-separated)
    /// </summary>
    public string? PreferredLocations { get; set; }

    /// <summary>
    /// Expected salary range
    /// </summary>
    public string? ExpectedSalary { get; set; }

    /// <summary>
    /// Availability: Immediately, 1 Month, 3 Months, After Graduation
    /// </summary>
    public string? Availability { get; set; }

    /// <summary>
    /// Open to relocation
    /// </summary>
    public bool IsOpenToRelocation { get; set; } = false;

    /// <summary>
    /// Open to remote work
    /// </summary>
    public bool IsOpenToRemote { get; set; } = true;

    // ============================================
    // MENTORSHIP & SPONSORSHIP
    // ============================================

    /// <summary>
    /// Looking for mentor
    /// </summary>
    public bool IsLookingForMentor { get; set; } = false;

    /// <summary>
    /// Looking for sponsor/funding
    /// </summary>
    public bool IsLookingForSponsor { get; set; } = false;

    /// <summary>
    /// Funding amount needed
    /// </summary>
    public decimal? FundingNeeded { get; set; }

    /// <summary>
    /// Funding purpose description
    /// </summary>
    public string? FundingPurpose { get; set; }

    // ============================================
    // SYSTEM DATA
    // ============================================

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}






