using GradLink.Api.DTOs.MentorDashboard;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Controllers.MentorDashboard;

/// <summary>
/// Controller for managing mentoring sessions
/// </summary>
[ApiController]
[Route("api/mentoring-sessions")]
[Authorize]
public class MentoringSessionController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public MentoringSessionController(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    /// Get all sessions for the current mentor
    /// </summary>
    [HttpGet("mentor")]
    public async Task<ActionResult<List<MentoringSessionDto>>> GetMentorSessions([FromQuery] SessionFilterDto filter)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var query = _context.MentoringSessions
            .Include(ms => ms.Mentee)
            .Where(ms => ms.MentorId == userId);

        // Apply filters
        if (!string.IsNullOrEmpty(filter.Status))
        {
            query = query.Where(ms => ms.Status == filter.Status);
        }
        if (filter.FromDate.HasValue)
        {
            query = query.Where(ms => ms.ScheduledDate >= filter.FromDate.Value);
        }
        if (filter.ToDate.HasValue)
        {
            query = query.Where(ms => ms.ScheduledDate <= filter.ToDate.Value);
        }
        if (!string.IsNullOrEmpty(filter.MenteeId))
        {
            query = query.Where(ms => ms.MenteeId == filter.MenteeId);
        }

        var sessions = await query
            .OrderByDescending(ms => ms.ScheduledDate)
            .ThenBy(ms => ms.StartTime)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(ms => new MentoringSessionDto
            {
                Id = ms.Id,
                MentorId = ms.MentorId,
                MentorName = ms.Mentor != null ? ms.Mentor.FullName ?? "" : "",
                MenteeId = ms.MenteeId,
                MenteeName = ms.Mentee != null ? ms.Mentee.FullName ?? "" : "",
                MenteeProfilePicture = ms.Mentee != null ? ms.Mentee.ProfilePicture : null,
                MentorshipRelationId = ms.MentorshipRelationId,
                Topic = ms.Topic,
                Description = ms.Description,
                ScheduledDate = ms.ScheduledDate,
                StartTime = ms.StartTime,
                DurationMinutes = ms.DurationMinutes,
                SessionType = ms.SessionType,
                MeetingLink = ms.MeetingLink,
                MeetingPlatform = ms.MeetingPlatform,
                Location = ms.Location,
                Status = ms.Status,
                MentorNotes = ms.MentorNotes,
                ActionItems = ms.ActionItems,
                MenteeRating = ms.MenteeRating,
                MenteeFeedback = ms.MenteeFeedback,
                CreatedAt = ms.CreatedAt
            })
            .ToListAsync();

        return Ok(sessions);
    }

    /// <summary>
    /// Get all sessions for the current student
    /// </summary>
    [HttpGet("mentee")]
    public async Task<ActionResult<List<MentoringSessionDto>>> GetMenteeSessions([FromQuery] SessionFilterDto filter)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var query = _context.MentoringSessions
            .Include(ms => ms.Mentor)
            .Where(ms => ms.MenteeId == userId);

        // Apply filters
        if (!string.IsNullOrEmpty(filter.Status))
        {
            query = query.Where(ms => ms.Status == filter.Status);
        }
        if (filter.FromDate.HasValue)
        {
            query = query.Where(ms => ms.ScheduledDate >= filter.FromDate.Value);
        }
        if (filter.ToDate.HasValue)
        {
            query = query.Where(ms => ms.ScheduledDate <= filter.ToDate.Value);
        }

        var sessions = await query
            .OrderByDescending(ms => ms.ScheduledDate)
            .ThenBy(ms => ms.StartTime)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(ms => new MentoringSessionDto
            {
                Id = ms.Id,
                MentorId = ms.MentorId,
                MentorName = ms.Mentor != null ? ms.Mentor.FullName ?? "" : "",
                MenteeId = ms.MenteeId,
                MenteeName = ms.Mentee != null ? ms.Mentee.FullName ?? "" : "",
                Topic = ms.Topic,
                Description = ms.Description,
                ScheduledDate = ms.ScheduledDate,
                StartTime = ms.StartTime,
                DurationMinutes = ms.DurationMinutes,
                SessionType = ms.SessionType,
                MeetingLink = ms.MeetingLink,
                Location = ms.Location,
                Status = ms.Status,
                CreatedAt = ms.CreatedAt
            })
            .ToListAsync();

        return Ok(sessions);
    }

    /// <summary>
    /// Get upcoming sessions for the current mentor (next 7 days)
    /// </summary>
    [HttpGet("upcoming")]
    public async Task<ActionResult<List<MentoringSessionDto>>> GetUpcomingSessions()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var today = DateTime.UtcNow.Date;
        var nextWeek = today.AddDays(7);

        var sessions = await _context.MentoringSessions
            .Include(ms => ms.Mentee)
            .Where(ms => ms.MentorId == userId 
                && ms.Status == "Scheduled" 
                && ms.ScheduledDate >= today 
                && ms.ScheduledDate <= nextWeek)
            .OrderBy(ms => ms.ScheduledDate)
            .ThenBy(ms => ms.StartTime)
            .Take(10)
            .Select(ms => new MentoringSessionDto
            {
                Id = ms.Id,
                MentorId = ms.MentorId,
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

        return Ok(sessions);
    }

    /// <summary>
    /// Get a specific session
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<MentoringSessionDto>> GetSession(int id)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var session = await _context.MentoringSessions
            .Include(ms => ms.Mentor)
            .Include(ms => ms.Mentee)
            .FirstOrDefaultAsync(ms => ms.Id == id && (ms.MentorId == userId || ms.MenteeId == userId));

        if (session == null) return NotFound();

        return Ok(new MentoringSessionDto
        {
            Id = session.Id,
            MentorId = session.MentorId,
            MentorName = session.Mentor?.FullName ?? "",
            MenteeId = session.MenteeId,
            MenteeName = session.Mentee?.FullName ?? "",
            MenteeProfilePicture = session.Mentee?.ProfilePicture,
            MentorshipRelationId = session.MentorshipRelationId,
            Topic = session.Topic,
            Description = session.Description,
            ScheduledDate = session.ScheduledDate,
            StartTime = session.StartTime,
            DurationMinutes = session.DurationMinutes,
            SessionType = session.SessionType,
            MeetingLink = session.MeetingLink,
            MeetingPlatform = session.MeetingPlatform,
            Location = session.Location,
            Status = session.Status,
            MentorNotes = session.MentorNotes,
            ActionItems = session.ActionItems,
            MenteeRating = session.MenteeRating,
            MenteeFeedback = session.MenteeFeedback,
            CreatedAt = session.CreatedAt
        });
    }

    /// <summary>
    /// Create a new session (mentor action)
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<MentoringSessionDto>> CreateSession([FromBody] CreateSessionDto dto)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        // Verify mentee exists
        var mentee = await _userManager.FindByIdAsync(dto.MenteeId);
        if (mentee == null) return NotFound("Mentee not found");

        var session = new MentoringSession
        {
            MentorId = userId,
            MenteeId = dto.MenteeId,
            MentorshipRelationId = dto.MentorshipRelationId,
            Topic = dto.Topic,
            Description = dto.Description,
            ScheduledDate = dto.ScheduledDate,
            StartTime = dto.StartTime,
            DurationMinutes = dto.DurationMinutes,
            SessionType = dto.SessionType,
            MeetingLink = dto.MeetingLink,
            MeetingPlatform = dto.MeetingPlatform,
            Location = dto.Location,
            AgendaNotes = dto.AgendaNotes,
            Status = "Scheduled",
            CreatedAt = DateTime.UtcNow
        };

        _context.MentoringSessions.Add(session);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSession), new { id = session.Id },
            new MentoringSessionDto
            {
                Id = session.Id,
                MentorId = session.MentorId,
                MenteeId = session.MenteeId,
                MenteeName = mentee.FullName ?? "",
                Topic = session.Topic,
                ScheduledDate = session.ScheduledDate,
                StartTime = session.StartTime,
                DurationMinutes = session.DurationMinutes,
                SessionType = session.SessionType,
                MeetingLink = session.MeetingLink,
                Location = session.Location,
                Status = session.Status,
                CreatedAt = session.CreatedAt
            });
    }

    /// <summary>
    /// Update a session (mentor action)
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<MentoringSessionDto>> UpdateSession(int id, [FromBody] UpdateSessionDto dto)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var session = await _context.MentoringSessions
            .Include(ms => ms.Mentee)
            .FirstOrDefaultAsync(ms => ms.Id == id && ms.MentorId == userId);

        if (session == null) return NotFound();

        if (session.Status != "Scheduled")
        {
            return BadRequest("Cannot update a session that is not scheduled");
        }

        // Update only provided fields
        if (dto.Topic != null) session.Topic = dto.Topic;
        if (dto.Description != null) session.Description = dto.Description;
        if (dto.ScheduledDate.HasValue) session.ScheduledDate = dto.ScheduledDate.Value;
        if (dto.StartTime.HasValue) session.StartTime = dto.StartTime.Value;
        if (dto.DurationMinutes.HasValue) session.DurationMinutes = dto.DurationMinutes.Value;
        if (dto.SessionType != null) session.SessionType = dto.SessionType;
        if (dto.MeetingLink != null) session.MeetingLink = dto.MeetingLink;
        if (dto.Location != null) session.Location = dto.Location;
        if (dto.AgendaNotes != null) session.AgendaNotes = dto.AgendaNotes;
        
        session.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new MentoringSessionDto
        {
            Id = session.Id,
            MentorId = session.MentorId,
            MenteeId = session.MenteeId,
            MenteeName = session.Mentee?.FullName ?? "",
            Topic = session.Topic,
            ScheduledDate = session.ScheduledDate,
            StartTime = session.StartTime,
            DurationMinutes = session.DurationMinutes,
            SessionType = session.SessionType,
            MeetingLink = session.MeetingLink,
            Location = session.Location,
            Status = session.Status
        });
    }

    /// <summary>
    /// Mark session as completed (mentor action)
    /// </summary>
    [HttpPost("{id}/complete")]
    public async Task<ActionResult> CompleteSession(int id, [FromBody] CompleteSessionDto dto)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var session = await _context.MentoringSessions
            .FirstOrDefaultAsync(ms => ms.Id == id && ms.MentorId == userId);

        if (session == null) return NotFound();

        session.Status = "Completed";
        session.MentorNotes = dto.MentorNotes;
        session.ActionItems = dto.ActionItems;
        session.ActualDurationMinutes = dto.ActualDurationMinutes ?? session.DurationMinutes;
        session.CompletedAt = DateTime.UtcNow;
        session.ActualEndTime = DateTime.UtcNow;
        session.UpdatedAt = DateTime.UtcNow;

        // Update mentorship relation stats if linked
        if (session.MentorshipRelationId.HasValue)
        {
            var relation = await _context.MentorshipRelations
                .FirstOrDefaultAsync(mr => mr.Id == session.MentorshipRelationId);
            
            if (relation != null)
            {
                relation.SessionsCompleted++;
                relation.TotalHours += (decimal)(session.ActualDurationMinutes ?? session.DurationMinutes) / 60;
                relation.LastActivityAt = DateTime.UtcNow;
                relation.UpdatedAt = DateTime.UtcNow;
            }
        }

        await _context.SaveChangesAsync();

        return Ok(new { message = "Session completed successfully" });
    }

    /// <summary>
    /// Cancel a session (either party)
    /// </summary>
    [HttpPost("{id}/cancel")]
    public async Task<ActionResult> CancelSession(int id, [FromBody] CancelSessionDto dto)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var session = await _context.MentoringSessions
            .FirstOrDefaultAsync(ms => ms.Id == id && (ms.MentorId == userId || ms.MenteeId == userId));

        if (session == null) return NotFound();

        if (session.Status != "Scheduled")
        {
            return BadRequest("Cannot cancel a session that is not scheduled");
        }

        session.Status = "Cancelled";
        session.StatusReason = dto.Reason;
        session.CancelledBy = session.MentorId == userId ? "Mentor" : "Mentee";
        session.CancelledAt = DateTime.UtcNow;
        session.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Session cancelled successfully" });
    }

    /// <summary>
    /// Rate a session (mentee action)
    /// </summary>
    [HttpPost("{id}/rate")]
    public async Task<ActionResult> RateSession(int id, [FromBody] RateSessionDto dto)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var session = await _context.MentoringSessions
            .FirstOrDefaultAsync(ms => ms.Id == id && ms.MenteeId == userId);

        if (session == null) return NotFound();

        if (session.Status != "Completed")
        {
            return BadRequest("Can only rate completed sessions");
        }

        session.MenteeRating = dto.Rating;
        session.MenteeFeedback = dto.Feedback;
        session.WasHelpful = dto.WasHelpful;
        session.HasMenteeRating = true;
        session.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Session rated successfully" });
    }

    /// <summary>
    /// Delete a session (mentor action, only if scheduled)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSession(int id)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var session = await _context.MentoringSessions
            .FirstOrDefaultAsync(ms => ms.Id == id && ms.MentorId == userId);

        if (session == null) return NotFound();

        if (session.Status != "Scheduled")
        {
            return BadRequest("Cannot delete a session that is not scheduled");
        }

        _context.MentoringSessions.Remove(session);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Session deleted successfully" });
    }
}






