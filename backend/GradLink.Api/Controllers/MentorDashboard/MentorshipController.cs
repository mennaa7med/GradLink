using GradLink.Api.DTOs.MentorDashboard;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Controllers.MentorDashboard;

/// <summary>
/// Controller for managing mentorship relations between mentors and students
/// </summary>
[ApiController]
[Route("api/mentorship")]
[Authorize]
public class MentorshipController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public MentorshipController(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    /// Get all mentees for the current mentor
    /// </summary>
    [HttpGet("my-mentees")]
    public async Task<ActionResult<List<MentorshipRelationDto>>> GetMyMentees([FromQuery] string? status = null)
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

        var relations = await query
            .OrderByDescending(mr => mr.RequestedAt)
            .Select(mr => new MentorshipRelationDto
            {
                Id = mr.Id,
                MentorId = mr.MentorId,
                MentorName = mr.Mentor != null ? mr.Mentor.FullName ?? "" : "",
                MenteeId = mr.MenteeId,
                MenteeName = mr.Mentee != null ? mr.Mentee.FullName ?? "" : "",
                MenteeProfilePicture = mr.Mentee != null ? mr.Mentee.ProfilePicture : null,
                MenteeUniversity = mr.Mentee != null ? mr.Mentee.University : null,
                MenteeMajor = mr.Mentee != null ? mr.Mentee.Major : null,
                Status = mr.Status,
                RequestMessage = mr.RequestMessage,
                MenteeGoals = mr.MenteeGoals,
                ProgressPercentage = mr.ProgressPercentage,
                SessionsCompleted = mr.SessionsCompleted,
                TotalHours = mr.TotalHours,
                RequestedAt = mr.RequestedAt,
                StartedAt = mr.StartedAt,
                EndedAt = mr.EndedAt,
                LastActivityAt = mr.LastActivityAt
            })
            .ToListAsync();

        return Ok(relations);
    }

    /// <summary>
    /// Get all mentors for the current student
    /// </summary>
    [HttpGet("my-mentors")]
    public async Task<ActionResult<List<MentorshipRelationDto>>> GetMyMentors([FromQuery] string? status = null)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var query = _context.MentorshipRelations
            .Include(mr => mr.Mentor)
            .Where(mr => mr.MenteeId == userId);

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(mr => mr.Status == status);
        }

        var relations = await query
            .OrderByDescending(mr => mr.RequestedAt)
            .Select(mr => new MentorshipRelationDto
            {
                Id = mr.Id,
                MentorId = mr.MentorId,
                MentorName = mr.Mentor != null ? mr.Mentor.FullName ?? "" : "",
                MentorProfilePicture = mr.Mentor != null ? mr.Mentor.ProfilePicture : null,
                MenteeId = mr.MenteeId,
                MenteeName = mr.Mentee != null ? mr.Mentee.FullName ?? "" : "",
                Status = mr.Status,
                RequestMessage = mr.RequestMessage,
                MenteeGoals = mr.MenteeGoals,
                ProgressPercentage = mr.ProgressPercentage,
                SessionsCompleted = mr.SessionsCompleted,
                TotalHours = mr.TotalHours,
                RequestedAt = mr.RequestedAt,
                StartedAt = mr.StartedAt,
                EndedAt = mr.EndedAt,
                LastActivityAt = mr.LastActivityAt
            })
            .ToListAsync();

        return Ok(relations);
    }

    /// <summary>
    /// Get pending mentorship requests for the current mentor
    /// </summary>
    [HttpGet("pending-requests")]
    public async Task<ActionResult<List<MentorshipRelationDto>>> GetPendingRequests()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var relations = await _context.MentorshipRelations
            .Include(mr => mr.Mentee)
            .Where(mr => mr.MentorId == userId && mr.Status == "Pending")
            .OrderByDescending(mr => mr.RequestedAt)
            .Select(mr => new MentorshipRelationDto
            {
                Id = mr.Id,
                MentorId = mr.MentorId,
                MenteeId = mr.MenteeId,
                MenteeName = mr.Mentee != null ? mr.Mentee.FullName ?? "" : "",
                MenteeProfilePicture = mr.Mentee != null ? mr.Mentee.ProfilePicture : null,
                MenteeUniversity = mr.Mentee != null ? mr.Mentee.University : null,
                MenteeMajor = mr.Mentee != null ? mr.Mentee.Major : null,
                Status = mr.Status,
                RequestMessage = mr.RequestMessage,
                MenteeGoals = mr.MenteeGoals,
                RequestedAt = mr.RequestedAt
            })
            .ToListAsync();

        return Ok(relations);
    }

    /// <summary>
    /// Request mentorship from a mentor (student action)
    /// </summary>
    [HttpPost("request")]
    public async Task<ActionResult<MentorshipRelationDto>> RequestMentorship([FromBody] CreateMentorshipRequestDto dto)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        // Check if mentor exists
        var mentor = await _userManager.FindByIdAsync(dto.MentorId);
        if (mentor == null) return NotFound("Mentor not found");

        // Check if relation already exists
        var existingRelation = await _context.MentorshipRelations
            .FirstOrDefaultAsync(mr => mr.MentorId == dto.MentorId && mr.MenteeId == userId);
        
        if (existingRelation != null)
        {
            return BadRequest("You already have a mentorship relation with this mentor");
        }

        var relation = new MentorshipRelation
        {
            MentorId = dto.MentorId,
            MenteeId = userId,
            RequestMessage = dto.RequestMessage,
            MenteeGoals = dto.MenteeGoals,
            PreferredCommunication = dto.PreferredCommunication,
            ExpectedDuration = dto.ExpectedDuration,
            Status = "Pending",
            RequestedAt = DateTime.UtcNow
        };

        _context.MentorshipRelations.Add(relation);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMentorshipById), new { id = relation.Id }, 
            new MentorshipRelationDto
            {
                Id = relation.Id,
                MentorId = relation.MentorId,
                MenteeId = relation.MenteeId,
                Status = relation.Status,
                RequestMessage = relation.RequestMessage,
                MenteeGoals = relation.MenteeGoals,
                RequestedAt = relation.RequestedAt
            });
    }

    /// <summary>
    /// Respond to a mentorship request (mentor action)
    /// </summary>
    [HttpPost("{id}/respond")]
    public async Task<ActionResult<MentorshipRelationDto>> RespondToRequest(int id, [FromBody] RespondToMentorshipRequestDto dto)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var relation = await _context.MentorshipRelations
            .Include(mr => mr.Mentee)
            .FirstOrDefaultAsync(mr => mr.Id == id && mr.MentorId == userId);

        if (relation == null) return NotFound();

        if (relation.Status != "Pending")
        {
            return BadRequest("This request has already been responded to");
        }

        relation.Status = dto.Status == "Accepted" ? "Active" : "Rejected";
        relation.MentorResponse = dto.MentorResponse;
        relation.GuidancePlan = dto.GuidancePlan;
        relation.StatusReason = dto.StatusReason;
        relation.RespondedAt = DateTime.UtcNow;
        
        if (dto.Status == "Accepted")
        {
            relation.StartedAt = DateTime.UtcNow;
        }

        relation.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(new MentorshipRelationDto
        {
            Id = relation.Id,
            MentorId = relation.MentorId,
            MenteeId = relation.MenteeId,
            MenteeName = relation.Mentee?.FullName ?? "",
            Status = relation.Status,
            RequestMessage = relation.RequestMessage,
            MenteeGoals = relation.MenteeGoals,
            RequestedAt = relation.RequestedAt,
            StartedAt = relation.StartedAt
        });
    }

    /// <summary>
    /// Get a specific mentorship relation
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<MentorshipRelationDto>> GetMentorshipById(int id)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var relation = await _context.MentorshipRelations
            .Include(mr => mr.Mentor)
            .Include(mr => mr.Mentee)
            .FirstOrDefaultAsync(mr => mr.Id == id && (mr.MentorId == userId || mr.MenteeId == userId));

        if (relation == null) return NotFound();

        return Ok(new MentorshipRelationDto
        {
            Id = relation.Id,
            MentorId = relation.MentorId,
            MentorName = relation.Mentor?.FullName ?? "",
            MentorProfilePicture = relation.Mentor?.ProfilePicture,
            MenteeId = relation.MenteeId,
            MenteeName = relation.Mentee?.FullName ?? "",
            MenteeProfilePicture = relation.Mentee?.ProfilePicture,
            MenteeUniversity = relation.Mentee?.University,
            MenteeMajor = relation.Mentee?.Major,
            Status = relation.Status,
            RequestMessage = relation.RequestMessage,
            MenteeGoals = relation.MenteeGoals,
            ProgressPercentage = relation.ProgressPercentage,
            SessionsCompleted = relation.SessionsCompleted,
            TotalHours = relation.TotalHours,
            RequestedAt = relation.RequestedAt,
            StartedAt = relation.StartedAt,
            EndedAt = relation.EndedAt,
            LastActivityAt = relation.LastActivityAt
        });
    }

    /// <summary>
    /// Update mentorship progress (mentor action)
    /// </summary>
    [HttpPatch("{id}/progress")]
    public async Task<ActionResult> UpdateProgress(int id, [FromBody] UpdateMentorshipProgressDto dto)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var relation = await _context.MentorshipRelations
            .FirstOrDefaultAsync(mr => mr.Id == id && mr.MentorId == userId);

        if (relation == null) return NotFound();

        relation.ProgressPercentage = dto.ProgressPercentage;
        relation.ProgressNotes = dto.ProgressNotes;
        relation.LastActivityAt = DateTime.UtcNow;
        relation.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Progress updated successfully" });
    }

    /// <summary>
    /// Complete a mentorship (mentor action)
    /// </summary>
    [HttpPost("{id}/complete")]
    public async Task<ActionResult> CompleteMentorship(int id, [FromBody] CompleteMentorshipDto dto)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var relation = await _context.MentorshipRelations
            .FirstOrDefaultAsync(mr => mr.Id == id && mr.MentorId == userId);

        if (relation == null) return NotFound();

        relation.Status = "Completed";
        relation.MentorFeedback = dto.MentorFeedback;
        relation.HasMentorFeedback = !string.IsNullOrEmpty(dto.MentorFeedback);
        relation.EndedAt = DateTime.UtcNow;
        relation.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Mentorship completed successfully" });
    }

    /// <summary>
    /// Add mentee feedback (student action)
    /// </summary>
    [HttpPost("{id}/feedback")]
    public async Task<ActionResult> AddMenteeFeedback(int id, [FromBody] MenteeFeedbackDto dto)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var relation = await _context.MentorshipRelations
            .FirstOrDefaultAsync(mr => mr.Id == id && mr.MenteeId == userId);

        if (relation == null) return NotFound();

        relation.MenteeRating = dto.Rating;
        relation.MenteeFeedback = dto.Feedback;
        relation.HasMenteeFeedback = true;
        relation.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Feedback submitted successfully" });
    }

    /// <summary>
    /// Cancel a mentorship (either party)
    /// </summary>
    [HttpPost("{id}/cancel")]
    public async Task<ActionResult> CancelMentorship(int id, [FromBody] string? reason = null)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var relation = await _context.MentorshipRelations
            .FirstOrDefaultAsync(mr => mr.Id == id && (mr.MentorId == userId || mr.MenteeId == userId));

        if (relation == null) return NotFound();

        relation.Status = "Cancelled";
        relation.StatusReason = reason;
        relation.EndedAt = DateTime.UtcNow;
        relation.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Mentorship cancelled" });
    }
}






