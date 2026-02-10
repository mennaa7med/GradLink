namespace GradLink.Domain.Entities;

/// <summary>
/// MentorProfile - Extended profile data specific to Mentors
/// Contains mentoring preferences, availability, and session data
/// </summary>
public class MentorProfile
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
    // MENTORING DETAILS
    // ============================================

    /// <summary>
    /// Areas of expertise (comma-separated)
    /// </summary>
    public string? ExpertiseAreas { get; set; }

    /// <summary>
    /// Mentoring style: Hands-on, Advisory, Coaching, etc.
    /// </summary>
    public string? MentoringStyle { get; set; }

    /// <summary>
    /// Maximum number of mentees
    /// </summary>
    public int? MaxMentees { get; set; }

    /// <summary>
    /// Current number of mentees
    /// </summary>
    public int CurrentMenteesCount { get; set; } = 0;

    /// <summary>
    /// Accepting new mentees
    /// </summary>
    public bool IsAcceptingMentees { get; set; } = true;

    /// <summary>
    /// Preferred communication methods (comma-separated)
    /// </summary>
    public string? PreferredCommunication { get; set; } // e.g., "Video Call, Chat, Email"

    /// <summary>
    /// Available time slots (JSON)
    /// </summary>
    public string? AvailableTimeSlots { get; set; }

    /// <summary>
    /// Timezone
    /// </summary>
    public string? Timezone { get; set; }

    // ============================================
    // SESSION & PRICING
    // ============================================

    /// <summary>
    /// Session duration in minutes
    /// </summary>
    public int SessionDurationMinutes { get; set; } = 60;

    /// <summary>
    /// Is mentoring free
    /// </summary>
    public bool IsFree { get; set; } = true;

    /// <summary>
    /// Hourly rate (if paid)
    /// </summary>
    public decimal? HourlyRate { get; set; }

    /// <summary>
    /// Currency for payment
    /// </summary>
    public string? Currency { get; set; } = "USD";

    // ============================================
    // STATISTICS
    // ============================================

    /// <summary>
    /// Total mentoring sessions conducted
    /// </summary>
    public int TotalSessions { get; set; } = 0;

    /// <summary>
    /// Total hours of mentoring
    /// </summary>
    public decimal TotalHours { get; set; } = 0;

    /// <summary>
    /// Total students mentored
    /// </summary>
    public int TotalStudentsMentored { get; set; } = 0;

    /// <summary>
    /// Average rating from mentees (1-5)
    /// </summary>
    public decimal? AverageRating { get; set; }

    /// <summary>
    /// Number of reviews
    /// </summary>
    public int ReviewsCount { get; set; } = 0;

    /// <summary>
    /// Response rate percentage
    /// </summary>
    public int ResponseRate { get; set; } = 100;

    // ============================================
    // QUALIFICATIONS
    // ============================================

    /// <summary>
    /// Educational background (JSON array)
    /// </summary>
    public string? Education { get; set; }

    /// <summary>
    /// Work experience (JSON array)
    /// </summary>
    public string? WorkExperience { get; set; }

    /// <summary>
    /// Achievements/Awards (JSON array)
    /// </summary>
    public string? Achievements { get; set; }

    /// <summary>
    /// Featured projects/case studies (JSON array)
    /// </summary>
    public string? FeaturedProjects { get; set; }

    // ============================================
    // VERIFICATION
    // ============================================

    /// <summary>
    /// Is mentor verified
    /// </summary>
    public bool IsVerified { get; set; } = false;

    /// <summary>
    /// Verification date
    /// </summary>
    public DateTime? VerifiedAt { get; set; }

    /// <summary>
    /// Verification documents (JSON array)
    /// </summary>
    public string? VerificationDocuments { get; set; }

    // ============================================
    // SYSTEM DATA
    // ============================================

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}






