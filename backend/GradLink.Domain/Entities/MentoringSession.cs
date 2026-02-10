namespace GradLink.Domain.Entities;

/// <summary>
/// MentoringSession - Tracks all mentorship sessions/meetings between Mentors and Students
/// Records session scheduling, type (online/in-person), notes, ratings, and reminders.
/// Used in: Sessions page, Upcoming Sessions, Calendar View, Notifications
/// </summary>
public class MentoringSession
{
    /// <summary>
    /// Primary Key - Unique identifier for the session
    /// </summary>
    public int Id { get; set; }

    // ============================================
    // PARTICIPANTS
    // ============================================

    /// <summary>
    /// Foreign key to ApplicationUser (Mentor)
    /// The mentor conducting the session
    /// </summary>
    public required string MentorId { get; set; }

    /// <summary>
    /// Navigation property to the Mentor user
    /// </summary>
    public ApplicationUser? Mentor { get; set; }

    /// <summary>
    /// Foreign key to ApplicationUser (Mentee/Student)
    /// The student attending the session
    /// </summary>
    public required string MenteeId { get; set; }

    /// <summary>
    /// Navigation property to the Mentee user
    /// </summary>
    public ApplicationUser? Mentee { get; set; }

    /// <summary>
    /// Foreign key to MentorshipRelation (optional)
    /// Links session to the mentorship relationship
    /// </summary>
    public int? MentorshipRelationId { get; set; }

    /// <summary>
    /// Navigation property to the mentorship relation
    /// </summary>
    public MentorshipRelation? MentorshipRelation { get; set; }

    // ============================================
    // SESSION DETAILS
    // ============================================

    /// <summary>
    /// Topic/Title of the session
    /// e.g., "Project Review", "Career Guidance", "Technical Discussion"
    /// </summary>
    public required string Topic { get; set; }

    /// <summary>
    /// Detailed description of what will be covered
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Scheduled date of the session
    /// </summary>
    public DateTime ScheduledDate { get; set; }

    /// <summary>
    /// Start time of the session
    /// </summary>
    public TimeSpan StartTime { get; set; }

    /// <summary>
    /// End time of the session
    /// </summary>
    public TimeSpan? EndTime { get; set; }

    /// <summary>
    /// Duration of the session in minutes
    /// Default: 60 minutes
    /// </summary>
    public int DurationMinutes { get; set; } = 60;

    /// <summary>
    /// Timezone for the session
    /// </summary>
    public string? Timezone { get; set; }

    // ============================================
    // SESSION TYPE
    // ============================================

    /// <summary>
    /// Type of session: Online, InPerson, Hybrid
    /// </summary>
    public string SessionType { get; set; } = "Online";

    /// <summary>
    /// Meeting link for online sessions
    /// e.g., Zoom, Google Meet, Microsoft Teams link
    /// </summary>
    public string? MeetingLink { get; set; }

    /// <summary>
    /// Meeting platform: Zoom, GoogleMeet, Teams, Other
    /// </summary>
    public string? MeetingPlatform { get; set; }

    /// <summary>
    /// Meeting ID (for platforms that require it)
    /// </summary>
    public string? MeetingId { get; set; }

    /// <summary>
    /// Meeting password (if required)
    /// </summary>
    public string? MeetingPassword { get; set; }

    /// <summary>
    /// Physical location for in-person sessions
    /// e.g., "University Library, Room 205"
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Address for in-person sessions
    /// </summary>
    public string? Address { get; set; }

    // ============================================
    // SESSION STATUS
    // ============================================

    /// <summary>
    /// Current status of the session
    /// Values: Scheduled, InProgress, Completed, Cancelled, NoShow, Rescheduled
    /// </summary>
    public string Status { get; set; } = "Scheduled";

    /// <summary>
    /// Reason for cancellation or rescheduling
    /// </summary>
    public string? StatusReason { get; set; }

    /// <summary>
    /// Who cancelled the session: Mentor, Mentee, System
    /// </summary>
    public string? CancelledBy { get; set; }

    /// <summary>
    /// Actual start time (when session actually started)
    /// </summary>
    public DateTime? ActualStartTime { get; set; }

    /// <summary>
    /// Actual end time (when session actually ended)
    /// </summary>
    public DateTime? ActualEndTime { get; set; }

    /// <summary>
    /// Actual duration in minutes
    /// </summary>
    public int? ActualDurationMinutes { get; set; }

