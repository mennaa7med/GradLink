using GradLink.Application.DTOs.Sponsors;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SponsorsController : ControllerBase
{
    private readonly AppDbContext _context;

    public SponsorsController(AppDbContext context)
    {
        _context = context;
    }

    #region Sponsors CRUD

    /// <summary>
    /// Get all sponsors (public endpoint)
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<SponsorDto>>> GetSponsors(
        [FromQuery] string? field = null,
        [FromQuery] string? search = null)
    {
        var query = _context.Sponsors.AsQueryable();

        // Filter by field
        if (!string.IsNullOrWhiteSpace(field))
        {
            query = query.Where(s => s.Field != null && s.Field.ToLower() == field.ToLower());
        }

        // Search in name
        if (!string.IsNullOrWhiteSpace(search))
        {
            var lowerSearch = search.ToLower();
            query = query.Where(s =>
                s.Name.ToLower().Contains(lowerSearch) ||
                (s.SupportedProject != null && s.SupportedProject.ToLower().Contains(lowerSearch)));
        }

        var sponsors = await query
            .OrderByDescending(s => s.CreatedAt)
            .Select(s => new SponsorDto
            {
                Id = s.Id,
                Name = s.Name,
                Field = s.Field,
                SupportedProject = s.SupportedProject,
                Link = s.Link,
                Logo = s.Logo,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            })
            .ToListAsync();

        return Ok(sponsors);
    }

    /// <summary>
    /// Get a specific sponsor by ID
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<SponsorDto>> GetSponsor(int id)
    {
        var sponsor = await _context.Sponsors
            .FirstOrDefaultAsync(s => s.Id == id);

        if (sponsor == null)
        {
            return NotFound(new { error = "Sponsor not found" });
        }

        var dto = new SponsorDto
        {
            Id = sponsor.Id,
            Name = sponsor.Name,
            Field = sponsor.Field,
            SupportedProject = sponsor.SupportedProject,
            Link = sponsor.Link,
            Logo = sponsor.Logo,
            CreatedAt = sponsor.CreatedAt,
            UpdatedAt = sponsor.UpdatedAt
        };

        return Ok(dto);
    }

    /// <summary>
    /// Create a new sponsor (Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SponsorDto>> CreateSponsor([FromBody] CreateSponsorRequest request)
    {
        var sponsor = new Sponsor
        {
            Name = request.Name,
            Field = request.Field,
            SupportedProject = request.SupportedProject,
            Link = request.Link,
            Logo = request.Logo,
            CreatedAt = DateTime.UtcNow
        };

        _context.Sponsors.Add(sponsor);
        await _context.SaveChangesAsync();

        var dto = new SponsorDto
        {
            Id = sponsor.Id,
            Name = sponsor.Name,
            Field = sponsor.Field,
            SupportedProject = sponsor.SupportedProject,
            Link = sponsor.Link,
            Logo = sponsor.Logo,
            CreatedAt = sponsor.CreatedAt,
            UpdatedAt = sponsor.UpdatedAt
        };

        return CreatedAtAction(nameof(GetSponsor), new { id = sponsor.Id }, dto);
    }

    /// <summary>
    /// Update a sponsor (Admin only)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateSponsor(int id, [FromBody] UpdateSponsorRequest request)
    {
        var sponsor = await _context.Sponsors.FindAsync(id);

        if (sponsor == null)
        {
            return NotFound(new { error = "Sponsor not found" });
        }

        if (!string.IsNullOrEmpty(request.Name)) sponsor.Name = request.Name;
        if (request.Field != null) sponsor.Field = request.Field;
        if (request.SupportedProject != null) sponsor.SupportedProject = request.SupportedProject;
        if (request.Link != null) sponsor.Link = request.Link;
        if (request.Logo != null) sponsor.Logo = request.Logo;

        sponsor.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Delete a sponsor (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteSponsor(int id)
    {
        var sponsor = await _context.Sponsors.FindAsync(id);

        if (sponsor == null)
        {
            return NotFound(new { error = "Sponsor not found" });
        }

        _context.Sponsors.Remove(sponsor);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    #endregion

    #region Sponsor Applications

    /// <summary>
    /// Get all sponsor applications (Admin only)
    /// </summary>
    [HttpGet("applications")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<SponsorApplicationDto>>> GetApplications(
        [FromQuery] string? status = null)
    {
        var query = _context.SponsorApplications.AsQueryable();

        // Filter by status
        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(sa => sa.Status.ToLower() == status.ToLower());
        }

        var applications = await query
            .OrderByDescending(sa => sa.CreatedAt)
            .Select(sa => new SponsorApplicationDto
            {
                Id = sa.Id,
                FullName = sa.FullName,
                Email = sa.Email,
                CompanyName = sa.CompanyName,
                SponsorshipType = sa.SponsorshipType,
                Message = sa.Message,
                Status = sa.Status,
                CreatedAt = sa.CreatedAt,
                UpdatedAt = sa.UpdatedAt,
                UserId = sa.UserId
            })
            .ToListAsync();

        return Ok(applications);
    }

    /// <summary>
    /// Get a specific application by ID (Admin only)
    /// </summary>
    [HttpGet("applications/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SponsorApplicationDto>> GetApplication(int id)
    {
        var application = await _context.SponsorApplications
            .FirstOrDefaultAsync(sa => sa.Id == id);

        if (application == null)
        {
            return NotFound(new { error = "Application not found" });
        }

        var dto = new SponsorApplicationDto
        {
            Id = application.Id,
            FullName = application.FullName,
            Email = application.Email,
            CompanyName = application.CompanyName,
            SponsorshipType = application.SponsorshipType,
            Message = application.Message,
            Status = application.Status,
            CreatedAt = application.CreatedAt,
            UpdatedAt = application.UpdatedAt,
            UserId = application.UserId
        };

        return Ok(dto);
    }

    /// <summary>
    /// Submit a new sponsor application (public)
    /// </summary>
    [HttpPost("applications")]
    [AllowAnonymous]
    public async Task<ActionResult<SponsorApplicationDto>> SubmitApplication([FromBody] CreateSponsorApplicationRequest request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        // Check if email already has a pending application
        var existingApplication = await _context.SponsorApplications
            .FirstOrDefaultAsync(sa => sa.Email.ToLower() == request.Email.ToLower() && sa.Status == "Pending");

        if (existingApplication != null)
        {
            return BadRequest(new { error = "You already have a pending application" });
        }

        var application = new SponsorApplication
        {
            FullName = request.FullName,
            Email = request.Email,
            CompanyName = request.CompanyName,
            SponsorshipType = request.SponsorshipType,
            Message = request.Message,
            Status = "Pending",
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.SponsorApplications.Add(application);
        await _context.SaveChangesAsync();

        var dto = new SponsorApplicationDto
        {
            Id = application.Id,
            FullName = application.FullName,
            Email = application.Email,
            CompanyName = application.CompanyName,
            SponsorshipType = application.SponsorshipType,
            Message = application.Message,
            Status = application.Status,
            CreatedAt = application.CreatedAt,
            UpdatedAt = application.UpdatedAt,
            UserId = application.UserId
        };

        return CreatedAtAction(nameof(GetApplication), new { id = application.Id }, dto);
    }

    /// <summary>
    /// Update an application (Admin only)
    /// </summary>
    [HttpPut("applications/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateApplication(int id, [FromBody] UpdateSponsorApplicationRequest request)
    {
        var application = await _context.SponsorApplications.FindAsync(id);

        if (application == null)
        {
            return NotFound(new { error = "Application not found" });
        }

        if (!string.IsNullOrEmpty(request.FullName)) application.FullName = request.FullName;
        if (!string.IsNullOrEmpty(request.Email)) application.Email = request.Email;
        if (!string.IsNullOrEmpty(request.CompanyName)) application.CompanyName = request.CompanyName;
        if (!string.IsNullOrEmpty(request.SponsorshipType)) application.SponsorshipType = request.SponsorshipType;
        if (request.Message != null) application.Message = request.Message;
        if (!string.IsNullOrEmpty(request.Status)) application.Status = request.Status;

        application.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Approve an application and optionally create a sponsor (Admin only)
    /// </summary>
    [HttpPost("applications/{id}/approve")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SponsorDto>> ApproveApplication(int id, [FromQuery] bool createSponsor = true)
    {
        var application = await _context.SponsorApplications.FindAsync(id);

        if (application == null)
        {
            return NotFound(new { error = "Application not found" });
        }

        if (application.Status == "Approved")
        {
            return BadRequest(new { error = "Application is already approved" });
        }

        application.Status = "Approved";
        application.UpdatedAt = DateTime.UtcNow;

        SponsorDto? sponsorDto = null;

        if (createSponsor)
        {
            var sponsor = new Sponsor
            {
                Name = application.CompanyName,
                CreatedAt = DateTime.UtcNow
            };

            _context.Sponsors.Add(sponsor);
            await _context.SaveChangesAsync();

            sponsorDto = new SponsorDto
            {
                Id = sponsor.Id,
                Name = sponsor.Name,
                Field = sponsor.Field,
                SupportedProject = sponsor.SupportedProject,
                Link = sponsor.Link,
                Logo = sponsor.Logo,
                CreatedAt = sponsor.CreatedAt,
                UpdatedAt = sponsor.UpdatedAt
            };
        }
        else
        {
            await _context.SaveChangesAsync();
        }

        return Ok(new
        {
            message = "Application approved successfully",
            sponsor = sponsorDto
        });
    }

    /// <summary>
    /// Reject an application (Admin only)
    /// </summary>
    [HttpPost("applications/{id}/reject")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RejectApplication(int id)
    {
        var application = await _context.SponsorApplications.FindAsync(id);

        if (application == null)
        {
            return NotFound(new { error = "Application not found" });
        }

        if (application.Status == "Rejected")
        {
            return BadRequest(new { error = "Application is already rejected" });
        }

        application.Status = "Rejected";
        application.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Application rejected successfully" });
    }

    /// <summary>
    /// Delete an application (Admin only)
    /// </summary>
    [HttpDelete("applications/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteApplication(int id)
    {
        var application = await _context.SponsorApplications.FindAsync(id);

        if (application == null)
        {
            return NotFound(new { error = "Application not found" });
        }

        _context.SponsorApplications.Remove(application);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Get my applications (for authenticated users)
    /// </summary>
    [HttpGet("applications/me")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<SponsorApplicationDto>>> GetMyApplications()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;

        var applications = await _context.SponsorApplications
            .Where(sa => sa.UserId == userId)
            .OrderByDescending(sa => sa.CreatedAt)
            .Select(sa => new SponsorApplicationDto
            {
                Id = sa.Id,
                FullName = sa.FullName,
                Email = sa.Email,
                CompanyName = sa.CompanyName,
                SponsorshipType = sa.SponsorshipType,
                Message = sa.Message,
                Status = sa.Status,
                CreatedAt = sa.CreatedAt,
                UpdatedAt = sa.UpdatedAt,
                UserId = sa.UserId
            })
            .ToListAsync();

        return Ok(applications);
    }

    #endregion

    #region Statistics

    /// <summary>
    /// Get sponsors statistics
    /// </summary>
    [HttpGet("stats")]
    [AllowAnonymous]
    public async Task<ActionResult<object>> GetStats()
    {
        var totalSponsors = await _context.Sponsors.CountAsync();

        var fieldDistribution = await _context.Sponsors
            .Where(s => s.Field != null)
            .GroupBy(s => s.Field)
            .Select(g => new { Field = g.Key, Count = g.Count() })
            .ToListAsync();

        var pendingApplications = await _context.SponsorApplications
            .Where(sa => sa.Status == "Pending")
            .CountAsync();

        var applicationStats = await _context.SponsorApplications
            .GroupBy(sa => sa.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync();

        var sponsorshipTypeStats = await _context.SponsorApplications
            .GroupBy(sa => sa.SponsorshipType)
            .Select(g => new { Type = g.Key, Count = g.Count() })
            .ToListAsync();

        var recentSponsors = await _context.Sponsors
            .OrderByDescending(s => s.CreatedAt)
            .Take(5)
            .Select(s => new { s.Id, s.Name, s.Field, s.CreatedAt })
            .ToListAsync();

        return Ok(new
        {
            TotalSponsors = totalSponsors,
            FieldDistribution = fieldDistribution,
            PendingApplications = pendingApplications,
            ApplicationStats = applicationStats,
            SponsorshipTypeStats = sponsorshipTypeStats,
            RecentSponsors = recentSponsors
        });
    }

    #endregion
}

