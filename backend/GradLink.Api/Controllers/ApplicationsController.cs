using GradLink.Application.DTOs.Applications;
using GradLink.Application.DTOs.Notifications;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GradLink.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ApplicationsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<ApplicationsController> _logger;

    public ApplicationsController(AppDbContext context, ILogger<ApplicationsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    #region Job Applications

    /// <summary>
    /// Apply for a job
    /// </summary>
    [HttpPost("jobs")]
    public async Task<ActionResult<JobApplicationDto>> ApplyForJob([FromBody] CreateJobApplicationRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        // Check if job exists
        var job = await _context.JobPostings.Include(j => j.PostedBy).FirstOrDefaultAsync(j => j.Id == request.JobPostingId);
        if (job == null) return NotFound("Job not found");

        // Check if already applied
        var existingApplication = await _context.JobApplications
            .FirstOrDefaultAsync(a => a.JobPostingId == request.JobPostingId && a.ApplicantId == userId);
        if (existingApplication != null)
            return BadRequest("You have already applied for this job");

        var user = await _context.Users.FindAsync(userId);

        var application = new JobApplication
        {
            JobPostingId = request.JobPostingId,
            ApplicantId = userId,
            ResumeId = request.ResumeId,
            CoverLetter = request.CoverLetter,
            AppliedAt = DateTime.UtcNow
        };

        _context.JobApplications.Add(application);

        // Create notification for company
        var notification = new Notification
        {
            UserId = job.PostedById,
            Type = "JobApplication",
            Title = "New Job Application",
            Message = $"{user?.FullName ?? "Someone"} applied for {job.Title}",
            Link = $"/company-dashboard-new?page=applicants",
            RelatedEntityType = "JobApplication",
            RelatedEntityId = application.Id
        };
        _context.Notifications.Add(notification);

        await _context.SaveChangesAsync();

        _logger.LogInformation("Job application created: Job {JobId} by User {UserId}", request.JobPostingId, userId);

        return Ok(new JobApplicationDto
        {
            Id = application.Id,
            JobPostingId = application.JobPostingId,
            JobTitle = job.Title,
            CompanyName = job.CompanyName,
            ApplicantId = application.ApplicantId,
            ApplicantName = user?.FullName ?? "",
            ApplicantEmail = user?.Email,
            ResumeId = application.ResumeId,
            CoverLetter = application.CoverLetter,
            Status = application.Status,
            AppliedAt = application.AppliedAt
        });
    }

    /// <summary>
    /// Get job applications for a specific job (company view)
    /// </summary>
    [HttpGet("jobs/{jobId}")]
    public async Task<ActionResult<List<JobApplicationDto>>> GetJobApplications(int jobId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var job = await _context.JobPostings.FindAsync(jobId);
        if (job == null) return NotFound("Job not found");

        // Only owner or admin can see applications
        var isAdmin = User.IsInRole("Admin");
        if (job.PostedById != userId && !isAdmin)
            return Forbid();

        var applications = await _context.JobApplications
            .Include(a => a.Applicant)
            .Include(a => a.Resume)
            .Where(a => a.JobPostingId == jobId)
            .OrderByDescending(a => a.AppliedAt)
            .Select(a => new JobApplicationDto
            {
                Id = a.Id,
                JobPostingId = a.JobPostingId,
                JobTitle = job.Title,
                CompanyName = job.CompanyName,
                ApplicantId = a.ApplicantId,
                ApplicantName = a.Applicant!.FullName ?? "",
                ApplicantEmail = a.Applicant.Email,
                ResumeId = a.ResumeId,
                ResumeName = a.Resume != null ? a.Resume.FileName : null,
                CoverLetter = a.CoverLetter,
                Status = a.Status,
                AppliedAt = a.AppliedAt,
                ReviewedAt = a.ReviewedAt,
                Notes = a.Notes
            })
            .ToListAsync();

        return Ok(applications);
    }

    /// <summary>
    /// Get all applications for company's jobs
    /// </summary>
    [HttpGet("jobs/company/all")]
    public async Task<ActionResult<List<JobApplicationDto>>> GetAllCompanyJobApplications()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var applications = await _context.JobApplications
            .Include(a => a.JobPosting)
            .Include(a => a.Applicant)
            .Include(a => a.Resume)
            .Where(a => a.JobPosting!.PostedById == userId)
            .OrderByDescending(a => a.AppliedAt)
            .Select(a => new JobApplicationDto
            {
                Id = a.Id,
                JobPostingId = a.JobPostingId,
                JobTitle = a.JobPosting!.Title,
                CompanyName = a.JobPosting.CompanyName,
                ApplicantId = a.ApplicantId,
                ApplicantName = a.Applicant!.FullName ?? "",
                ApplicantEmail = a.Applicant.Email,
                ResumeId = a.ResumeId,
                ResumeName = a.Resume != null ? a.Resume.FileName : null,
                CoverLetter = a.CoverLetter,
                Status = a.Status,
                AppliedAt = a.AppliedAt,
                ReviewedAt = a.ReviewedAt,
                Notes = a.Notes
            })
            .ToListAsync();

        return Ok(applications);
    }

    /// <summary>
    /// Update job application status (company action)
    /// </summary>
    [HttpPut("jobs/{applicationId}/status")]
    public async Task<ActionResult<JobApplicationDto>> UpdateJobApplicationStatus(
        int applicationId,
        [FromBody] UpdateJobApplicationStatusRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var application = await _context.JobApplications
            .Include(a => a.JobPosting)
            .Include(a => a.Applicant)
            .FirstOrDefaultAsync(a => a.Id == applicationId);

        if (application == null) return NotFound();

        // Only job owner or admin can update
        var isAdmin = User.IsInRole("Admin");
        if (application.JobPosting!.PostedById != userId && !isAdmin)
            return Forbid();

        var oldStatus = application.Status;
        application.Status = request.Status;
        application.Notes = request.Notes;
        application.ReviewedAt = DateTime.UtcNow;

        // Create notification for applicant
        var notification = new Notification
        {
            UserId = application.ApplicantId,
            Type = "ApplicationUpdate",
            Title = $"Application {request.Status}",
            Message = $"Your application for {application.JobPosting.Title} has been {request.Status.ToLower()}",
            Link = "/dashboard?tab=applications",
            RelatedEntityType = "JobApplication",
            RelatedEntityId = application.Id
        };
        _context.Notifications.Add(notification);

        await _context.SaveChangesAsync();

        _logger.LogInformation("Job application {Id} status changed from {OldStatus} to {NewStatus}",
            applicationId, oldStatus, request.Status);

        return Ok(new JobApplicationDto
        {
            Id = application.Id,
            JobPostingId = application.JobPostingId,
            JobTitle = application.JobPosting.Title,
            CompanyName = application.JobPosting.CompanyName,
            ApplicantId = application.ApplicantId,
            ApplicantName = application.Applicant?.FullName ?? "",
            ApplicantEmail = application.Applicant?.Email,
            Status = application.Status,
            AppliedAt = application.AppliedAt,
            ReviewedAt = application.ReviewedAt,
            Notes = application.Notes
        });
    }

    #endregion

    #region Internship Applications

    /// <summary>
    /// Apply for internship
    /// </summary>
    [HttpPost("internships")]
    public async Task<ActionResult<InternshipApplicationDto>> ApplyForInternship([FromBody] CreateInternshipApplicationRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var internship = await _context.Internships.Include(i => i.PostedBy).FirstOrDefaultAsync(i => i.Id == request.InternshipId);
        if (internship == null) return NotFound("Internship not found");

        var existingApplication = await _context.InternshipApplications
            .FirstOrDefaultAsync(a => a.InternshipId == request.InternshipId && a.ApplicantId == userId);
        if (existingApplication != null)
            return BadRequest("You have already applied for this internship");

        var user = await _context.Users.FindAsync(userId);

        var application = new InternshipApplication
        {
            InternshipId = request.InternshipId,
            ApplicantId = userId,
            ResumeId = request.ResumeId,
            CoverLetter = request.CoverLetter,
            AppliedAt = DateTime.UtcNow
        };

        _context.InternshipApplications.Add(application);

        // Create notification
        var notification = new Notification
        {
            UserId = internship.PostedById,
            Type = "InternshipApplication",
            Title = "New Internship Application",
            Message = $"{user?.FullName ?? "Someone"} applied for {internship.Title}",
            Link = $"/company-dashboard-new?page=applicants",
            RelatedEntityType = "InternshipApplication",
            RelatedEntityId = application.Id
        };
        _context.Notifications.Add(notification);

        await _context.SaveChangesAsync();

        return Ok(new InternshipApplicationDto
        {
            Id = application.Id,
            InternshipId = application.InternshipId,
            InternshipTitle = internship.Title,
            CompanyName = internship.CompanyName,
            ApplicantId = application.ApplicantId,
            ApplicantName = user?.FullName ?? "",
            ApplicantEmail = user?.Email,
            ResumeId = application.ResumeId,
            CoverLetter = application.CoverLetter,
            Status = application.Status,
            AppliedAt = application.AppliedAt
        });
    }

    /// <summary>
    /// Get internship applications for company
    /// </summary>
    [HttpGet("internships/company/all")]
    public async Task<ActionResult<List<InternshipApplicationDto>>> GetAllCompanyInternshipApplications()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var applications = await _context.InternshipApplications
            .Include(a => a.Internship)
            .Include(a => a.Applicant)
            .Include(a => a.Resume)
            .Where(a => a.Internship!.PostedById == userId)
            .OrderByDescending(a => a.AppliedAt)
            .Select(a => new InternshipApplicationDto
            {
                Id = a.Id,
                InternshipId = a.InternshipId,
                InternshipTitle = a.Internship!.Title,
                CompanyName = a.Internship.CompanyName,
                ApplicantId = a.ApplicantId,
                ApplicantName = a.Applicant!.FullName ?? "",
                ApplicantEmail = a.Applicant.Email,
                ResumeId = a.ResumeId,
                ResumeName = a.Resume != null ? a.Resume.FileName : null,
                CoverLetter = a.CoverLetter,
                Status = a.Status,
                AppliedAt = a.AppliedAt,
                ReviewedAt = a.ReviewedAt,
                Notes = a.Notes
            })
            .ToListAsync();

        return Ok(applications);
    }

    /// <summary>
    /// Update internship application status
    /// </summary>
    [HttpPut("internships/{applicationId}/status")]
    public async Task<ActionResult<InternshipApplicationDto>> UpdateInternshipApplicationStatus(
        int applicationId,
        [FromBody] UpdateJobApplicationStatusRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var application = await _context.InternshipApplications
            .Include(a => a.Internship)
            .Include(a => a.Applicant)
            .FirstOrDefaultAsync(a => a.Id == applicationId);

        if (application == null) return NotFound();

        var isAdmin = User.IsInRole("Admin");
        if (application.Internship!.PostedById != userId && !isAdmin)
            return Forbid();

        application.Status = request.Status;
        application.Notes = request.Notes;
        application.ReviewedAt = DateTime.UtcNow;

        // Create notification
        var notification = new Notification
        {
            UserId = application.ApplicantId,
            Type = "ApplicationUpdate",
            Title = $"Internship Application {request.Status}",
            Message = $"Your application for {application.Internship.Title} has been {request.Status.ToLower()}",
            Link = "/dashboard?tab=applications",
            RelatedEntityType = "InternshipApplication",
            RelatedEntityId = application.Id
        };
        _context.Notifications.Add(notification);

        await _context.SaveChangesAsync();

        return Ok(new InternshipApplicationDto
        {
            Id = application.Id,
            InternshipId = application.InternshipId,
            InternshipTitle = application.Internship.Title,
            CompanyName = application.Internship.CompanyName,
            ApplicantId = application.ApplicantId,
            ApplicantName = application.Applicant?.FullName ?? "",
            ApplicantEmail = application.Applicant?.Email,
            Status = application.Status,
            AppliedAt = application.AppliedAt,
            ReviewedAt = application.ReviewedAt,
            Notes = application.Notes
        });
    }

    #endregion

    #region My Applications (Student View)

    /// <summary>
    /// Get all my applications (student view)
    /// </summary>
    [HttpGet("my")]
    public async Task<ActionResult<List<MyApplicationDto>>> GetMyApplications()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var jobApplications = await _context.JobApplications
            .Include(a => a.JobPosting)
            .Where(a => a.ApplicantId == userId)
            .Select(a => new MyApplicationDto
            {
                Id = a.Id,
                Type = "Job",
                OpportunityId = a.JobPostingId,
                OpportunityTitle = a.JobPosting!.Title,
                CompanyName = a.JobPosting.CompanyName,
                Location = a.JobPosting.Location,
                Status = a.Status,
                AppliedAt = a.AppliedAt,
                ReviewedAt = a.ReviewedAt
            })
            .ToListAsync();

        var internshipApplications = await _context.InternshipApplications
            .Include(a => a.Internship)
            .Where(a => a.ApplicantId == userId)
            .Select(a => new MyApplicationDto
            {
                Id = a.Id,
                Type = "Internship",
                OpportunityId = a.InternshipId,
                OpportunityTitle = a.Internship!.Title,
                CompanyName = a.Internship.CompanyName,
                Location = a.Internship.Location,
                Status = a.Status,
                AppliedAt = a.AppliedAt,
                ReviewedAt = a.ReviewedAt
            })
            .ToListAsync();

        var allApplications = jobApplications
            .Concat(internshipApplications)
            .OrderByDescending(a => a.AppliedAt)
            .ToList();

        return Ok(allApplications);
    }

    /// <summary>
    /// Check if user has applied for a job
    /// </summary>
    [HttpGet("jobs/{jobId}/check")]
    public async Task<ActionResult<bool>> HasAppliedForJob(int jobId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var hasApplied = await _context.JobApplications
            .AnyAsync(a => a.JobPostingId == jobId && a.ApplicantId == userId);

        return Ok(hasApplied);
    }

    /// <summary>
    /// Check if user has applied for an internship
    /// </summary>
    [HttpGet("internships/{internshipId}/check")]
    public async Task<ActionResult<bool>> HasAppliedForInternship(int internshipId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var hasApplied = await _context.InternshipApplications
            .AnyAsync(a => a.InternshipId == internshipId && a.ApplicantId == userId);

        return Ok(hasApplied);
    }

    #endregion
}

















