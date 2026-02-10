using GradLink.Api.DTOs.MentorDashboard;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Controllers.MentorDashboard;

/// <summary>
/// Controller for mentor dashboard overview and profile management
/// </summary>
[ApiController]
[Route("api/mentor-dashboard")]
[Authorize]
public class MentorDashboardController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public MentorDashboardController(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    /// Get dashboard overview for the current mentor
    /// </summary>
    [HttpGet("overview")]
    public async Task<ActionResult<MentorDashboardOverviewDto>> GetOverview()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var today = DateTime.UtcNow.Date;
        var nextWeek = today.AddDays(7);

        // Get mentorship relations stats
        var relations = await _context.MentorshipRelations
            .Where(mr => mr.MentorId == userId)
            .ToListAsync();

        var totalMentees = relations.Count(r => r.Status == "Active" || r.Status == "Completed");
        var activeMentees = relations.Count(r => r.Status == "Active");
        var pendingRequests = relations.Count(r => r.Status == "Pending");
        var averageProgress = relations.Where(r => r.Status == "Active").Any()
            ? relations.Where(r => r.Status == "Active").Average(r => r.ProgressPercentage)
            : 0;

        // Get sessions stats
        var upcomingSessions = await _context.MentoringSessions
            .Where(ms => ms.MentorId == userId && ms.Status == "Scheduled" && ms.ScheduledDate >= today)
            .CountAsync();

        var completedSessions = await _context.MentoringSessions
            .Where(ms => ms.MentorId == userId && ms.Status == "Completed")
            .CountAsync();

        // Get upcoming sessions list
        var upcomingSessionsList = await _context.MentoringSessions
            .Include(ms => ms.Mentee)
            .Where(ms => ms.MentorId == userId && ms.Status == "Scheduled" && ms.ScheduledDate >= today && ms.ScheduledDate <= nextWeek)
            .OrderBy(ms => ms.ScheduledDate)
            .ThenBy(ms => ms.StartTime)
            .Take(5)
            .Select(ms => new MentoringSessionDto
            {
                Id = ms.Id,
                MenteeId = ms.MenteeId,
                MenteeName = ms.Mentee != null ? ms.Mentee.FullName ?? "" : "",
                MenteeProfilePicture = ms.Mentee != null ? ms.Mentee.ProfilePicture : null,
                Topic = ms.Topic,
                ScheduledDate = ms.ScheduledDate,
                StartTime = ms.StartTime,
                DurationMinutes = ms.DurationMinutes,
                SessionType = ms.SessionType,
                MeetingLink = ms.MeetingLink,
                Location = ms.Location,
                Status = ms.Status
            })
            .ToListAsync();

        // Get review stats
        var reviews = await _context.MentorReviews
            .Where(mr => mr.MentorId == userId && mr.IsApproved)
            .ToListAsync();

        var averageRating = reviews.Any() ? (decimal?)Math.Round(reviews.Average(r => r.Rating), 1) : null;

        // Get unread messages count (simplified - would need actual unread tracking)
        var unreadMessages = await _context.Conversations
            .Where(c => c.User1Id == userId || c.User2Id == userId)
            .SelectMany(c => c.Messages)
            .Where(m => !m.IsRead && m.SenderId != userId)
            .CountAsync();

        // Build recent activities
        var recentActivities = new List<RecentActivityDto>();

        // Recent mentorship requests
        var recentRequests = await _context.MentorshipRelations
            .Include(mr => mr.Mentee)
            .Where(mr => mr.MentorId == userId && mr.Status == "Pending")
            .OrderByDescending(mr => mr.RequestedAt)
            .Take(3)
            .Select(mr => new RecentActivityDto
            {
                Type = "mentee_request",
                Description = $"New mentorship request from {mr.Mentee!.FullName}",
                Timestamp = mr.RequestedAt,
                RelatedUserId = mr.MenteeId,
                RelatedUserName = mr.Mentee.FullName
            })
            .ToListAsync();
        recentActivities.AddRange(recentRequests);

        // Recent completed sessions
        var recentSessions = await _context.MentoringSessions
            .Include(ms => ms.Mentee)
            .Where(ms => ms.MentorId == userId && ms.Status == "Completed")
            .OrderByDescending(ms => ms.CompletedAt)
            .Take(3)
            .Select(ms => new RecentActivityDto
            {
                Type = "session_completed",
                Description = $"Session with {ms.Mentee!.FullName} completed",
                Timestamp = ms.CompletedAt ?? ms.UpdatedAt ?? DateTime.UtcNow,
                RelatedUserId = ms.MenteeId,
                RelatedUserName = ms.Mentee.FullName
            })
            .ToListAsync();
        recentActivities.AddRange(recentSessions);

        // Recent reviews
        var recentReviews = await _context.MentorReviews
            .Include(mr => mr.Reviewer)
            .Where(mr => mr.MentorId == userId)
            .OrderByDescending(mr => mr.CreatedAt)
            .Take(2)
            .Select(mr => new RecentActivityDto
            {
                Type = "review",
                Description = $"New {mr.Rating}-star review received",
                Timestamp = mr.CreatedAt,
                RelatedUserId = mr.ReviewerId,
                RelatedUserName = mr.ShowReviewerName ? mr.Reviewer!.FullName : "Anonymous"
            })
            .ToListAsync();
        recentActivities.AddRange(recentReviews);

        // Sort by timestamp
        recentActivities = recentActivities.OrderByDescending(a => a.Timestamp).Take(10).ToList();

        return Ok(new MentorDashboardOverviewDto
        {
            TotalMentees = totalMentees,
            ActiveMentees = activeMentees,
            PendingRequests = pendingRequests,
            UpcomingSessions = upcomingSessions,
            CompletedSessions = completedSessions,
            UnreadMessages = unreadMessages,
            AverageProgress = (decimal)averageProgress,
            AverageRating = averageRating,
            TotalReviews = reviews.Count,
            UpcomingSessionsList = upcomingSessionsList,
            RecentActivities = recentActivities
        });
    }

    /// <summary>
    /// Get mentor profile for the current user
    /// </summary>
    [HttpGet("profile")]
    public async Task<ActionResult<MentorProfileDto>> GetMyProfile()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        var mentorProfile = await _context.MentorProfiles
            .FirstOrDefaultAsync(mp => mp.UserId == userId);

        return Ok(new MentorProfileDto
        {
            Id = user.Id,
            FullName = user.FullName ?? "",
            ProfilePicture = user.ProfilePicture,
            Bio = user.Bio,
            JobTitle = user.JobTitle,
            Company = user.Company,
            Specialization = user.Specialization,
            ExperienceYears = user.ExperienceYears,
            Skills = user.Skills,
            Location = user.Location,
            LinkedInUrl = user.LinkedInUrl,
            GitHubUrl = user.GitHubUrl,
            WebsiteUrl = user.WebsiteUrl,
            ExpertiseAreas = mentorProfile?.ExpertiseAreas,
            MentoringStyle = mentorProfile?.MentoringStyle,
            MaxMentees = mentorProfile?.MaxMentees,
            CurrentMenteesCount = mentorProfile?.CurrentMenteesCount ?? 0,
            IsAcceptingMentees = mentorProfile?.IsAcceptingMentees ?? true,
            AvailableTimeSlots = mentorProfile?.AvailableTimeSlots,
            Timezone = mentorProfile?.Timezone,
            SessionDurationMinutes = mentorProfile?.SessionDurationMinutes ?? 60,
            IsFree = mentorProfile?.IsFree ?? true,
            HourlyRate = mentorProfile?.HourlyRate,
            Currency = mentorProfile?.Currency,
            TotalSessions = mentorProfile?.TotalSessions ?? 0,
            TotalStudentsMentored = mentorProfile?.TotalStudentsMentored ?? 0,
            AverageRating = mentorProfile?.AverageRating,
            ReviewsCount = mentorProfile?.ReviewsCount ?? 0,
            IsVerified = mentorProfile?.IsVerified ?? false
        });
    }

    /// <summary>
    /// Get a mentor's public profile
    /// </summary>
    [HttpGet("profile/{mentorId}")]
    [AllowAnonymous]
    public async Task<ActionResult<MentorProfileDto>> GetMentorProfile(string mentorId)
    {
        var user = await _userManager.FindByIdAsync(mentorId);
        if (user == null) return NotFound();

        var mentorProfile = await _context.MentorProfiles
            .FirstOrDefaultAsync(mp => mp.UserId == mentorId);

        return Ok(new MentorProfileDto
        {
            Id = user.Id,
            FullName = user.FullName ?? "",
            ProfilePicture = user.ProfilePicture,
            Bio = user.Bio,
            JobTitle = user.JobTitle,
            Company = user.Company,
            Specialization = user.Specialization,
            ExperienceYears = user.ExperienceYears,
            Skills = user.Skills,
            Location = user.Location,
            LinkedInUrl = user.LinkedInUrl,
            GitHubUrl = user.GitHubUrl,
            WebsiteUrl = user.WebsiteUrl,
            ExpertiseAreas = mentorProfile?.ExpertiseAreas,
            MentoringStyle = mentorProfile?.MentoringStyle,
            MaxMentees = mentorProfile?.MaxMentees,
            CurrentMenteesCount = mentorProfile?.CurrentMenteesCount ?? 0,
            IsAcceptingMentees = mentorProfile?.IsAcceptingMentees ?? true,
            SessionDurationMinutes = mentorProfile?.SessionDurationMinutes ?? 60,
            IsFree = mentorProfile?.IsFree ?? true,
            HourlyRate = mentorProfile?.HourlyRate,
            Currency = mentorProfile?.Currency,
            TotalSessions = mentorProfile?.TotalSessions ?? 0,
            TotalStudentsMentored = mentorProfile?.TotalStudentsMentored ?? 0,
            AverageRating = mentorProfile?.AverageRating,
            ReviewsCount = mentorProfile?.ReviewsCount ?? 0,
            IsVerified = mentorProfile?.IsVerified ?? false
        });
    }

    /// <summary>
    /// Update mentor profile
    /// </summary>
    [HttpPut("profile")]
    public async Task<ActionResult<MentorProfileDto>> UpdateProfile([FromBody] UpdateMentorProfileDto dto)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var mentorProfile = await _context.MentorProfiles
            .FirstOrDefaultAsync(mp => mp.UserId == userId);

        if (mentorProfile == null)
        {
            // Create new mentor profile
            mentorProfile = new MentorProfile
            {
                UserId = userId,
                ExpertiseAreas = dto.ExpertiseAreas,
                MentoringStyle = dto.MentoringStyle,
                MaxMentees = dto.MaxMentees,
                IsAcceptingMentees = dto.IsAcceptingMentees ?? true,
                PreferredCommunication = dto.PreferredCommunication,
                AvailableTimeSlots = dto.AvailableTimeSlots,
                Timezone = dto.Timezone,
                SessionDurationMinutes = dto.SessionDurationMinutes ?? 60,
                IsFree = dto.IsFree ?? true,
                HourlyRate = dto.HourlyRate,
                Currency = dto.Currency,
                CreatedAt = DateTime.UtcNow
            };
            _context.MentorProfiles.Add(mentorProfile);
        }
        else
        {
            // Update existing
            if (dto.ExpertiseAreas != null) mentorProfile.ExpertiseAreas = dto.ExpertiseAreas;
            if (dto.MentoringStyle != null) mentorProfile.MentoringStyle = dto.MentoringStyle;
            if (dto.MaxMentees.HasValue) mentorProfile.MaxMentees = dto.MaxMentees;
            if (dto.IsAcceptingMentees.HasValue) mentorProfile.IsAcceptingMentees = dto.IsAcceptingMentees.Value;
            if (dto.PreferredCommunication != null) mentorProfile.PreferredCommunication = dto.PreferredCommunication;
            if (dto.AvailableTimeSlots != null) mentorProfile.AvailableTimeSlots = dto.AvailableTimeSlots;
            if (dto.Timezone != null) mentorProfile.Timezone = dto.Timezone;
            if (dto.SessionDurationMinutes.HasValue) mentorProfile.SessionDurationMinutes = dto.SessionDurationMinutes.Value;
            if (dto.IsFree.HasValue) mentorProfile.IsFree = dto.IsFree.Value;
            if (dto.HourlyRate.HasValue) mentorProfile.HourlyRate = dto.HourlyRate;
            if (dto.Currency != null) mentorProfile.Currency = dto.Currency;
            mentorProfile.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        return await GetMyProfile();
    }

    /// <summary>
    /// Get all available mentors (for students to browse)
    /// </summary>
    [HttpGet("available-mentors")]
    [AllowAnonymous]
    public async Task<ActionResult<List<MentorProfileDto>>> GetAvailableMentors(
        [FromQuery] string? specialization = null,
        [FromQuery] string? expertise = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 12)
    {
        var query = _context.MentorProfiles
            .Include(mp => mp.User)
            .Where(mp => mp.IsAcceptingMentees);

        if (!string.IsNullOrEmpty(specialization))
        {
            query = query.Where(mp => mp.User != null && mp.User.Specialization != null && 
                mp.User.Specialization.Contains(specialization));
        }

        if (!string.IsNullOrEmpty(expertise))
        {
            query = query.Where(mp => mp.ExpertiseAreas != null && mp.ExpertiseAreas.Contains(expertise));
        }

        var mentors = await query
            .OrderByDescending(mp => mp.AverageRating)
            .ThenByDescending(mp => mp.TotalStudentsMentored)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(mp => new MentorProfileDto
            {
                Id = mp.UserId,
                FullName = mp.User != null ? mp.User.FullName ?? "" : "",
                ProfilePicture = mp.User != null ? mp.User.ProfilePicture : null,
                Bio = mp.User != null ? mp.User.Bio : null,
                JobTitle = mp.User != null ? mp.User.JobTitle : null,
                Company = mp.User != null ? mp.User.Company : null,
                Specialization = mp.User != null ? mp.User.Specialization : null,
                ExperienceYears = mp.User != null ? mp.User.ExperienceYears : null,
                Skills = mp.User != null ? mp.User.Skills : null,
                Location = mp.User != null ? mp.User.Location : null,
                ExpertiseAreas = mp.ExpertiseAreas,
                MentoringStyle = mp.MentoringStyle,
                IsAcceptingMentees = mp.IsAcceptingMentees,
                SessionDurationMinutes = mp.SessionDurationMinutes,
                IsFree = mp.IsFree,
                HourlyRate = mp.HourlyRate,
                TotalStudentsMentored = mp.TotalStudentsMentored,
                AverageRating = mp.AverageRating,
                ReviewsCount = mp.ReviewsCount,
                IsVerified = mp.IsVerified
            })
            .ToListAsync();

        return Ok(mentors);
    }

    /// <summary>
    /// Get list of mentees for the current mentor
    /// </summary>
    [HttpGet("mentees")]
    public async Task<ActionResult<List<MenteeListItemDto>>> GetMenteesList([FromQuery] string? status = "Active")
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var query = _context.MentorshipRelations
            .Include(mr => mr.Mentee)
            .Where(mr => mr.MentorId == userId);

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(mr => mr.Status == status);
        }

        var mentees = await query
            .OrderByDescending(mr => mr.LastActivityAt)
            .Select(mr => new MenteeListItemDto
            {
                Id = mr.MenteeId,
                FullName = mr.Mentee != null ? mr.Mentee.FullName ?? "" : "",
                ProfilePicture = mr.Mentee != null ? mr.Mentee.ProfilePicture : null,
                University = mr.Mentee != null ? mr.Mentee.University : null,
                Major = mr.Mentee != null ? mr.Mentee.Major : null,
                AcademicYear = mr.Mentee != null ? mr.Mentee.AcademicYear : null,
                ProgressPercentage = mr.ProgressPercentage,
                SessionsCompleted = mr.SessionsCompleted,
                LastActivityAt = mr.LastActivityAt,
                Status = mr.Status
            })
            .ToListAsync();

        return Ok(mentees);
    }
}






