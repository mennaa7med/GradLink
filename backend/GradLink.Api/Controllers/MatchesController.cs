using System.Text.Json;
using GradLink.Application.DTOs.Matches;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MatchesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<MatchesController> _logger;

    public MatchesController(AppDbContext context, ILogger<MatchesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("resume/{resumeId}")]
    public async Task<ActionResult<IEnumerable<MatchDto>>> GetMatchesForResume(int resumeId)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        // Verify resume ownership
        var resume = await _context.Resumes
            .FirstOrDefaultAsync(r => r.Id == resumeId && r.UserId == userId);

        if (resume == null)
        {
            return NotFound();
        }

        var matches = await _context.Matches
            .Where(m => m.ResumeId == resumeId)
            .Include(m => m.JobPosting)
            .Include(m => m.Resume)
            .OrderByDescending(m => m.Score)
            .Select(m => new MatchDto
            {
                Id = m.Id,
                Score = m.Score,
                Reason = m.Reason,
                MatchedAt = m.MatchedAt,
                ResumeId = m.ResumeId,
                ResumeFileName = m.Resume!.FileName,
                JobPostingId = m.JobPostingId,
                JobTitle = m.JobPosting!.Title
            })
            .ToListAsync();

        return Ok(matches);
    }

    [HttpGet("job/{jobId}")]
    public async Task<ActionResult<IEnumerable<MatchDto>>> GetMatchesForJob(int jobId)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        // Verify job ownership
        var job = await _context.JobPostings
            .FirstOrDefaultAsync(j => j.Id == jobId && j.PostedById == userId);

        if (job == null)
        {
            return NotFound();
        }

        var matches = await _context.Matches
            .Where(m => m.JobPostingId == jobId)
            .Include(m => m.JobPosting)
            .Include(m => m.Resume)
            .OrderByDescending(m => m.Score)
            .Select(m => new MatchDto
            {
                Id = m.Id,
                Score = m.Score,
                Reason = m.Reason,
                MatchedAt = m.MatchedAt,
                ResumeId = m.ResumeId,
                ResumeFileName = m.Resume!.FileName,
                JobPostingId = m.JobPostingId,
                JobTitle = m.JobPosting!.Title
            })
            .ToListAsync();

        return Ok(matches);
    }

    [HttpPost("run/{resumeId}")]
    public async Task<ActionResult<IEnumerable<MatchDto>>> RunMatching(int resumeId)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        // Verify resume ownership
        var resume = await _context.Resumes
            .FirstOrDefaultAsync(r => r.Id == resumeId && r.UserId == userId);

        if (resume == null)
        {
            return NotFound();
        }

        if (resume.AnalysisStatus != "Completed")
        {
            return BadRequest(new { error = "Resume analysis is not completed yet" });
        }

        // Get all active jobs
        var jobs = await _context.JobPostings
            .Where(j => j.Status == "Active")
            .ToListAsync();

        // Clear existing matches for this resume
        var existingMatches = await _context.Matches
            .Where(m => m.ResumeId == resumeId)
            .ToListAsync();
        _context.Matches.RemoveRange(existingMatches);

        // Calculate matches
        var resumeSkills = resume.Skills != null ? JsonSerializer.Deserialize<List<string>>(resume.Skills) ?? new List<string>() : new List<string>();

        foreach (var job in jobs)
        {
            var jobSkills = job.Skills != null ? JsonSerializer.Deserialize<List<string>>(job.Skills) ?? new List<string>() : new List<string>();
            
            var score = CalculateMatchScore(resumeSkills, jobSkills, resume.Experience, job.Requirements);

            if (score >= 0.3) // Threshold for matching
            {
                var match = new Match
                {
                    ResumeId = resumeId,
                    JobPostingId = job.Id,
                    Score = score,
                    Reason = GenerateMatchReason(resumeSkills, jobSkills),
                    MatchedAt = DateTime.UtcNow
                };

                _context.Matches.Add(match);
            }
        }

        await _context.SaveChangesAsync();

        // Return matches
        var newMatches = await _context.Matches
            .Where(m => m.ResumeId == resumeId)
            .Include(m => m.JobPosting)
            .Include(m => m.Resume)
            .OrderByDescending(m => m.Score)
            .Select(m => new MatchDto
            {
                Id = m.Id,
                Score = m.Score,
                Reason = m.Reason,
                MatchedAt = m.MatchedAt,
                ResumeId = m.ResumeId,
                ResumeFileName = m.Resume!.FileName,
                JobPostingId = m.JobPostingId,
                JobTitle = m.JobPosting!.Title
            })
            .ToListAsync();

        return Ok(newMatches);
    }

    private double CalculateMatchScore(List<string> resumeSkills, List<string> jobSkills, string? experience, string? requirements)
    {
        if (jobSkills.Count == 0) return 0.5;

        var matchingSkills = resumeSkills
            .Where(rs => jobSkills.Any(js => js.Equals(rs, StringComparison.OrdinalIgnoreCase)))
            .Count();

        var skillScore = jobSkills.Count > 0 ? (double)matchingSkills / jobSkills.Count : 0;

        // Simple heuristic - can be enhanced
        return Math.Min(1.0, skillScore * 1.2);
    }

    private string GenerateMatchReason(List<string> resumeSkills, List<string> jobSkills)
    {
        var matchingSkills = resumeSkills
            .Where(rs => jobSkills.Any(js => js.Equals(rs, StringComparison.OrdinalIgnoreCase)))
            .Take(5)
            .ToList();

        if (matchingSkills.Count == 0)
        {
            return "General fit based on profile";
        }

        return $"Matching skills: {string.Join(", ", matchingSkills)}";
    }
}

