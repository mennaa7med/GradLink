using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GradLink.Infrastructure.Persistence;
using GradLink.Domain.Entities;
using GradLink.Application.DTOs.Sponsors;
using System.Text.Json;

namespace GradLink.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/sponsor-dashboard")]
public class SponsorDashboardController : ControllerBase
{
    private readonly AppDbContext _context;

    public SponsorDashboardController(AppDbContext context)
    {
        _context = context;
    }

    private string? GetUserId() => User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

    // ==================== Overview ====================

    /// <summary>
    /// Get dashboard overview with statistics
    /// </summary>
    [HttpGet("overview")]
    public async Task<ActionResult<SponsorDashboardOverview>> GetOverview()
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var profile = await _context.SponsorProfiles
            .Include(sp => sp.SponsoredProjects)
            .Include(sp => sp.Fundings)
            .FirstOrDefaultAsync(sp => sp.UserId == userId);

        if (profile == null)
            return NotFound(new { error = "Sponsor profile not found" });

        var projects = profile.SponsoredProjects.ToList();
        var fundings = profile.Fundings.Where(f => f.Status == "Completed").ToList();

        var unreadMessages = await _context.SponsorMessages
            .CountAsync(m => m.ReceiverId == userId && !m.IsRead);

        var overview = new SponsorDashboardOverview
        {
            TotalProjectsSponsored = projects.Count,
            PendingRequests = projects.Count(p => p.Status == "Pending"),
            ApprovedProjects = projects.Count(p => p.Status == "Approved"),
            RejectedProjects = projects.Count(p => p.Status == "Rejected"),
            CompletedProjects = projects.Count(p => p.Status == "Completed"),
            ActiveProjects = projects.Count(p => p.Status == "Approved" && p.ProgressPercentage < 100),
            TotalFundingProvided = fundings.Sum(f => f.Amount),
            TotalFundingDelivered = projects.Sum(p => p.FundingDelivered ?? 0),
            RemainingBudget = profile.RemainingBudget ?? 0,
            UnreadMessages = unreadMessages,
            ProjectsByCategory = projects
                .GroupBy(p => p.Category ?? "Other")
                .Select(g => new CategoryStats
                {
                    Category = g.Key,
                    Count = g.Count(),
                    TotalFunding = g.Sum(p => p.FundingAmount ?? 0)
                }).ToList(),
            MonthlyStats = projects
                .Where(p => p.ApprovedAt != null || p.RejectedAt != null)
                .GroupBy(p => (p.ApprovedAt ?? p.RejectedAt ?? p.RequestedAt).ToString("yyyy-MM"))
                .OrderByDescending(g => g.Key)
                .Take(6)
                .Select(g => new MonthlyStats
                {
                    Month = g.Key,
                    ProjectsApproved = g.Count(p => p.Status == "Approved" || p.Status == "Completed"),
                    ProjectsRejected = g.Count(p => p.Status == "Rejected"),
                    FundingProvided = g.Sum(p => p.FundingDelivered ?? 0)
                }).ToList()
        };

