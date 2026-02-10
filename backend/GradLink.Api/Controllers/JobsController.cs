using System.Text.Json;
using GradLink.Application.DTOs.Jobs;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Controllers;

/// <summary>
/// Manage job postings - create, update, delete, and search jobs
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class JobsController : ControllerBase
{
    private readonly AppDbContext _context;

    public JobsController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all active job postings
    /// </summary>
    /// <returns>List of all active jobs</returns>
    /// <response code="200">Returns the list of jobs</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<JobPostingDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<JobPostingDto>>> GetJobs()
    {
        var jobs = await _context.JobPostings
            .Where(j => j.Status == "Active")
            .Include(j => j.PostedBy)
            .ToListAsync();

        var jobDtos = jobs.Select(j => new JobPostingDto
        {
            Id = j.Id,
            Title = j.Title,
            Description = j.Description,
            Requirements = j.Requirements != null ? JsonSerializer.Deserialize<List<string>>(j.Requirements) : null,
            Skills = j.Skills != null ? JsonSerializer.Deserialize<List<string>>(j.Skills) : null,
            Location = j.Location,
            EmploymentType = j.EmploymentType,
            SalaryMin = j.SalaryMin,
            SalaryMax = j.SalaryMax,
            CompanyName = j.CompanyName,
            Status = j.Status,
            CreatedAt = j.CreatedAt,
            UpdatedAt = j.UpdatedAt,
            ExpiresAt = j.ExpiresAt,
            PostedById = j.PostedById,
            PostedByName = j.PostedBy!.FullName
        }).ToList();

        return Ok(jobDtos);
    }

    [HttpGet("my")]
    public async Task<ActionResult<IEnumerable<JobPostingDto>>> GetMyJobs()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var jobs = await _context.JobPostings
            .Where(j => j.PostedById == userId)
            .Include(j => j.PostedBy)
            .ToListAsync();

        var jobDtos = jobs.Select(j => new JobPostingDto
        {
            Id = j.Id,
            Title = j.Title,
            Description = j.Description,
            Requirements = j.Requirements != null ? JsonSerializer.Deserialize<List<string>>(j.Requirements) : null,
            Skills = j.Skills != null ? JsonSerializer.Deserialize<List<string>>(j.Skills) : null,
            Location = j.Location,
            EmploymentType = j.EmploymentType,
            SalaryMin = j.SalaryMin,
            SalaryMax = j.SalaryMax,
            CompanyName = j.CompanyName,
            Status = j.Status,
            CreatedAt = j.CreatedAt,
            UpdatedAt = j.UpdatedAt,
            ExpiresAt = j.ExpiresAt,
            PostedById = j.PostedById,
            PostedByName = j.PostedBy!.FullName
        }).ToList();

        return Ok(jobDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JobPostingDto>> GetJob(int id)
    {
        var job = await _context.JobPostings
            .Where(j => j.Id == id)
            .Include(j => j.PostedBy)
            .FirstOrDefaultAsync();

        if (job == null)
        {
            return NotFound();
        }

        var jobDto = new JobPostingDto
        {
            Id = job.Id,
            Title = job.Title,
            Description = job.Description,
            Requirements = job.Requirements != null ? JsonSerializer.Deserialize<List<string>>(job.Requirements) : null,
            Skills = job.Skills != null ? JsonSerializer.Deserialize<List<string>>(job.Skills) : null,
            Location = job.Location,
            EmploymentType = job.EmploymentType,
            SalaryMin = job.SalaryMin,
            SalaryMax = job.SalaryMax,
            CompanyName = job.CompanyName,
            Status = job.Status,
            CreatedAt = job.CreatedAt,
            UpdatedAt = job.UpdatedAt,
            ExpiresAt = job.ExpiresAt,
            PostedById = job.PostedById,
            PostedByName = job.PostedBy!.FullName
        };

        return Ok(jobDto);
    }

    [HttpPost]
    public async Task<ActionResult<JobPostingDto>> CreateJob([FromBody] CreateJobRequest request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;

        var job = new JobPosting
        {
            Title = request.Title,
            Description = request.Description,
            Requirements = request.Requirements != null ? JsonSerializer.Serialize(request.Requirements) : null,
            Skills = request.Skills != null ? JsonSerializer.Serialize(request.Skills) : null,
            Location = request.Location,
            EmploymentType = request.EmploymentType,
            SalaryMin = request.SalaryMin,
            SalaryMax = request.SalaryMax,
            CompanyName = request.CompanyName,
            ExpiresAt = request.ExpiresAt,
            PostedById = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.JobPostings.Add(job);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetJob), new { id = job.Id }, new JobPostingDto
        {
            Id = job.Id,
            Title = job.Title,
            Description = job.Description,
            Requirements = request.Requirements,
            Skills = request.Skills,
            Location = job.Location,
            EmploymentType = job.EmploymentType,
            SalaryMin = job.SalaryMin,
            SalaryMax = job.SalaryMax,
            CompanyName = job.CompanyName,
            Status = job.Status,
            CreatedAt = job.CreatedAt,
            PostedById = job.PostedById
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateJob(int id, [FromBody] CreateJobRequest request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var job = await _context.JobPostings
            .FirstOrDefaultAsync(j => j.Id == id && j.PostedById == userId);

        if (job == null)
        {
            return NotFound();
        }

        job.Title = request.Title;
        job.Description = request.Description;
        job.Requirements = request.Requirements != null ? JsonSerializer.Serialize(request.Requirements) : null;
        job.Skills = request.Skills != null ? JsonSerializer.Serialize(request.Skills) : null;
        job.Location = request.Location;
        job.EmploymentType = request.EmploymentType;
        job.SalaryMin = request.SalaryMin;
        job.SalaryMax = request.SalaryMax;
        job.CompanyName = request.CompanyName;
        job.ExpiresAt = request.ExpiresAt;
        job.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJob(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var job = await _context.JobPostings
            .FirstOrDefaultAsync(j => j.Id == id && j.PostedById == userId);

        if (job == null)
        {
            return NotFound();
        }

        _context.JobPostings.Remove(job);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