    // ============================================
    // NOTES & CONTENT
    // ============================================

    /// <summary>
    /// Notes prepared before the session (agenda)
    /// </summary>
    public string? AgendaNotes { get; set; }

    /// <summary>
    /// Notes taken by mentor during/after the session
    /// </summary>
    public string? MentorNotes { get; set; }

    /// <summary>
    /// Notes taken by mentee during/after the session
    /// </summary>
    public string? MenteeNotes { get; set; }

    /// <summary>
    /// Key takeaways or action items
    /// </summary>
    public string? ActionItems { get; set; }

    /// <summary>
    /// Resources shared during the session (JSON array of URLs/files)
    /// </summary>
    public string? SharedResources { get; set; }

    /// <summary>
    /// Session recording URL (if recorded)
    /// </summary>
    public string? RecordingUrl { get; set; }

    // ============================================
    // FEEDBACK & RATING
    // ============================================

    /// <summary>
    /// Has the mentee rated the session
    /// </summary>
    public bool HasMenteeRating { get; set; } = false;

    /// <summary>
    /// Rating from mentee (1-5 stars)
    /// </summary>
    public int? MenteeRating { get; set; }

    /// <summary>
    /// Feedback comment from mentee
    /// </summary>
    public string? MenteeFeedback { get; set; }

    /// <summary>
    /// Has the mentor rated the session/mentee
    /// </summary>
    public bool HasMentorRating { get; set; } = false;

    /// <summary>
    /// Rating from mentor about mentee's engagement (1-5)
    /// </summary>
    public int? MentorRating { get; set; }

    /// <summary>
    /// Feedback comment from mentor
    /// </summary>
    public string? MentorFeedback { get; set; }

    /// <summary>
    /// Was the session helpful (from mentee perspective)
    /// </summary>
    public bool? WasHelpful { get; set; }

    /// <summary>
    /// Would the mentee recommend this mentor
    /// </summary>
    public bool? WouldRecommend { get; set; }

    // ============================================
    // REMINDERS & NOTIFICATIONS
    // ============================================

    /// <summary>
    /// Was the reminder sent
    /// </summary>
    public bool ReminderSent { get; set; } = false;

    /// <summary>
    /// When was the reminder sent
    /// </summary>
    public DateTime? ReminderSentAt { get; set; }

    /// <summary>
    /// Reminder time in minutes before session (default: 30)
    /// </summary>
    public int ReminderMinutesBefore { get; set; } = 30;

    /// <summary>
    /// Was the mentor notified
    /// </summary>
    public bool MentorNotified { get; set; } = false;

    /// <summary>
    /// Was the mentee notified
    /// </summary>
    public bool MenteeNotified { get; set; } = false;

    // ============================================
    // RECURRENCE (Optional - for recurring sessions)
    // ============================================

    /// <summary>
    /// Is this a recurring session
    /// </summary>
    public bool IsRecurring { get; set; } = false;

    /// <summary>
    /// Recurrence pattern: None, Daily, Weekly, BiWeekly, Monthly
    /// </summary>
    public string? RecurrencePattern { get; set; }

    /// <summary>
    /// Parent session ID for recurring sessions
    /// </summary>
    public int? ParentSessionId { get; set; }

    /// <summary>
    /// Recurrence end date
    /// </summary>
    public DateTime? RecurrenceEndDate { get; set; }

    // ============================================
    // TIMESTAMPS
    // ============================================

    /// <summary>
    /// When the session was created/scheduled
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Last update timestamp
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// When the session was completed
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// When the session was cancelled
    /// </summary>
    public DateTime? CancelledAt { get; set; }

    // ============================================
    // METADATA
    // ============================================

    /// <summary>
    /// Session number in the mentorship (1st, 2nd, etc.)
    /// </summary>
    public int SessionNumber { get; set; } = 1;

    /// <summary>
    /// Is this a paid session
    /// </summary>
    public bool IsPaid { get; set; } = false;

    /// <summary>
    /// Session fee (if paid)
    /// </summary>
    public decimal? SessionFee { get; set; }

    /// <summary>
    /// Currency for payment
    /// </summary>
    public string? Currency { get; set; }

    /// <summary>
    /// Payment status: NotRequired, Pending, Paid, Refunded
    /// </summary>
    public string? PaymentStatus { get; set; }
}






