namespace GradLink.Domain.Entities;

/// <summary>
/// MentorReview - Public reviews and ratings for Mentors
/// Displayed on mentor profiles for students to see before requesting mentorship.
/// Used in: Mentor Profile, Find Mentors page, Overview statistics
/// </summary>
public class MentorReview
{
    /// <summary>
    /// Primary Key - Unique identifier for the review
    /// </summary>
    public int Id { get; set; }

    // ============================================
    // MENTOR BEING REVIEWED
    // ============================================

    /// <summary>
    /// Foreign key to ApplicationUser (Mentor)
    /// The mentor being reviewed
    /// </summary>
    public required string MentorId { get; set; }

    /// <summary>
    /// Navigation property to the Mentor user
    /// </summary>
    public ApplicationUser? Mentor { get; set; }

    // ============================================
    // REVIEWER (STUDENT/MENTEE)
    // ============================================

    /// <summary>
    /// Foreign key to ApplicationUser (Reviewer)
    /// The student who wrote the review
    /// </summary>
    public required string ReviewerId { get; set; }

    /// <summary>
    /// Navigation property to the Reviewer user
    /// </summary>
    public ApplicationUser? Reviewer { get; set; }

    // ============================================
    // LINKED ENTITIES (Optional)
    // ============================================

    /// <summary>
    /// Foreign key to MentorshipRelation (optional)
    /// Links review to the mentorship relationship
    /// </summary>
    public int? MentorshipRelationId { get; set; }

    /// <summary>
    /// Navigation property to the mentorship relation
    /// </summary>
    public MentorshipRelation? MentorshipRelation { get; set; }

    /// <summary>
    /// Foreign key to MentoringSession (optional)
    /// Links review to a specific session
    /// </summary>
    public int? MentoringSessionId { get; set; }

    /// <summary>
    /// Navigation property to the session
    /// </summary>
    public MentoringSession? MentoringSession { get; set; }

    // ============================================
    // RATING
    // ============================================

    /// <summary>
    /// Overall rating (1-5 stars)
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    /// Knowledge & Expertise rating (1-5)
    /// </summary>
    public int? KnowledgeRating { get; set; }

    /// <summary>
    /// Communication skills rating (1-5)
    /// </summary>
    public int? CommunicationRating { get; set; }

    /// <summary>
    /// Availability & Responsiveness rating (1-5)
    /// </summary>
    public int? AvailabilityRating { get; set; }

    /// <summary>
    /// Helpfulness rating (1-5)
    /// </summary>
    public int? HelpfulnessRating { get; set; }

    // ============================================
    // REVIEW CONTENT
    // ============================================

    /// <summary>
    /// Review title/headline
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Main review comment
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// What the reviewer liked most
    /// </summary>
    public string? Pros { get; set; }

    /// <summary>
    /// What could be improved
    /// </summary>
    public string? Cons { get; set; }

    /// <summary>
    /// Would the reviewer recommend this mentor
    /// </summary>
    public bool WouldRecommend { get; set; } = true;

    // ============================================
    // REVIEW CONTEXT
    // ============================================

    /// <summary>
    /// Type of mentorship: Project, Career, Technical, Academic
    /// </summary>
    public string? MentorshipType { get; set; }

    /// <summary>
    /// Duration of mentorship when review was written
    /// </summary>
    public string? MentorshipDuration { get; set; }

    /// <summary>
    /// Number of sessions when review was written
    /// </summary>
    public int? SessionsCount { get; set; }

    // ============================================
    // MODERATION
    // ============================================

    /// <summary>
    /// Is the review approved for public display
    /// </summary>
    public bool IsApproved { get; set; } = true;

    /// <summary>
    /// Is the review flagged for review
    /// </summary>
    public bool IsFlagged { get; set; } = false;

    /// <summary>
    /// Reason for flagging
    /// </summary>
    public string? FlagReason { get; set; }

    /// <summary>
    /// Is the review hidden
    /// </summary>
    public bool IsHidden { get; set; } = false;

    /// <summary>
    /// Moderation notes
    /// </summary>
    public string? ModerationNotes { get; set; }

    // ============================================
    // VISIBILITY
    // ============================================

    /// <summary>
    /// Is the reviewer's name shown publicly
    /// </summary>
    public bool ShowReviewerName { get; set; } = true;

    /// <summary>
    /// Is the review verified (from actual mentee)
    /// </summary>
    public bool IsVerified { get; set; } = false;

    // ============================================
    // HELPFULNESS
    // ============================================

    /// <summary>
    /// Number of users who found this review helpful
    /// </summary>
    public int HelpfulCount { get; set; } = 0;

    /// <summary>
    /// Number of users who found this review not helpful
    /// </summary>
    public int NotHelpfulCount { get; set; } = 0;

    // ============================================
    // MENTOR RESPONSE
    // ============================================

    /// <summary>
    /// Has the mentor responded to this review
    /// </summary>
    public bool HasMentorResponse { get; set; } = false;

    /// <summary>
    /// Mentor's response to the review
    /// </summary>
    public string? MentorResponse { get; set; }

    /// <summary>
    /// When the mentor responded
    /// </summary>
    public DateTime? MentorRespondedAt { get; set; }

    // ============================================
    // TIMESTAMPS
    // ============================================

    /// <summary>
    /// When the review was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// When the review was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}






