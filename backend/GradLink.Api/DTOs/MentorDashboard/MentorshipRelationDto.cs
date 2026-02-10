namespace GradLink.Api.DTOs.MentorDashboard;

// ============================================
// MENTORSHIP RELATION DTOs
// ============================================

public record MentorshipRelationDto
{
    public int Id { get; init; }
    public string MentorId { get; init; } = string.Empty;
    public string MentorName { get; init; } = string.Empty;
    public string? MentorProfilePicture { get; init; }
    public string MenteeId { get; init; } = string.Empty;
    public string MenteeName { get; init; } = string.Empty;
    public string? MenteeProfilePicture { get; init; }
    public string? MenteeUniversity { get; init; }
    public string? MenteeMajor { get; init; }
    public string Status { get; init; } = "Pending";
    public string? RequestMessage { get; init; }
    public string? MenteeGoals { get; init; }
    public int ProgressPercentage { get; init; }
    public int SessionsCompleted { get; init; }
    public decimal TotalHours { get; init; }
    public DateTime RequestedAt { get; init; }
    public DateTime? StartedAt { get; init; }
    public DateTime? EndedAt { get; init; }
    public DateTime? LastActivityAt { get; init; }
}

public record CreateMentorshipRequestDto
{
    public required string MentorId { get; init; }
    public string? RequestMessage { get; init; }
    public string? MenteeGoals { get; init; }
    public string? PreferredCommunication { get; init; }
    public string? ExpectedDuration { get; init; }
}

public record RespondToMentorshipRequestDto
{
    public required string Status { get; init; } // Accepted, Rejected
    public string? MentorResponse { get; init; }
    public string? GuidancePlan { get; init; }
    public string? StatusReason { get; init; }
}

public record UpdateMentorshipProgressDto
{
    public int ProgressPercentage { get; init; }
    public string? ProgressNotes { get; init; }
}

public record CompleteMentorshipDto
{
    public string? MentorFeedback { get; init; }
}

public record MenteeFeedbackDto
{
    public int? Rating { get; init; }
    public string? Feedback { get; init; }
}

// ============================================
// MENTORING SESSION DTOs
// ============================================

