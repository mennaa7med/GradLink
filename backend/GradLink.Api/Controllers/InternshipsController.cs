using GradLink.Application.DTOs.Internships;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GradLink.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InternshipsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<InternshipsController> _logger;

    public InternshipsController(AppDbContext context, ILogger<InternshipsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all active internships (public endpoint)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<InternshipDto>>> GetInternships(
        [FromQuery] string? search,
        [FromQuery] string? location,
        [FromQuery] bool? isPaid,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = _context.Internships
            .Include(i => i.PostedBy)
            .Where(i => i.Status == "Active" && (i.ExpiresAt == null || i.ExpiresAt > DateTime.UtcNow));

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(i =>
                i.Title.Contains(search) ||
                (i.Description != null && i.Description.Contains(search)) ||
                (i.Skills != null && i.Skills.Contains(search)) ||
                (i.CompanyName != null && i.CompanyName.Contains(search)));
        }

        if (!string.IsNullOrEmpty(location))
        {
            query = query.Where(i => i.Location != null && i.Location.Contains(location));
        }

        if (isPaid.HasValue)
        {
            query = query.Where(i => i.IsPaid == isPaid.Value);
        }

        var total = await query.CountAsync();
        var internships = await query
            .OrderByDescending(i => i.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(i => new InternshipDto
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                Requirements = i.Requirements,
                Skills = i.Skills,
                Location = i.Location,
                Duration = i.Duration,
                IsPaid = i.IsPaid,
                Stipend = i.Stipend,
                CompanyName = i.CompanyName ?? i.PostedBy!.Company,
                Status = i.Status,
                CreatedAt = i.CreatedAt,
                ExpiresAt = i.ExpiresAt,
                PostedById = i.PostedById,
                PostedByName = i.PostedBy!.FullName,
                ApplicationsCount = i.Applications != null ? i.Applications.Count : 0
            })
            .ToListAsync();

        return Ok(new { total, page, pageSize, internships });
    }

    /// <summary>
    /// Get my internships (company dashboard)
    /// </summary>
    [Authorize]
    [HttpGet("my")]
    public async Task<ActionResult<List<InternshipDto>>> GetMyInternships()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var internships = await _context.Internships
            .Include(i => i.Applications)
            .Where(i => i.PostedById == userId)
            .OrderByDescending(i => i.CreatedAt)
            .Select(i => new InternshipDto
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                Requirements = i.Requirements,
                Skills = i.Skills,
                Location = i.Location,
                Duration = i.Duration,
                IsPaid = i.IsPaid,
                Stipend = i.Stipend,
                CompanyName = i.CompanyName,
                Status = i.Status,
                CreatedAt = i.CreatedAt,
                ExpiresAt = i.ExpiresAt,
                PostedById = i.PostedById,
                ApplicationsCount = i.Applications != null ? i.Applications.Count : 0
            })
            .ToListAsync();

        return Ok(internships);
    }

    /// <summary>
    /// Get single internship
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<InternshipDto>> GetInternship(int id)
    {
        var internship = await _context.Internships
            .Include(i => i.PostedBy)
            .Include(i => i.Applications)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (internship == null) return NotFound();

        return Ok(new InternshipDto
        {
            Id = internship.Id,
            Title = internship.Title,
            Description = internship.Description,
            Requirements = internship.Requirements,
            Skills = internship.Skills,
            Location = internship.Location,
            Duration = internship.Duration,
            IsPaid = internship.IsPaid,
            Stipend = internship.Stipend,
            CompanyName = internship.CompanyName ?? internship.PostedBy?.Company,
            Status = internship.Status,
            CreatedAt = internship.CreatedAt,
            ExpiresAt = internship.ExpiresAt,
            PostedById = internship.PostedById,
            PostedByName = internship.PostedBy?.FullName,
            ApplicationsCount = internship.Applications?.Count ?? 0
        });
    }

    /// <summary>
    /// Create internship (company only)
    /// </summary>
    [Authorize(Roles = "Company,Sponsor,Admin")]
    [HttpPost]
    public async Task<ActionResult<InternshipDto>> CreateInternship([FromBody] CreateInternshipRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var user = await _context.Users.FindAsync(userId);
        if (user == null) return Unauthorized();

        var internship = new Internship
        {
            Title = request.Title,
            Description = request.Description,
            Requirements = request.Requirements,
            Skills = request.Skills,
            Location = request.Location,
            Duration = request.Duration,
            IsPaid = request.IsPaid,
            Stipend = request.Stipend,
            CompanyName = request.CompanyName ?? user.Company ?? user.FullName,
            ExpiresAt = request.ExpiresAt,
            PostedById = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Internships.Add(internship);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Internship created: {Title} by {UserId}", internship.Title, userId);

        return CreatedAtAction(nameof(GetInternship), new { id = internship.Id }, new InternshipDto
        {
            Id = internship.Id,
            Title = internship.Title,
            Description = internship.Description,
            Requirements = internship.Requirements,
            Skills = internship.Skills,
            Location = internship.Location,
            Duration = internship.Duration,
            IsPaid = internship.IsPaid,
            Stipend = internship.Stipend,
            CompanyName = internship.CompanyName,
            Status = internship.Status,
            CreatedAt = internship.CreatedAt,
            ExpiresAt = internship.ExpiresAt,
            PostedById = internship.PostedById
        });
    }

    /// <summary>
    /// Update internship
    /// </summary>
    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<InternshipDto>> UpdateInternship(int id, [FromBody] UpdateInternshipRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var internship = await _context.Internships.FindAsync(id);
        if (internship == null) return NotFound();

        // Only owner or admin can update
        var isAdmin = User.IsInRole("Admin");
        if (internship.PostedById != userId && !isAdmin)
            return Forbid();

        // Update fields
        if (request.Title != null) internship.Title = request.Title;
        if (request.Description != null) internship.Description = request.Description;
        if (request.Requirements != null) internship.Requirements = request.Requirements;
        if (request.Skills != null) internship.Skills = request.Skills;
        if (request.Location != null) internship.Location = request.Location;
        if (request.Duration != null) internship.Duration = request.Duration;
        if (request.IsPaid.HasValue) internship.IsPaid = request.IsPaid.Value;
        if (request.Stipend.HasValue) internship.Stipend = request.Stipend;
        if (request.CompanyName != null) internship.CompanyName = request.CompanyName;
        if (request.Status != null) internship.Status = request.Status;
        if (request.ExpiresAt.HasValue) internship.ExpiresAt = request.ExpiresAt;

        internship.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new InternshipDto
        {
            Id = internship.Id,
            Title = internship.Title,
            Description = internship.Description,
            Requirements = internship.Requirements,
            Skills = internship.Skills,
            Location = internship.Location,
            Duration = internship.Duration,
            IsPaid = internship.IsPaid,
            Stipend = internship.Stipend,
            CompanyName = internship.CompanyName,
            Status = internship.Status,
            CreatedAt = internship.CreatedAt,
            ExpiresAt = internship.ExpiresAt,
            PostedById = internship.PostedById
        });
    }

    /// <summary>
    /// Delete internship
    /// </summary>
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInternship(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var internship = await _context.Internships.FindAsync(id);
        if (internship == null) return NotFound();

        var isAdmin = User.IsInRole("Admin");
        if (internship.PostedById != userId && !isAdmin)
            return Forbid();

        _context.Internships.Remove(internship);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Internship deleted: {Id} by {UserId}", id, userId);

        return NoContent();
    }
}

