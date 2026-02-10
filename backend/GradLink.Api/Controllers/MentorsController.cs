using GradLink.Application.DTOs.Mentors;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MentorsController : ControllerBase
{
    private readonly AppDbContext _context;

    public MentorsController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all mentors (public endpoint for students to view available mentors)
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<MentorDto>>> GetMentors()
    {
        var mentors = await _context.Mentors
            .OrderByDescending(m => m.CreatedAt)
            .Select(m => new MentorDto
            {
                Id = m.Id,
                FullName = m.FullName,
                Email = m.Email,
                JobTitle = m.JobTitle,
                ProfilePicture = m.ProfilePicture,
                Specialization = m.Specialization,
                Bio = m.Bio,
                Skills = m.Skills,
                ExperienceYears = m.ExperienceYears,
                WhatsApp = m.WhatsApp,
                FacebookUrl = m.FacebookUrl,
                LinkedInUrl = m.LinkedInUrl,
                CreatedAt = m.CreatedAt,
                UpdatedAt = m.UpdatedAt,
                UserId = m.UserId
            })
            .ToListAsync();

        return Ok(mentors);
    }

    /// <summary>
    /// Get a specific mentor by ID
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<MentorDto>> GetMentor(int id)
    {
        var mentor = await _context.Mentors
            .Where(m => m.Id == id)
            .Select(m => new MentorDto
            {
                Id = m.Id,
                FullName = m.FullName,
                Email = m.Email,
                JobTitle = m.JobTitle,
                ProfilePicture = m.ProfilePicture,
                Specialization = m.Specialization,
                Bio = m.Bio,
                Skills = m.Skills,
                ExperienceYears = m.ExperienceYears,
                WhatsApp = m.WhatsApp,
                FacebookUrl = m.FacebookUrl,
                LinkedInUrl = m.LinkedInUrl,
                CreatedAt = m.CreatedAt,
                UpdatedAt = m.UpdatedAt,
                UserId = m.UserId
            })
            .FirstOrDefaultAsync();

        if (mentor == null)
        {
            return NotFound(new { error = "Mentor not found" });
        }

        return Ok(mentor);
    }

    /// <summary>
    /// Get mentors by specialization
    /// </summary>
    [HttpGet("specialization/{specialization}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<MentorDto>>> GetMentorsBySpecialization(string specialization)
    {
        var mentors = await _context.Mentors
            .Where(m => m.Specialization != null && m.Specialization.ToLower().Contains(specialization.ToLower()))
            .OrderByDescending(m => m.ExperienceYears)
            .Select(m => new MentorDto
            {
                Id = m.Id,
                FullName = m.FullName,
                Email = m.Email,
                JobTitle = m.JobTitle,
                ProfilePicture = m.ProfilePicture,
                Specialization = m.Specialization,
                Bio = m.Bio,
                Skills = m.Skills,
                ExperienceYears = m.ExperienceYears,
                WhatsApp = m.WhatsApp,
                FacebookUrl = m.FacebookUrl,
                LinkedInUrl = m.LinkedInUrl,
                CreatedAt = m.CreatedAt,
                UpdatedAt = m.UpdatedAt,
                UserId = m.UserId
            })
            .ToListAsync();

        return Ok(mentors);
    }

    /// <summary>
    /// Create a new mentor (Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<MentorDto>> CreateMentor([FromBody] CreateMentorRequest request)
    {
        // Check if email already exists
        var existingMentor = await _context.Mentors
            .FirstOrDefaultAsync(m => m.Email.ToLower() == request.Email.ToLower());

        if (existingMentor != null)
        {
            return BadRequest(new { error = "A mentor with this email already exists" });
        }

        var mentor = new Mentor
        {
            FullName = request.FullName,
            Email = request.Email,
            JobTitle = request.JobTitle,
            ProfilePicture = request.ProfilePicture,
            Specialization = request.Specialization,
            Bio = request.Bio,
            Skills = request.Skills,
            ExperienceYears = request.ExperienceYears,
            WhatsApp = request.WhatsApp,
            FacebookUrl = request.FacebookUrl,
            LinkedInUrl = request.LinkedInUrl,
            CreatedAt = DateTime.UtcNow
        };

        _context.Mentors.Add(mentor);
        await _context.SaveChangesAsync();

        var mentorDto = new MentorDto
        {
            Id = mentor.Id,
            FullName = mentor.FullName,
            Email = mentor.Email,
            JobTitle = mentor.JobTitle,
            ProfilePicture = mentor.ProfilePicture,
            Specialization = mentor.Specialization,
            Bio = mentor.Bio,
            Skills = mentor.Skills,
            ExperienceYears = mentor.ExperienceYears,
            WhatsApp = mentor.WhatsApp,
            FacebookUrl = mentor.FacebookUrl,
            LinkedInUrl = mentor.LinkedInUrl,
            CreatedAt = mentor.CreatedAt,
            UpdatedAt = mentor.UpdatedAt,
            UserId = mentor.UserId
        };

        return CreatedAtAction(nameof(GetMentor), new { id = mentor.Id }, mentorDto);
    }

    /// <summary>
    /// Apply to become a mentor (Authenticated users)
    /// </summary>
    [HttpPost("apply")]
    [Authorize]
    public async Task<ActionResult<MentorDto>> ApplyAsMentor([FromBody] CreateMentorRequest request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        // Check if user already applied as mentor
        var existingMentor = await _context.Mentors
            .FirstOrDefaultAsync(m => m.UserId == userId || m.Email.ToLower() == request.Email.ToLower());

        if (existingMentor != null)
        {
            return BadRequest(new { error = "You have already applied as a mentor or this email is already in use" });
        }

        var mentor = new Mentor
        {
            FullName = request.FullName,
            Email = request.Email,
            JobTitle = request.JobTitle,
            ProfilePicture = request.ProfilePicture,
            Specialization = request.Specialization,
            Bio = request.Bio,
            Skills = request.Skills,
            ExperienceYears = request.ExperienceYears,
            WhatsApp = request.WhatsApp,
            FacebookUrl = request.FacebookUrl,
            LinkedInUrl = request.LinkedInUrl,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Mentors.Add(mentor);
        await _context.SaveChangesAsync();

        var mentorDto = new MentorDto
        {
            Id = mentor.Id,
            FullName = mentor.FullName,
            Email = mentor.Email,
            JobTitle = mentor.JobTitle,
            ProfilePicture = mentor.ProfilePicture,
            Specialization = mentor.Specialization,
            Bio = mentor.Bio,
            Skills = mentor.Skills,
            ExperienceYears = mentor.ExperienceYears,
            WhatsApp = mentor.WhatsApp,
            FacebookUrl = mentor.FacebookUrl,
            LinkedInUrl = mentor.LinkedInUrl,
            CreatedAt = mentor.CreatedAt,
            UpdatedAt = mentor.UpdatedAt,
            UserId = mentor.UserId
        };

        return CreatedAtAction(nameof(GetMentor), new { id = mentor.Id }, mentorDto);
    }

    /// <summary>
    /// Update a mentor (Admin or the mentor themselves)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateMentor(int id, [FromBody] UpdateMentorRequest request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var isAdmin = User.IsInRole("Admin");

        var mentor = await _context.Mentors.FindAsync(id);

        if (mentor == null)
        {
            return NotFound(new { error = "Mentor not found" });
        }

        // Only admin or the mentor themselves can update
        if (!isAdmin && mentor.UserId != userId)
        {
            return Forbid();
        }

        // Check email uniqueness if being updated
        if (!string.IsNullOrEmpty(request.Email) && request.Email.ToLower() != mentor.Email.ToLower())
        {
            var emailExists = await _context.Mentors
                .AnyAsync(m => m.Email.ToLower() == request.Email.ToLower() && m.Id != id);

            if (emailExists)
            {
                return BadRequest(new { error = "A mentor with this email already exists" });
            }
        }

        // Update fields if provided
        if (!string.IsNullOrEmpty(request.FullName)) mentor.FullName = request.FullName;
        if (!string.IsNullOrEmpty(request.Email)) mentor.Email = request.Email;
        if (request.JobTitle != null) mentor.JobTitle = request.JobTitle;
        if (request.ProfilePicture != null) mentor.ProfilePicture = request.ProfilePicture;
        if (request.Specialization != null) mentor.Specialization = request.Specialization;
        if (request.Bio != null) mentor.Bio = request.Bio;
        if (request.Skills != null) mentor.Skills = request.Skills;
        if (request.ExperienceYears.HasValue) mentor.ExperienceYears = request.ExperienceYears;
        if (request.WhatsApp != null) mentor.WhatsApp = request.WhatsApp;
        if (request.FacebookUrl != null) mentor.FacebookUrl = request.FacebookUrl;
        if (request.LinkedInUrl != null) mentor.LinkedInUrl = request.LinkedInUrl;

        mentor.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Delete a mentor (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteMentor(int id)
    {
        var mentor = await _context.Mentors.FindAsync(id);

        if (mentor == null)
        {
            return NotFound(new { error = "Mentor not found" });
        }

        _context.Mentors.Remove(mentor);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Search mentors by name or skills
    /// </summary>
    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<MentorDto>>> SearchMentors([FromQuery] string? query)
    {
        var mentorsQuery = _context.Mentors.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var lowerQuery = query.ToLower();
            mentorsQuery = mentorsQuery.Where(m =>
                m.FullName.ToLower().Contains(lowerQuery) ||
                (m.Skills != null && m.Skills.ToLower().Contains(lowerQuery)) ||
                (m.Specialization != null && m.Specialization.ToLower().Contains(lowerQuery)) ||
                (m.Bio != null && m.Bio.ToLower().Contains(lowerQuery)));
        }

        var mentors = await mentorsQuery
            .OrderByDescending(m => m.ExperienceYears)
            .Select(m => new MentorDto
            {
                Id = m.Id,
                FullName = m.FullName,
                Email = m.Email,
                JobTitle = m.JobTitle,
                ProfilePicture = m.ProfilePicture,
                Specialization = m.Specialization,
                Bio = m.Bio,
                Skills = m.Skills,
                ExperienceYears = m.ExperienceYears,
                WhatsApp = m.WhatsApp,
                FacebookUrl = m.FacebookUrl,
                LinkedInUrl = m.LinkedInUrl,
                CreatedAt = m.CreatedAt,
                UpdatedAt = m.UpdatedAt,
                UserId = m.UserId
            })
            .ToListAsync();

        return Ok(mentors);
    }
}

