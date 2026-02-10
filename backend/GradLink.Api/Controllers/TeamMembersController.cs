using GradLink.Application.DTOs.Teams;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TeamMembersController : ControllerBase
{
    private readonly AppDbContext _context;

    public TeamMembersController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all team members across all user's projects
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamMemberDto>>> GetAllTeamMembers()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var teamMembers = await _context.TeamMembers
            .Include(tm => tm.Project)
            .Include(tm => tm.Tasks)
            .Where(tm => tm.Project!.OwnerId == userId)
            .OrderByDescending(tm => tm.CreatedAt)
            .Take(10)
            .Select(tm => new TeamMemberDto
            {
                Id = tm.Id,
                FullName = tm.FullName,
                Email = tm.Email,
                ProfileImageUrl = tm.ProfileImageUrl,
                University = tm.University,
                GraduationYear = tm.GraduationYear,
                Role = tm.Role,
                Skills = tm.Skills,
                Availability = tm.Availability,
                Rating = tm.Rating,
                ProjectContributions = tm.ProjectContributions,
                TotalTasks = tm.TotalTasks,
                TasksCompleted = tm.TasksCompleted,
                CreatedAt = tm.CreatedAt,
                UpdatedAt = tm.UpdatedAt,
                ProjectId = tm.ProjectId,
                ProjectTitle = tm.Project!.Title,
                UserId = tm.UserId
            })
            .ToListAsync();

        return Ok(teamMembers);
    }

    /// <summary>
    /// Get all team members for a specific project
    /// </summary>
    [HttpGet("project/{projectId}")]
    public async Task<ActionResult<IEnumerable<TeamMemberDto>>> GetTeamMembersByProject(int projectId)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        // Verify the user has access to this project
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId && p.OwnerId == userId);

        if (project == null)
        {
            return NotFound(new { error = "Project not found or you don't have access" });
        }

        var teamMembers = await _context.TeamMembers
            .Where(tm => tm.ProjectId == projectId)
            .Include(tm => tm.Tasks)
            .OrderBy(tm => tm.FullName)
            .Select(tm => new TeamMemberDto
            {
                Id = tm.Id,
                FullName = tm.FullName,
                Email = tm.Email,
                ProfileImageUrl = tm.ProfileImageUrl,
                University = tm.University,
                GraduationYear = tm.GraduationYear,
                Role = tm.Role,
                Skills = tm.Skills,
                Availability = tm.Availability,
                Rating = tm.Rating,
                ProjectContributions = tm.ProjectContributions,
                TotalTasks = tm.TotalTasks,
                TasksCompleted = tm.TasksCompleted,
                CreatedAt = tm.CreatedAt,
                UpdatedAt = tm.UpdatedAt,
                ProjectId = tm.ProjectId,
                ProjectTitle = tm.Project!.Title,
                UserId = tm.UserId,
                Tasks = tm.Tasks.Select(t => new TeamMemberTaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status,
                    Priority = t.Priority,
                    DueDate = t.DueDate,
                    CompletedAt = t.CompletedAt,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt,
                    TeamMemberId = t.TeamMemberId
                }).ToList()
            })
            .ToListAsync();

        return Ok(teamMembers);
    }

    /// <summary>
    /// Get a specific team member by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TeamMemberDto>> GetTeamMember(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var teamMember = await _context.TeamMembers
            .Include(tm => tm.Project)
            .Include(tm => tm.Tasks)
            .FirstOrDefaultAsync(tm => tm.Id == id && tm.Project!.OwnerId == userId);

        if (teamMember == null)
        {
            return NotFound(new { error = "Team member not found or you don't have access" });
        }

        var dto = new TeamMemberDto
        {
            Id = teamMember.Id,
            FullName = teamMember.FullName,
            Email = teamMember.Email,
            ProfileImageUrl = teamMember.ProfileImageUrl,
            University = teamMember.University,
            GraduationYear = teamMember.GraduationYear,
            Role = teamMember.Role,
            Skills = teamMember.Skills,
            Availability = teamMember.Availability,
            Rating = teamMember.Rating,
            ProjectContributions = teamMember.ProjectContributions,
            TotalTasks = teamMember.TotalTasks,
            TasksCompleted = teamMember.TasksCompleted,
            CreatedAt = teamMember.CreatedAt,
            UpdatedAt = teamMember.UpdatedAt,
            ProjectId = teamMember.ProjectId,
            ProjectTitle = teamMember.Project?.Title,
            UserId = teamMember.UserId,
            Tasks = teamMember.Tasks.Select(t => new TeamMemberTaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                Priority = t.Priority,
                DueDate = t.DueDate,
                CompletedAt = t.CompletedAt,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                TeamMemberId = t.TeamMemberId
            }).ToList()
        };

        return Ok(dto);
    }

    /// <summary>
    /// Add a new team member to a project
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TeamMemberDto>> CreateTeamMember([FromBody] CreateTeamMemberRequest request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        // Verify the user owns this project
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId && p.OwnerId == userId);

        if (project == null)
        {
            return NotFound(new { error = "Project not found or you don't have access" });
        }

        // Check if email already exists in this project
        var existingMember = await _context.TeamMembers
            .FirstOrDefaultAsync(tm => tm.ProjectId == request.ProjectId && tm.Email.ToLower() == request.Email.ToLower());

        if (existingMember != null)
        {
            return BadRequest(new { error = "A team member with this email already exists in this project" });
        }

        var teamMember = new TeamMember
        {
            FullName = request.FullName,
            Email = request.Email,
            ProfileImageUrl = request.ProfileImageUrl,
            University = request.University,
            GraduationYear = request.GraduationYear,
            Role = request.Role,
            Skills = request.Skills,
            Availability = request.Availability,
            ProjectId = request.ProjectId,
            CreatedAt = DateTime.UtcNow
        };

        _context.TeamMembers.Add(teamMember);
        await _context.SaveChangesAsync();

        var dto = new TeamMemberDto
        {
            Id = teamMember.Id,
            FullName = teamMember.FullName,
            Email = teamMember.Email,
            ProfileImageUrl = teamMember.ProfileImageUrl,
            University = teamMember.University,
            GraduationYear = teamMember.GraduationYear,
            Role = teamMember.Role,
            Skills = teamMember.Skills,
            Availability = teamMember.Availability,
            Rating = teamMember.Rating,
            ProjectContributions = teamMember.ProjectContributions,
            TotalTasks = teamMember.TotalTasks,
            TasksCompleted = teamMember.TasksCompleted,
            CreatedAt = teamMember.CreatedAt,
            UpdatedAt = teamMember.UpdatedAt,
            ProjectId = teamMember.ProjectId,
            ProjectTitle = project.Title,
            UserId = teamMember.UserId
        };

        return CreatedAtAction(nameof(GetTeamMember), new { id = teamMember.Id }, dto);
    }

    /// <summary>
    /// Update a team member
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTeamMember(int id, [FromBody] UpdateTeamMemberRequest request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var teamMember = await _context.TeamMembers
            .Include(tm => tm.Project)
            .FirstOrDefaultAsync(tm => tm.Id == id && tm.Project!.OwnerId == userId);

        if (teamMember == null)
        {
            return NotFound(new { error = "Team member not found or you don't have access" });
        }

        // Check email uniqueness if being updated
        if (!string.IsNullOrEmpty(request.Email) && request.Email.ToLower() != teamMember.Email.ToLower())
        {
            var emailExists = await _context.TeamMembers
                .AnyAsync(tm => tm.ProjectId == teamMember.ProjectId && tm.Email.ToLower() == request.Email.ToLower() && tm.Id != id);

            if (emailExists)
            {
                return BadRequest(new { error = "A team member with this email already exists in this project" });
            }
        }

        // Update fields if provided
        if (!string.IsNullOrEmpty(request.FullName)) teamMember.FullName = request.FullName;
        if (!string.IsNullOrEmpty(request.Email)) teamMember.Email = request.Email;
        if (request.ProfileImageUrl != null) teamMember.ProfileImageUrl = request.ProfileImageUrl;
        if (request.University != null) teamMember.University = request.University;
        if (request.GraduationYear.HasValue) teamMember.GraduationYear = request.GraduationYear;
        if (!string.IsNullOrEmpty(request.Role)) teamMember.Role = request.Role;
        if (request.Skills != null) teamMember.Skills = request.Skills;
        if (request.Availability != null) teamMember.Availability = request.Availability;
        if (request.Rating.HasValue) teamMember.Rating = request.Rating;
        if (request.ProjectContributions.HasValue) teamMember.ProjectContributions = request.ProjectContributions.Value;

        teamMember.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Delete a team member
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeamMember(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var teamMember = await _context.TeamMembers
            .Include(tm => tm.Project)
            .FirstOrDefaultAsync(tm => tm.Id == id && tm.Project!.OwnerId == userId);

        if (teamMember == null)
        {
            return NotFound(new { error = "Team member not found or you don't have access" });
        }

        _context.TeamMembers.Remove(teamMember);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    #region Team Member Tasks

    /// <summary>
    /// Get all tasks for a team member
    /// </summary>
    [HttpGet("{memberId}/tasks")]
    public async Task<ActionResult<IEnumerable<TeamMemberTaskDto>>> GetTeamMemberTasks(int memberId)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var teamMember = await _context.TeamMembers
            .Include(tm => tm.Project)
            .FirstOrDefaultAsync(tm => tm.Id == memberId && tm.Project!.OwnerId == userId);

        if (teamMember == null)
        {
            return NotFound(new { error = "Team member not found or you don't have access" });
        }

        var tasks = await _context.TeamMemberTasks
            .Where(t => t.TeamMemberId == memberId)
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TeamMemberTaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                Priority = t.Priority,
                DueDate = t.DueDate,
                CompletedAt = t.CompletedAt,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                TeamMemberId = t.TeamMemberId
            })
            .ToListAsync();

        return Ok(tasks);
    }

    /// <summary>
    /// Add a task to a team member
    /// </summary>
    [HttpPost("tasks")]
    public async Task<ActionResult<TeamMemberTaskDto>> CreateTeamMemberTask([FromBody] CreateTeamMemberTaskRequest request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var teamMember = await _context.TeamMembers
            .Include(tm => tm.Project)
            .FirstOrDefaultAsync(tm => tm.Id == request.TeamMemberId && tm.Project!.OwnerId == userId);

        if (teamMember == null)
        {
            return NotFound(new { error = "Team member not found or you don't have access" });
        }

        var task = new TeamMemberTask
        {
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority ?? "Medium",
            DueDate = request.DueDate,
            TeamMemberId = request.TeamMemberId,
            CreatedAt = DateTime.UtcNow
        };

        _context.TeamMemberTasks.Add(task);

        // Update task statistics
        teamMember.TotalTasks++;
        teamMember.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var dto = new TeamMemberTaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            DueDate = task.DueDate,
            CompletedAt = task.CompletedAt,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            TeamMemberId = task.TeamMemberId
        };

        return CreatedAtAction(nameof(GetTeamMemberTasks), new { memberId = task.TeamMemberId }, dto);
    }

    /// <summary>
    /// Update a team member task
    /// </summary>
    [HttpPut("tasks/{taskId}")]
    public async Task<IActionResult> UpdateTeamMemberTask(int taskId, [FromBody] UpdateTeamMemberTaskRequest request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var task = await _context.TeamMemberTasks
            .Include(t => t.TeamMember)
                .ThenInclude(tm => tm!.Project)
            .FirstOrDefaultAsync(t => t.Id == taskId && t.TeamMember!.Project!.OwnerId == userId);

        if (task == null)
        {
            return NotFound(new { error = "Task not found or you don't have access" });
        }

        var wasCompleted = task.Status == "Completed";

        // Update fields if provided
        if (!string.IsNullOrEmpty(request.Title)) task.Title = request.Title;
        if (request.Description != null) task.Description = request.Description;
        if (!string.IsNullOrEmpty(request.Status)) task.Status = request.Status;
        if (request.Priority != null) task.Priority = request.Priority;
        if (request.DueDate.HasValue) task.DueDate = request.DueDate;

        task.UpdatedAt = DateTime.UtcNow;

        // Handle completion status change
        var isNowCompleted = task.Status == "Completed";
        if (!wasCompleted && isNowCompleted)
        {
            task.CompletedAt = DateTime.UtcNow;
            task.TeamMember!.TasksCompleted++;
            task.TeamMember.ProjectContributions++;
            task.TeamMember.UpdatedAt = DateTime.UtcNow;
        }
        else if (wasCompleted && !isNowCompleted)
        {
            task.CompletedAt = null;
            task.TeamMember!.TasksCompleted = Math.Max(0, task.TeamMember.TasksCompleted - 1);
            task.TeamMember.ProjectContributions = Math.Max(0, task.TeamMember.ProjectContributions - 1);
            task.TeamMember.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Delete a team member task
    /// </summary>
    [HttpDelete("tasks/{taskId}")]
    public async Task<IActionResult> DeleteTeamMemberTask(int taskId)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var task = await _context.TeamMemberTasks
            .Include(t => t.TeamMember)
                .ThenInclude(tm => tm!.Project)
            .FirstOrDefaultAsync(t => t.Id == taskId && t.TeamMember!.Project!.OwnerId == userId);

        if (task == null)
        {
            return NotFound(new { error = "Task not found or you don't have access" });
        }

        // Update task statistics
        task.TeamMember!.TotalTasks = Math.Max(0, task.TeamMember.TotalTasks - 1);
        if (task.Status == "Completed")
        {
            task.TeamMember.TasksCompleted = Math.Max(0, task.TeamMember.TasksCompleted - 1);
        }
        task.TeamMember.UpdatedAt = DateTime.UtcNow;

        _context.TeamMemberTasks.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    #endregion

    /// <summary>
    /// Search team members by name, role, or skills within a project
    /// </summary>
    [HttpGet("project/{projectId}/search")]
    public async Task<ActionResult<IEnumerable<TeamMemberDto>>> SearchTeamMembers(int projectId, [FromQuery] string? query)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        // Verify the user has access to this project
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId && p.OwnerId == userId);

        if (project == null)
        {
            return NotFound(new { error = "Project not found or you don't have access" });
        }

        var teamMembersQuery = _context.TeamMembers
            .Where(tm => tm.ProjectId == projectId);

        if (!string.IsNullOrWhiteSpace(query))
        {
            var lowerQuery = query.ToLower();
            teamMembersQuery = teamMembersQuery.Where(tm =>
                tm.FullName.ToLower().Contains(lowerQuery) ||
                tm.Role.ToLower().Contains(lowerQuery) ||
                (tm.Skills != null && tm.Skills.ToLower().Contains(lowerQuery)) ||
                tm.Email.ToLower().Contains(lowerQuery));
        }

        var teamMembers = await teamMembersQuery
            .OrderBy(tm => tm.FullName)
            .Select(tm => new TeamMemberDto
            {
                Id = tm.Id,
                FullName = tm.FullName,
                Email = tm.Email,
                ProfileImageUrl = tm.ProfileImageUrl,
                University = tm.University,
                GraduationYear = tm.GraduationYear,
                Role = tm.Role,
                Skills = tm.Skills,
                Availability = tm.Availability,
                Rating = tm.Rating,
                ProjectContributions = tm.ProjectContributions,
                TotalTasks = tm.TotalTasks,
                TasksCompleted = tm.TasksCompleted,
                CreatedAt = tm.CreatedAt,
                UpdatedAt = tm.UpdatedAt,
                ProjectId = tm.ProjectId,
                ProjectTitle = tm.Project!.Title,
                UserId = tm.UserId
            })
            .ToListAsync();

        return Ok(teamMembers);
    }

    /// <summary>
    /// Get team statistics for a project
    /// </summary>
    [HttpGet("project/{projectId}/stats")]
    public async Task<ActionResult<object>> GetTeamStats(int projectId)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        // Verify the user has access to this project
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId && p.OwnerId == userId);

        if (project == null)
        {
            return NotFound(new { error = "Project not found or you don't have access" });
        }

        var teamMembers = await _context.TeamMembers
            .Where(tm => tm.ProjectId == projectId)
            .ToListAsync();

        var stats = new
        {
            TotalMembers = teamMembers.Count,
            TotalTasks = teamMembers.Sum(tm => tm.TotalTasks),
            TasksCompleted = teamMembers.Sum(tm => tm.TasksCompleted),
            TotalContributions = teamMembers.Sum(tm => tm.ProjectContributions),
            AverageRating = teamMembers.Where(tm => tm.Rating.HasValue).Average(tm => (double?)tm.Rating) ?? 0,
            RoleDistribution = teamMembers.GroupBy(tm => tm.Role)
                .Select(g => new { Role = g.Key, Count = g.Count() })
                .ToList(),
            AvailabilityDistribution = teamMembers.GroupBy(tm => tm.Availability ?? "Not specified")
                .Select(g => new { Availability = g.Key, Count = g.Count() })
                .ToList()
        };

        return Ok(stats);
    }
}