public record MentoringSessionDto
{
    public int Id { get; init; }
    public string MentorId { get; init; } = string.Empty;
    public string MentorName { get; init; } = string.Empty;
    public string MenteeId { get; init; } = string.Empty;
    public string MenteeName { get; init; } = string.Empty;
    public string? MenteeProfilePicture { get; init; }
    public int? MentorshipRelationId { get; init; }
    public string Topic { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTime ScheduledDate { get; init; }
    public TimeSpan StartTime { get; init; }
    public int DurationMinutes { get; init; }
    public string SessionType { get; init; } = "Online";
    public string? MeetingLink { get; init; }
    public string? MeetingPlatform { get; init; }
    public string? Location { get; init; }
    public string Status { get; init; } = "Scheduled";
    public string? MentorNotes { get; init; }
    public string? ActionItems { get; init; }
    public int? MenteeRating { get; init; }
    public string? MenteeFeedback { get; init; }
    public DateTime CreatedAt { get; init; }
}

public record CreateSessionDto
{
    public required string MenteeId { get; init; }
    public int? MentorshipRelationId { get; init; }
    public required string Topic { get; init; }
    public string? Description { get; init; }
    public required DateTime ScheduledDate { get; init; }
    public required TimeSpan StartTime { get; init; }
    public int DurationMinutes { get; init; } = 60;
    public string SessionType { get; init; } = "Online";
    public string? MeetingLink { get; init; }
    public string? MeetingPlatform { get; init; }
    public string? Location { get; init; }
    public string? AgendaNotes { get; init; }
}

public record UpdateSessionDto
{
    public string? Topic { get; init; }
    public string? Description { get; init; }
    public DateTime? ScheduledDate { get; init; }
    public TimeSpan? StartTime { get; init; }
    public int? DurationMinutes { get; init; }
    public string? SessionType { get; init; }
    public string? MeetingLink { get; init; }
    public string? Location { get; init; }
    public string? AgendaNotes { get; init; }
}

public record CompleteSessionDto
{
    public string? MentorNotes { get; init; }
    public string? ActionItems { get; init; }
    public int? ActualDurationMinutes { get; init; }
}

public record CancelSessionDto
{
    public required string Reason { get; init; }
}

public record RateSessionDto
{
    public required int Rating { get; init; }
    public string? Feedback { get; init; }
    public bool? WasHelpful { get; init; }
}

// ============================================
// MENTOR REVIEW DTOs
// ============================================

public record MentorReviewDto
{
    public int Id { get; init; }
    public string MentorId { get; init; } = string.Empty;
    public string MentorName { get; init; } = string.Empty;
    public string ReviewerId { get; init; } = string.Empty;
    public string ReviewerName { get; init; } = string.Empty;
    public string? ReviewerProfilePicture { get; init; }
    public int Rating { get; init; }
    public int? KnowledgeRating { get; init; }
    public int? CommunicationRating { get; init; }
    public int? AvailabilityRating { get; init; }
    public int? HelpfulnessRating { get; init; }
    public string? Title { get; init; }
    public string? Comment { get; init; }
    public string? Pros { get; init; }
    public string? Cons { get; init; }
    public bool WouldRecommend { get; init; }
    public string? MentorshipType { get; init; }
    public bool IsVerified { get; init; }
    public int HelpfulCount { get; init; }
    public bool HasMentorResponse { get; init; }
    public string? MentorResponse { get; init; }
    public DateTime CreatedAt { get; init; }
}

public record CreateReviewDto
{
    public required string MentorId { get; init; }
    public int? MentorshipRelationId { get; init; }
    public int? MentoringSessionId { get; init; }
    public required int Rating { get; init; }
    public int? KnowledgeRating { get; init; }
    public int? CommunicationRating { get; init; }
    public int? AvailabilityRating { get; init; }
    public int? HelpfulnessRating { get; init; }
    public string? Title { get; init; }
    public string? Comment { get; init; }
    public string? Pros { get; init; }
    public string? Cons { get; init; }
    public bool WouldRecommend { get; init; } = true;
    public string? MentorshipType { get; init; }
    public bool ShowReviewerName { get; init; } = true;
}

public record MentorResponseDto
{
    public required string Response { get; init; }
}

// ============================================
// MENTOR PROFILE DTOs
// ============================================

public record MentorProfileDto
{
    public string Id { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string? ProfilePicture { get; init; }
    public string? Bio { get; init; }
    public string? JobTitle { get; init; }
    public string? Company { get; init; }
    public string? Specialization { get; init; }
    public int? ExperienceYears { get; init; }
    public string? Skills { get; init; }
    public string? Location { get; init; }
    public string? LinkedInUrl { get; init; }
    public string? GitHubUrl { get; init; }
    public string? WebsiteUrl { get; init; }
    
    // From MentorProfile entity
    public string? ExpertiseAreas { get; init; }
    public string? MentoringStyle { get; init; }
    public int? MaxMentees { get; init; }
    public int CurrentMenteesCount { get; init; }
    public bool IsAcceptingMentees { get; init; }
    public string? AvailableTimeSlots { get; init; }
    public string? Timezone { get; init; }
    public int SessionDurationMinutes { get; init; }
    public bool IsFree { get; init; }
    public decimal? HourlyRate { get; init; }
    public string? Currency { get; init; }
    public int TotalSessions { get; init; }
    public int TotalStudentsMentored { get; init; }
    public decimal? AverageRating { get; init; }
    public int ReviewsCount { get; init; }
    public bool IsVerified { get; init; }
}

public record UpdateMentorProfileDto
{
    public string? ExpertiseAreas { get; init; }
    public string? MentoringStyle { get; init; }
    public int? MaxMentees { get; init; }
    public bool? IsAcceptingMentees { get; init; }
    public string? PreferredCommunication { get; init; }
    public string? AvailableTimeSlots { get; init; }
    public string? Timezone { get; init; }
    public int? SessionDurationMinutes { get; init; }
    public bool? IsFree { get; init; }
    public decimal? HourlyRate { get; init; }
    public string? Currency { get; init; }
}

// ============================================
// DASHBOARD OVERVIEW DTOs
// ============================================

public record MentorDashboardOverviewDto
{
    public int TotalMentees { get; init; }
    public int ActiveMentees { get; init; }
    public int PendingRequests { get; init; }
    public int UpcomingSessions { get; init; }
    public int CompletedSessions { get; init; }
    public int UnreadMessages { get; init; }
    public decimal AverageProgress { get; init; }
    public decimal? AverageRating { get; init; }
    public int TotalReviews { get; init; }
    public List<MentoringSessionDto> UpcomingSessionsList { get; init; } = new();
    public List<RecentActivityDto> RecentActivities { get; init; } = new();
}

public record RecentActivityDto
{
    public string Type { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime Timestamp { get; init; }
    public string? RelatedUserId { get; init; }
    public string? RelatedUserName { get; init; }
}

// ============================================
// LIST & FILTER DTOs
// ============================================

public record MenteeListItemDto
{
    public string Id { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string? ProfilePicture { get; init; }
    public string? University { get; init; }
    public string? Major { get; init; }
    public string? AcademicYear { get; init; }
    public int ProgressPercentage { get; init; }
    public int SessionsCompleted { get; init; }
    public DateTime? LastActivityAt { get; init; }
    public string Status { get; init; } = "Active";
}

public record SessionFilterDto
{
    public string? Status { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
    public string? MenteeId { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}






