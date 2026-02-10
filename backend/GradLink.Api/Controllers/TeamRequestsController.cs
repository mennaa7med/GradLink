using GradLink.Application.DTOs.TeamRequests;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamRequestsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TeamRequestsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/TeamRequests
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamRequestDto>>> GetTeamRequests(
        [FromQuery] string? category = null,
        [FromQuery] string? skill = null,
        [FromQuery] string? search = null)
    {
        var query = _context.TeamRequests
            .Include(tr => tr.Owner)
            .Where(tr => tr.Status == "Active")
            .AsQueryable();

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(tr => tr.Category == category);
        }

        if (!string.IsNullOrEmpty(skill))
        {
            query = query.Where(tr => tr.SkillsNeeded.Contains(skill));
        }

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(tr => 
                tr.ProjectName.Contains(search) || 
                tr.Description.Contains(search) ||
                tr.SkillsNeeded.Contains(search));
        }

        var rawData = await query
            .OrderByDescending(tr => tr.CreatedAt)
            .ToListAsync();

        var teamRequests = rawData.Select(tr => new TeamRequestDto
        {
            Id = tr.Id,
            ProjectName = tr.ProjectName,
            Description = tr.Description,
            Category = tr.Category,
            SkillsNeeded = tr.SkillsNeeded.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList(),
            MembersNeeded = tr.MembersNeeded,
            CurrentMembers = tr.CurrentMembers,
            OwnerId = tr.OwnerId,
            OwnerName = tr.Owner != null ? tr.Owner.FullName : "Unknown",
            OwnerAvatar = tr.Owner != null && !string.IsNullOrEmpty(tr.Owner.ProfilePicture) ? tr.Owner.ProfilePicture : "👨‍💻",
            ContactEmail = tr.ContactEmail,
            Status = tr.Status,
            CreatedAt = tr.CreatedAt,
            PostedDate = GetRelativeTime(tr.CreatedAt)
        }).ToList();

        return Ok(teamRequests);
    }

    // GET: api/TeamRequests/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TeamRequestDto>> GetTeamRequest(int id)
    {
        var teamRequest = await _context.TeamRequests
            .Include(tr => tr.Owner)
            .FirstOrDefaultAsync(tr => tr.Id == id);

        if (teamRequest == null)
        {
            return NotFound();
        }

        return Ok(new TeamRequestDto
        {
            Id = teamRequest.Id,
            ProjectName = teamRequest.ProjectName,
            Description = teamRequest.Description,
            Category = teamRequest.Category,
            SkillsNeeded = teamRequest.SkillsNeeded.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList(),
            MembersNeeded = teamRequest.MembersNeeded,
            CurrentMembers = teamRequest.CurrentMembers,
            OwnerId = teamRequest.OwnerId,
            OwnerName = teamRequest.Owner != null ? teamRequest.Owner.FullName : "Unknown",
            OwnerAvatar = teamRequest.Owner != null && !string.IsNullOrEmpty(teamRequest.Owner.ProfilePicture) ? teamRequest.Owner.ProfilePicture : "👨‍💻",
            ContactEmail = teamRequest.ContactEmail,
            Status = teamRequest.Status,
            CreatedAt = teamRequest.CreatedAt,
            PostedDate = GetRelativeTime(teamRequest.CreatedAt)
        });
    }

    // GET: api/TeamRequests/my
    [Authorize]
    [HttpGet("my")]
    public async Task<ActionResult<IEnumerable<TeamRequestDto>>> GetMyTeamRequests()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var rawData = await _context.TeamRequests
            .Include(tr => tr.Owner)
            .Where(tr => tr.OwnerId == userId)
            .OrderByDescending(tr => tr.CreatedAt)
            .ToListAsync();

        var teamRequests = rawData.Select(tr => new TeamRequestDto
        {
            Id = tr.Id,
            ProjectName = tr.ProjectName,
            Description = tr.Description,
            Category = tr.Category,
            SkillsNeeded = tr.SkillsNeeded.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList(),
            MembersNeeded = tr.MembersNeeded,
            CurrentMembers = tr.CurrentMembers,
            OwnerId = tr.OwnerId,
            OwnerName = tr.Owner != null ? tr.Owner.FullName : "Unknown",
            ContactEmail = tr.ContactEmail,
            Status = tr.Status,
            CreatedAt = tr.CreatedAt,
            PostedDate = GetRelativeTime(tr.CreatedAt)
        }).ToList();

        return Ok(teamRequests);
    }

    // POST: api/TeamRequests
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<TeamRequestDto>> CreateTeamRequest([FromBody] CreateTeamRequestDto request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;

        var teamRequest = new TeamRequest
        {
            ProjectName = request.ProjectName,
            Description = request.Description,
            Category = request.Category,
            SkillsNeeded = request.SkillsNeeded,
            MembersNeeded = request.MembersNeeded,
            ContactEmail = request.ContactEmail,
            OwnerId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.TeamRequests.Add(teamRequest);
        await _context.SaveChangesAsync();

        // Load owner for response
        await _context.Entry(teamRequest).Reference(tr => tr.Owner).LoadAsync();

        var response = new TeamRequestDto
        {
            Id = teamRequest.Id,
            ProjectName = teamRequest.ProjectName,
            Description = teamRequest.Description,
            Category = teamRequest.Category,
            SkillsNeeded = teamRequest.SkillsNeeded.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList(),
            MembersNeeded = teamRequest.MembersNeeded,
            CurrentMembers = teamRequest.CurrentMembers,
            OwnerId = teamRequest.OwnerId,
            OwnerName = teamRequest.Owner != null ? teamRequest.Owner.FullName : "Unknown",
            ContactEmail = teamRequest.ContactEmail,
            Status = teamRequest.Status,
            CreatedAt = teamRequest.CreatedAt,
            PostedDate = "Just now"
        };

        return CreatedAtAction(nameof(GetTeamRequest), new { id = teamRequest.Id }, response);
    }

    // PUT: api/TeamRequests/5
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTeamRequest(int id, [FromBody] UpdateTeamRequestDto request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var teamRequest = await _context.TeamRequests
            .FirstOrDefaultAsync(tr => tr.Id == id && tr.OwnerId == userId);

        if (teamRequest == null)
        {
            return NotFound();
        }

        if (request.ProjectName != null) teamRequest.ProjectName = request.ProjectName;
        if (request.Description != null) teamRequest.Description = request.Description;
        if (request.Category != null) teamRequest.Category = request.Category;
        if (request.SkillsNeeded != null) teamRequest.SkillsNeeded = request.SkillsNeeded;
        if (request.MembersNeeded.HasValue) teamRequest.MembersNeeded = request.MembersNeeded.Value;
        if (request.CurrentMembers.HasValue) teamRequest.CurrentMembers = request.CurrentMembers.Value;
        if (request.Status != null) teamRequest.Status = request.Status;
        teamRequest.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/TeamRequests/5
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeamRequest(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var teamRequest = await _context.TeamRequests
            .FirstOrDefaultAsync(tr => tr.Id == id && tr.OwnerId == userId);

        if (teamRequest == null)
        {
            return NotFound();
        }

        _context.TeamRequests.Remove(teamRequest);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // ============================================
    // JOIN REQUESTS
    // ============================================

    // POST: api/TeamRequests/5/join
    [Authorize]
    [HttpPost("{teamRequestId}/join")]
    public async Task<ActionResult<JoinRequestDto>> CreateJoinRequest(int teamRequestId, [FromBody] CreateJoinRequestDto request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;

        var teamRequest = await _context.TeamRequests.FindAsync(teamRequestId);
        if (teamRequest == null)
        {
            return NotFound("Team request not found");
        }

        // Check if already requested
        var existingRequest = await _context.JoinRequests
            .FirstOrDefaultAsync(jr => jr.TeamRequestId == teamRequestId && jr.ApplicantId == userId);
        
        if (existingRequest != null)
        {
            return BadRequest("You have already sent a join request for this team");
        }

        // Check if owner is trying to join their own team
        if (teamRequest.OwnerId == userId)
        {
            return BadRequest("You cannot join your own team request");
        }

        var joinRequest = new JoinRequest
        {
            TeamRequestId = teamRequestId,
            ApplicantId = userId,
            ApplicantName = request.ApplicantName,
            ApplicantEmail = request.ApplicantEmail,
            ApplicantSkills = request.ApplicantSkills,
            Message = request.Message,
            CreatedAt = DateTime.UtcNow
        };

        _context.JoinRequests.Add(joinRequest);
        await _context.SaveChangesAsync();

        return Ok(new JoinRequestDto
        {
            Id = joinRequest.Id,
            TeamRequestId = joinRequest.TeamRequestId,
            ApplicantId = joinRequest.ApplicantId,
            ApplicantName = joinRequest.ApplicantName,
            ApplicantEmail = joinRequest.ApplicantEmail,
            ApplicantSkills = joinRequest.ApplicantSkills.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList(),
            Message = joinRequest.Message,
            Status = joinRequest.Status,
            CreatedAt = joinRequest.CreatedAt
        });
    }

    // GET: api/TeamRequests/5/join-requests
    [Authorize]
    [HttpGet("{teamRequestId}/join-requests")]
    public async Task<ActionResult<IEnumerable<JoinRequestDto>>> GetJoinRequests(int teamRequestId)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var teamRequest = await _context.TeamRequests.FindAsync(teamRequestId);
        if (teamRequest == null)
        {
            return NotFound("Team request not found");
        }

        // Only owner can see join requests
        if (teamRequest.OwnerId != userId)
        {
            return Forbid();
        }

        var rawData = await _context.JoinRequests
            .Include(jr => jr.Applicant)
            .Where(jr => jr.TeamRequestId == teamRequestId)
            .OrderByDescending(jr => jr.CreatedAt)
            .ToListAsync();

        var joinRequests = rawData.Select(jr => new JoinRequestDto
        {
            Id = jr.Id,
            TeamRequestId = jr.TeamRequestId,
            ApplicantId = jr.ApplicantId,
            ApplicantName = jr.ApplicantName,
            ApplicantEmail = jr.ApplicantEmail,
            ApplicantSkills = jr.ApplicantSkills.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList(),
            Message = jr.Message,
            Status = jr.Status,
            CreatedAt = jr.CreatedAt
        }).ToList();

        return Ok(joinRequests);
    }

    // PUT: api/TeamRequests/join-requests/5/status
    [Authorize]
    [HttpPut("join-requests/{joinRequestId}/status")]
    public async Task<IActionResult> UpdateJoinRequestStatus(int joinRequestId, [FromBody] UpdateJoinRequestStatusDto request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var joinRequest = await _context.JoinRequests
            .Include(jr => jr.TeamRequest)
            .FirstOrDefaultAsync(jr => jr.Id == joinRequestId);

        if (joinRequest == null)
        {
            return NotFound("Join request not found");
        }

        // Only team request owner can update status
        if (joinRequest.TeamRequest?.OwnerId != userId)
        {
            return Forbid();
        }

        joinRequest.Status = request.Status;
        joinRequest.ReviewedAt = DateTime.UtcNow;

        // If accepted, increment current members
        if (request.Status == "Accepted" && joinRequest.TeamRequest != null)
        {
            joinRequest.TeamRequest.CurrentMembers++;
            
            // Close team request if full
            if (joinRequest.TeamRequest.CurrentMembers >= joinRequest.TeamRequest.MembersNeeded + 1)
            {
                joinRequest.TeamRequest.Status = "Closed";
            }
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/TeamRequests/stats
    [HttpGet("stats")]
    public async Task<ActionResult<object>> GetStats()
    {
        var activeRequests = await _context.TeamRequests.CountAsync(tr => tr.Status == "Active");
        var totalMembersNeeded = await _context.TeamRequests
            .Where(tr => tr.Status == "Active")
            .SumAsync(tr => tr.MembersNeeded);
        var uniqueSkills = await _context.TeamRequests
            .Where(tr => tr.Status == "Active")
            .Select(tr => tr.SkillsNeeded)
            .ToListAsync();

        var skillCount = uniqueSkills
            .SelectMany(s => s.Split(',', StringSplitOptions.RemoveEmptyEntries))
            .Select(s => s.Trim())
            .Distinct()
            .Count();

        return Ok(new
        {
            ActiveRequests = activeRequests,
            MembersNeeded = totalMembersNeeded,
            SkillsRequired = skillCount
        });
    }

    private static string GetRelativeTime(DateTime dateTime)
    {
        var timeSpan = DateTime.UtcNow - dateTime;

        if (timeSpan.TotalMinutes < 1)
            return "Just now";
        if (timeSpan.TotalMinutes < 60)
            return $"{(int)timeSpan.TotalMinutes} minutes ago";
        if (timeSpan.TotalHours < 24)
            return $"{(int)timeSpan.TotalHours} hours ago";
        if (timeSpan.TotalDays < 7)
            return $"{(int)timeSpan.TotalDays} days ago";
        if (timeSpan.TotalDays < 30)
            return $"{(int)(timeSpan.TotalDays / 7)} weeks ago";
        
        return dateTime.ToString("MMM dd, yyyy");
    }
}