        return Ok(overview);
    }

    // ==================== Profile ====================

    /// <summary>
    /// Get sponsor profile
    /// </summary>
    [HttpGet("profile")]
    public async Task<ActionResult<SponsorProfileDto>> GetProfile()
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var profile = await _context.SponsorProfiles
            .FirstOrDefaultAsync(sp => sp.UserId == userId);

        if (profile == null)
            return NotFound(new { error = "Sponsor profile not found" });

        return Ok(MapToProfileDto(profile));
    }

    /// <summary>
    /// Update sponsor profile
    /// </summary>
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateSponsorProfileRequest request)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var profile = await _context.SponsorProfiles
            .FirstOrDefaultAsync(sp => sp.UserId == userId);

        if (profile == null)
            return NotFound(new { error = "Sponsor profile not found" });

        if (request.CompanyName != null) profile.CompanyName = request.CompanyName;
        if (request.CompanyDescription != null) profile.CompanyDescription = request.CompanyDescription;
        if (request.Logo != null) profile.Logo = request.Logo;
        if (request.Website != null) profile.Website = request.Website;
        if (request.Industry != null) profile.Industry = request.Industry;
        if (request.ContactPersonName != null) profile.ContactPersonName = request.ContactPersonName;
        if (request.ContactEmail != null) profile.ContactEmail = request.ContactEmail;
        if (request.ContactPhone != null) profile.ContactPhone = request.ContactPhone;
        if (request.Address != null) profile.Address = request.Address;
        if (request.FieldsOfInterest != null) profile.FieldsOfInterest = request.FieldsOfInterest;
        if (request.TotalBudget.HasValue)
        {
            var difference = request.TotalBudget.Value - (profile.TotalBudget ?? 0);
            profile.TotalBudget = request.TotalBudget.Value;
            profile.RemainingBudget = (profile.RemainingBudget ?? 0) + difference;
        }
        profile.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(MapToProfileDto(profile));
    }

    // ==================== Projects ====================

    /// <summary>
    /// Get all sponsored projects
    /// </summary>
    [HttpGet("projects")]
    public async Task<ActionResult<IEnumerable<SponsoredProjectDto>>> GetProjects(
        [FromQuery] string? status = null,
        [FromQuery] string? category = null)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var profile = await _context.SponsorProfiles
            .FirstOrDefaultAsync(sp => sp.UserId == userId);

        if (profile == null)
            return NotFound(new { error = "Sponsor profile not found" });

        var query = _context.SponsoredProjects
            .Where(sp => sp.SponsorProfileId == profile.Id);

        if (!string.IsNullOrEmpty(status))
            query = query.Where(sp => sp.Status == status);

        if (!string.IsNullOrEmpty(category))
            query = query.Where(sp => sp.Category == category);

        var projects = await query
            .OrderByDescending(sp => sp.RequestedAt)
            .ToListAsync();

        return Ok(projects.Select(MapToProjectDto));
    }

    /// <summary>
    /// Get single project details
    /// </summary>
    [HttpGet("projects/{id}")]
    public async Task<ActionResult<SponsoredProjectDto>> GetProject(int id)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var profile = await _context.SponsorProfiles
            .FirstOrDefaultAsync(sp => sp.UserId == userId);

        if (profile == null)
            return NotFound(new { error = "Sponsor profile not found" });

        var project = await _context.SponsoredProjects
            .FirstOrDefaultAsync(sp => sp.Id == id && sp.SponsorProfileId == profile.Id);

        if (project == null)
            return NotFound(new { error = "Project not found" });

        return Ok(MapToProjectDto(project));
    }

    /// <summary>
    /// Approve or reject a project
    /// </summary>
    [HttpPost("projects/approve-reject")]
    public async Task<IActionResult> ApproveRejectProject([FromBody] ApproveRejectProjectRequest request)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var profile = await _context.SponsorProfiles
            .FirstOrDefaultAsync(sp => sp.UserId == userId);

        if (profile == null)
            return NotFound(new { error = "Sponsor profile not found" });

        var project = await _context.SponsoredProjects
            .FirstOrDefaultAsync(sp => sp.Id == request.SponsoredProjectId && sp.SponsorProfileId == profile.Id);

        if (project == null)
            return NotFound(new { error = "Project not found" });

        if (project.Status != "Pending")
            return BadRequest(new { error = "Project has already been processed" });

        if (request.Approve)
        {
            project.Status = "Approved";
            project.ApprovedAt = DateTime.UtcNow;
            if (request.FundingAmount.HasValue)
            {
                project.FundingAmount = request.FundingAmount.Value;
            }
            profile.ApprovedProjects = (profile.ApprovedProjects) + 1;
            profile.ActiveProjects = (profile.ActiveProjects) + 1;
        }
        else
        {
            project.Status = "Rejected";
            project.RejectedAt = DateTime.UtcNow;
            project.RejectionReason = request.Reason;
        }

        project.UpdatedAt = DateTime.UtcNow;
        profile.PendingRequests = Math.Max(0, profile.PendingRequests - 1);

        await _context.SaveChangesAsync();
        return Ok(MapToProjectDto(project));
    }

    /// <summary>
    /// Update project progress
    /// </summary>
    [HttpPut("projects/{id}/progress")]
    public async Task<IActionResult> UpdateProjectProgress(int id, [FromBody] UpdateProjectProgressRequest request)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var profile = await _context.SponsorProfiles
            .FirstOrDefaultAsync(sp => sp.UserId == userId);

        if (profile == null)
            return NotFound(new { error = "Sponsor profile not found" });

        var project = await _context.SponsoredProjects
            .FirstOrDefaultAsync(sp => sp.Id == id && sp.SponsorProfileId == profile.Id);

        if (project == null)
            return NotFound(new { error = "Project not found" });

        if (request.ProgressPercentage.HasValue)
            project.ProgressPercentage = request.ProgressPercentage.Value;

        if (request.CurrentMilestone != null)
            project.CurrentMilestone = request.CurrentMilestone;

        if (request.Milestones != null)
            project.Milestones = JsonSerializer.Serialize(request.Milestones);

        if (project.ProgressPercentage >= 100 && project.Status == "Approved")
        {
            project.Status = "Completed";
            project.CompletedAt = DateTime.UtcNow;
            profile.CompletedProjects = (profile.CompletedProjects) + 1;
            profile.ActiveProjects = Math.Max(0, profile.ActiveProjects - 1);
        }

        project.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(MapToProjectDto(project));
    }

    // ==================== Funding ====================

    /// <summary>
    /// Get funding history
    /// </summary>
    [HttpGet("funding")]
    public async Task<ActionResult<IEnumerable<SponsorFundingDto>>> GetFundingHistory()
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var profile = await _context.SponsorProfiles
            .FirstOrDefaultAsync(sp => sp.UserId == userId);

        if (profile == null)
            return NotFound(new { error = "Sponsor profile not found" });

        var fundings = await _context.SponsorFundings
            .Include(sf => sf.SponsoredProject)
            .Where(sf => sf.SponsorProfileId == profile.Id)
            .OrderByDescending(sf => sf.CreatedAt)
            .ToListAsync();

        return Ok(fundings.Select(f => new SponsorFundingDto
        {
            Id = f.Id,
            Amount = f.Amount,
            TransactionType = f.TransactionType,
            Status = f.Status,
            TransactionReference = f.TransactionReference,
            Notes = f.Notes,
            ProjectTitle = f.SponsoredProject?.ProjectTitle,
            CreatedAt = f.CreatedAt,
            CompletedAt = f.CompletedAt
        }));
    }

    /// <summary>
    /// Add funding to a project
    /// </summary>
    [HttpPost("funding")]
    public async Task<ActionResult<SponsorFundingDto>> AddFunding([FromBody] CreateFundingRequest request)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var profile = await _context.SponsorProfiles
            .FirstOrDefaultAsync(sp => sp.UserId == userId);

        if (profile == null)
            return NotFound(new { error = "Sponsor profile not found" });

        var project = await _context.SponsoredProjects
            .FirstOrDefaultAsync(sp => sp.Id == request.SponsoredProjectId && sp.SponsorProfileId == profile.Id);

        if (project == null)
            return NotFound(new { error = "Project not found" });

        if (project.Status != "Approved")
            return BadRequest(new { error = "Can only fund approved projects" });

        if (profile.RemainingBudget < request.Amount)
            return BadRequest(new { error = "Insufficient budget" });

        var funding = new SponsorFunding
        {
            Amount = request.Amount,
            TransactionType = "Payment",
            Status = "Completed",
            TransactionReference = request.TransactionReference,
            Notes = request.Notes,
            SponsorProfileId = profile.Id,
            SponsoredProjectId = project.Id,
            CreatedAt = DateTime.UtcNow,
            CompletedAt = DateTime.UtcNow
        };

        _context.SponsorFundings.Add(funding);

        project.FundingDelivered = (project.FundingDelivered ?? 0) + request.Amount;
        profile.RemainingBudget = (profile.RemainingBudget ?? 0) - request.Amount;

        await _context.SaveChangesAsync();

        return Ok(new SponsorFundingDto
        {
            Id = funding.Id,
            Amount = funding.Amount,
            TransactionType = funding.TransactionType,
            Status = funding.Status,
            TransactionReference = funding.TransactionReference,
            Notes = funding.Notes,
            ProjectTitle = project.ProjectTitle,
            CreatedAt = funding.CreatedAt,
            CompletedAt = funding.CompletedAt
        });
    }

    // ==================== Messages ====================

    /// <summary>
    /// Get messages
    /// </summary>
    [HttpGet("messages")]
    public async Task<ActionResult<IEnumerable<SponsorMessageDto>>> GetMessages([FromQuery] int? projectId = null)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var profile = await _context.SponsorProfiles
            .FirstOrDefaultAsync(sp => sp.UserId == userId);

        if (profile == null)
            return NotFound(new { error = "Sponsor profile not found" });

        var query = _context.SponsorMessages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Include(m => m.SponsoredProject)
            .Where(m => m.SponsorProfileId == profile.Id);

        if (projectId.HasValue)
            query = query.Where(m => m.SponsoredProjectId == projectId);

        var messages = await query
            .OrderByDescending(m => m.SentAt)
            .ToListAsync();

        return Ok(messages.Select(m => new SponsorMessageDto
        {
            Id = m.Id,
            Content = m.Content,
            Attachments = string.IsNullOrEmpty(m.Attachments) 
                ? new List<string>() 
                : JsonSerializer.Deserialize<List<string>>(m.Attachments) ?? new List<string>(),
            IsRead = m.IsRead,
            SenderId = m.SenderId,
            SenderName = m.Sender?.FullName,
            ReceiverId = m.ReceiverId,
            ReceiverName = m.Receiver?.FullName,
            IsSponsorSender = m.IsSponsorSender,
            ProjectTitle = m.SponsoredProject?.ProjectTitle,
            SentAt = m.SentAt,
            ReadAt = m.ReadAt
        }));
    }

    /// <summary>
    /// Send a message
    /// </summary>
    [HttpPost("messages")]
    public async Task<ActionResult<SponsorMessageDto>> SendMessage([FromBody] SendMessageRequest request)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var profile = await _context.SponsorProfiles
            .FirstOrDefaultAsync(sp => sp.UserId == userId);

        if (profile == null)
            return NotFound(new { error = "Sponsor profile not found" });

        var message = new SponsorMessage
        {
            Content = request.Content,
            Attachments = request.Attachments != null ? JsonSerializer.Serialize(request.Attachments) : null,
            SenderId = userId,
            ReceiverId = request.ReceiverId,
            IsSponsorSender = true,
            SponsorProfileId = profile.Id,
            SponsoredProjectId = request.SponsoredProjectId,
            SentAt = DateTime.UtcNow
        };

        _context.SponsorMessages.Add(message);
        await _context.SaveChangesAsync();

        var sender = await _context.Users.FindAsync(userId);
        var receiver = await _context.Users.FindAsync(request.ReceiverId);

        return Ok(new SponsorMessageDto
        {
            Id = message.Id,
            Content = message.Content,
            Attachments = request.Attachments ?? new List<string>(),
            IsRead = false,
            SenderId = userId,
            SenderName = sender?.FullName,
            ReceiverId = request.ReceiverId,
            ReceiverName = receiver?.FullName,
            IsSponsorSender = true,
            SentAt = message.SentAt
        });
    }

    /// <summary>
    /// Mark message as read
    /// </summary>
    [HttpPut("messages/{id}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var message = await _context.SponsorMessages
            .FirstOrDefaultAsync(m => m.Id == id && m.ReceiverId == userId);

        if (message == null)
            return NotFound();

        message.IsRead = true;
        message.ReadAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // ==================== Available Projects (for sponsoring) ====================

    /// <summary>
    /// Get available projects to sponsor
    /// </summary>
    [HttpGet("available-projects")]
    public async Task<ActionResult> GetAvailableProjects(
        [FromQuery] string? category = null,
        [FromQuery] string? search = null)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var profile = await _context.SponsorProfiles
            .FirstOrDefaultAsync(sp => sp.UserId == userId);

        if (profile == null)
            return NotFound(new { error = "Sponsor profile not found" });

        // Get project IDs already sponsored by this sponsor
        var sponsoredProjectIds = await _context.SponsoredProjects
            .Where(sp => sp.SponsorProfileId == profile.Id)
            .Select(sp => sp.ProjectId)
            .ToListAsync();

        var query = _context.Projects
            .Include(p => p.Owner)
            .Include(p => p.TeamMembers)
            .Where(p => p.Status == "Active" && !sponsoredProjectIds.Contains(p.Id));

        if (!string.IsNullOrEmpty(category))
            query = query.Where(p => p.Category == category);

        if (!string.IsNullOrEmpty(search))
            query = query.Where(p => p.Title.Contains(search) || 
                (p.Description != null && p.Description.Contains(search)));

        var projects = await query
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new
            {
                p.Id,
                p.Title,
                p.Description,
                p.Category,
                p.Status,
                OwnerName = p.Owner != null ? p.Owner.FullName : null,
                TeamSize = p.TeamMembers.Count,
                p.CreatedAt
            })
            .ToListAsync();

        return Ok(projects);
    }

    /// <summary>
    /// Request to sponsor a project
    /// </summary>
    [HttpPost("sponsor-project")]
    public async Task<ActionResult<SponsoredProjectDto>> SponsorProject([FromBody] SponsorProjectRequest request)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var profile = await _context.SponsorProfiles
            .FirstOrDefaultAsync(sp => sp.UserId == userId);

        if (profile == null)
            return NotFound(new { error = "Sponsor profile not found" });

        var project = await _context.Projects
            .Include(p => p.Owner)
            .Include(p => p.TeamMembers)
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId);

        if (project == null)
            return NotFound(new { error = "Project not found" });

        // Check if already sponsoring
        var existing = await _context.SponsoredProjects
            .AnyAsync(sp => sp.SponsorProfileId == profile.Id && sp.ProjectId == project.Id);

        if (existing)
            return BadRequest(new { error = "You are already sponsoring this project" });

        var sponsoredProject = new SponsoredProject
        {
            ProjectTitle = project.Title,
            ProjectDescription = project.Description,
            StudentName = project.Owner?.FullName,
            TeamName = project.TeamMembers.Any() ? $"{project.Owner?.FullName}'s Team" : null,
            Category = project.Category,
            Status = "Approved", // Auto-approve when sponsor requests
            FundingAmount = request.FundingAmount,
            SponsorProfileId = profile.Id,
            ProjectId = project.Id,
            StudentUserId = project.OwnerId,
            RequestedAt = DateTime.UtcNow,
            ApprovedAt = DateTime.UtcNow
        };

        _context.SponsoredProjects.Add(sponsoredProject);

        profile.TotalProjectsSponsored++;
        profile.ApprovedProjects++;
        profile.ActiveProjects++;

        await _context.SaveChangesAsync();

        // Send initial message if provided
        if (!string.IsNullOrEmpty(request.Message))
        {
            var message = new SponsorMessage
            {
                Content = request.Message,
                SenderId = userId,
                ReceiverId = project.OwnerId,
                IsSponsorSender = true,
                SponsorProfileId = profile.Id,
                SponsoredProjectId = sponsoredProject.Id,
                SentAt = DateTime.UtcNow
            };
            _context.SponsorMessages.Add(message);
            await _context.SaveChangesAsync();
        }

        return Ok(MapToProjectDto(sponsoredProject));
    }

    // ==================== Helper Methods ====================

    private static SponsorProfileDto MapToProfileDto(SponsorProfile profile)
    {
        return new SponsorProfileDto
        {
            Id = profile.Id,
            CompanyName = profile.CompanyName,
            CompanyDescription = profile.CompanyDescription,
            Logo = profile.Logo,
            Website = profile.Website,
            Industry = profile.Industry,
            ContactPersonName = profile.ContactPersonName,
            ContactEmail = profile.ContactEmail,
            ContactPhone = profile.ContactPhone,
            Address = profile.Address,
            FieldsOfInterest = string.IsNullOrEmpty(profile.FieldsOfInterest)
                ? new List<string>()
                : profile.FieldsOfInterest.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(f => f.Trim()).ToList(),
            TotalBudget = profile.TotalBudget,
            RemainingBudget = profile.RemainingBudget,
            TotalProjectsSponsored = profile.TotalProjectsSponsored,
            ActiveProjects = profile.ActiveProjects,
            CompletedProjects = profile.CompletedProjects,
            PendingRequests = profile.PendingRequests,
            IsVerified = profile.IsVerified,
            CreatedAt = profile.CreatedAt
        };
    }

    private static SponsoredProjectDto MapToProjectDto(SponsoredProject project)
    {
        return new SponsoredProjectDto
        {
            Id = project.Id,
            ProjectTitle = project.ProjectTitle,
            ProjectDescription = project.ProjectDescription,
            StudentName = project.StudentName,
            TeamName = project.TeamName,
            Category = project.Category,
            Status = project.Status,
            RejectionReason = project.RejectionReason,
            FundingAmount = project.FundingAmount,
            FundingDelivered = project.FundingDelivered,
            ProgressPercentage = project.ProgressPercentage,
            CurrentMilestone = project.CurrentMilestone,
            Milestones = string.IsNullOrEmpty(project.Milestones)
                ? new List<MilestoneDto>()
                : JsonSerializer.Deserialize<List<MilestoneDto>>(project.Milestones) ?? new List<MilestoneDto>(),
            Documents = string.IsNullOrEmpty(project.Documents)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(project.Documents) ?? new List<string>(),
            RequestedAt = project.RequestedAt,
            ApprovedAt = project.ApprovedAt,
            ProjectId = project.ProjectId,
            StudentUserId = project.StudentUserId
        };
    }
}

