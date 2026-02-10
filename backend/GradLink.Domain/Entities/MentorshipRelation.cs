namespace GradLink.Domain.Entities;

/// <summary>
/// MentorshipRelation - Tracks the relationship between Mentors and Students (Mentees)
/// Records mentorship requests, tracks relationship status, and links mentors to their mentees.
/// Used in: My Mentees page, Find Mentors page, Overview statistics
/// </summary>
public class MentorshipRelation
{
    /// <summary>
    /// Primary Key - Unique identifier for the mentorship relation
    /// </summary>
    public int Id { get; set; }

    // ============================================
    // MENTOR INFORMATION
    // ============================================

    /// <summary>
    /// Foreign key to ApplicationUser (Mentor)
    /// The mentor providing guidance and support
    /// </summary>
    public required string MentorId { get; set; }

    /// <summary>
    /// Navigation property to the Mentor user
    /// </summary>
    public ApplicationUser? Mentor { get; set; }

    // ============================================
    // MENTEE (STUDENT) INFORMATION
    // ============================================

    /// <summary>
    /// Foreign key to ApplicationUser (Mentee/Student)
    /// The student receiving mentorship
    /// </summary>
    public required string MenteeId { get; set; }

    /// <summary>
    /// Navigation property to the Mentee user
    /// </summary>
    public ApplicationUser? Mentee { get; set; }

    // ============================================
    // RELATIONSHIP STATUS
    // ============================================

    /// <summary>
    /// Current status of the mentorship relationship
    /// Values: Pending, Active, Completed, Cancelled, Rejected
    /// - Pending: Student sent request, waiting for mentor approval
    /// - Active: Mentor accepted, mentorship is ongoing
    /// - Completed: Mentorship finished successfully
    /// - Cancelled: Either party cancelled the mentorship
    /// - Rejected: Mentor rejected the request
    /// </summary>
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// Reason for rejection or cancellation (optional)
    /// </summary>
    public string? StatusReason { get; set; }

    // ============================================
    // REQUEST DETAILS
    // ============================================

    /// <summary>
    /// Message from the student when requesting mentorship
    /// Explains why they want this mentor's guidance
    /// </summary>
    public string? RequestMessage { get; set; }

    /// <summary>
    /// Goals the student wants to achieve through mentorship
    /// </summary>
    public string? MenteeGoals { get; set; }

    /// <summary>
    /// Preferred communication method: Chat, Video Call, In-Person
    /// </summary>
    public string? PreferredCommunication { get; set; }

    /// <summary>
    /// Expected duration of mentorship (in weeks/months)
    /// </summary>
    public string? ExpectedDuration { get; set; }

    // ============================================
    // MENTOR RESPONSE
    // ============================================

    /// <summary>
    /// Message from the mentor when accepting/rejecting
    /// </summary>
    public string? MentorResponse { get; set; }

    /// <summary>
    /// Initial guidance plan set by the mentor
    /// </summary>
    public string? GuidancePlan { get; set; }

    // ============================================
    // PROGRESS TRACKING
    // ============================================

    /// <summary>
    /// Current progress percentage (0-100)
    /// Updated based on sessions completed, goals achieved
    /// </summary>
    public int ProgressPercentage { get; set; } = 0;

    /// <summary>
    /// Number of sessions completed
    /// </summary>
    public int SessionsCompleted { get; set; } = 0;

    /// <summary>
    /// Total hours of mentoring
    /// </summary>
    public decimal TotalHours { get; set; } = 0;

    /// <summary>
    /// Notes about the mentorship progress (JSON array)
    /// </summary>
    public string? ProgressNotes { get; set; }

    // ============================================
    // TIMESTAMPS
    // ============================================

    /// <summary>
    /// When the mentorship request was sent
    /// </summary>
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// When the mentor accepted/rejected the request
    /// </summary>
    public DateTime? RespondedAt { get; set; }

    /// <summary>
    /// When the mentorship became active
    /// </summary>
    public DateTime? StartedAt { get; set; }

    /// <summary>
    /// When the mentorship was completed/cancelled
    /// </summary>
    public DateTime? EndedAt { get; set; }

    /// <summary>
    /// Last activity in this mentorship
    /// </summary>
    public DateTime? LastActivityAt { get; set; }

    /// <summary>
    /// Record creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Last update timestamp
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    // ============================================
    // NAVIGATION PROPERTIES
    // ============================================

    /// <summary>
    /// All mentoring sessions within this mentorship relation
    /// </summary>
    public ICollection<MentoringSession> Sessions { get; set; } = new List<MentoringSession>();

    // ============================================
    // FEEDBACK & RATING
    // ============================================

    /// <summary>
    /// Has the mentee provided feedback after completion
    /// </summary>
    public bool HasMenteeFeedback { get; set; } = false;

    /// <summary>
    /// Final rating from mentee (1-5)
    /// </summary>
    public int? MenteeRating { get; set; }

    /// <summary>
    /// Final feedback from mentee
    /// </summary>
    public string? MenteeFeedback { get; set; }

    /// <summary>
    /// Has the mentor provided feedback
    /// </summary>
    public bool HasMentorFeedback { get; set; } = false;

    /// <summary>
    /// Final feedback from mentor about the mentee
    /// </summary>
    public string? MentorFeedback { get; set; }
}

