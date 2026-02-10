using GradLink.Application.DTOs.Projects;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/projects/{projectId}/tasks")]
public class ProjectTasksController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProjectTasksController(AppDbContext context)
    {
        _context = context;
    }

    private string? GetUserId() => User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks(int projectId)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        // Verify project ownership
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId && p.OwnerId == userId);

        if (project == null)
        {
            return NotFound();
        }

        var tasks = await _context.Tasks
            .Where(t => t.ProjectId == projectId)
            .Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Name, // Map Name to Title
                Description = t.Description,
                Status = t.Status,
                Priority = t.Priority,
                DueDate = t.DueDate,
                CreatedAt = t.CreatedAt,
                CompletedAt = t.CompletedAt,
                ProjectId = t.ProjectId
            })
            .ToListAsync();

        return Ok(tasks);
    }

    [HttpPost]
    public async Task<ActionResult<TaskDto>> CreateTask(int projectId, [FromBody] CreateProjectTaskRequest request)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        // Verify project ownership
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId && p.OwnerId == userId);

        if (project == null)
        {
            return NotFound();
        }

        var task = new TaskItem
        {
            Name = request.Title, // Map Title to Name
            Description = request.Description,
            Priority = request.Priority ?? "Medium",
            DueDate = request.DueDate,
            ProjectId = projectId,
            UserId = userId, // Required field
            CreatedAt = DateTime.UtcNow
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        var taskDto = new TaskDto
        {
            Id = task.Id,
            Title = task.Name,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            CompletedAt = task.CompletedAt,
            ProjectId = task.ProjectId
        };

        return CreatedAtAction(nameof(GetTasks), new { projectId }, taskDto);
    }

    [HttpPut("{taskId}")]
    public async Task<IActionResult> UpdateTask(int projectId, int taskId, [FromBody] TaskDto request)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        // Verify project ownership
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId && p.OwnerId == userId);

        if (project == null)
        {
            return NotFound();
        }

        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId == projectId);

        if (task == null)
        {
            return NotFound();
        }

        task.Name = request.Title; // Map Title to Name
        task.Description = request.Description;
        task.Status = request.Status;
        task.Priority = request.Priority;
        task.DueDate = request.DueDate;
        task.UpdatedAt = DateTime.UtcNow;

        if (request.Status == "Completed" && task.CompletedAt == null)
        {
            task.CompletedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{taskId}")]
    public async Task<IActionResult> DeleteTask(int projectId, int taskId)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        // Verify project ownership
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId && p.OwnerId == userId);

        if (project == null)
        {
            return NotFound();
        }

        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId == projectId);

        if (task == null)
        {
            return NotFound();
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
