using GradLink.Application.DTOs.Tasks;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    private string? GetUserId() => User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

    private static TaskItemDto MapToDto(TaskItem task)
    {
        return new TaskItemDto
        {
            Id = task.Id,
            Name = task.Name,
            Description = task.Description,
            Notes = task.Notes,
            Tags = string.IsNullOrEmpty(task.Tags)
                ? new List<string>()
                : task.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()).ToList(),
            DueDate = task.DueDate,
            Priority = task.Priority,
            Person = task.Person,
            Status = task.Status,
            Category = task.Category,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            CompletedAt = task.CompletedAt,
            ProjectId = task.ProjectId,
            Subtasks = task.Subtasks.Select(s => new SubtaskDto
            {
                Id = s.Id,
                Name = s.Name,
                Completed = s.Completed
            }).ToList()
        };
    }

    // GET: api/tasks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetTasks(
        [FromQuery] string? status = null,
        [FromQuery] string? category = null,
        [FromQuery] string? priority = null,
        [FromQuery] string? person = null,
        [FromQuery] string? search = null)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var query = _context.Tasks
            .Include(t => t.Subtasks)
            .Where(t => t.UserId == userId);

        // Apply filters
        if (!string.IsNullOrEmpty(status))
            query = query.Where(t => t.Status == status);

        if (!string.IsNullOrEmpty(category))
            query = query.Where(t => t.Category == category);

        if (!string.IsNullOrEmpty(priority))
            query = query.Where(t => t.Priority == priority);

        if (!string.IsNullOrEmpty(person))
            query = query.Where(t => t.Person == person);

        if (!string.IsNullOrEmpty(search))
            query = query.Where(t => 
                t.Name.Contains(search) || 
                (t.Description != null && t.Description.Contains(search)) ||
                (t.Notes != null && t.Notes.Contains(search)));

        var tasks = await query
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        return Ok(tasks.Select(MapToDto));
    }

    // GET: api/tasks/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItemDto>> GetTask(int id)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var task = await _context.Tasks
            .Include(t => t.Subtasks)
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (task == null) return NotFound();

        return Ok(MapToDto(task));
    }

    // POST: api/tasks
    [HttpPost]
    public async Task<ActionResult<TaskItemDto>> CreateTask([FromBody] CreateTaskRequest request)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var task = new TaskItem
        {
            Name = request.Name,
            Description = request.Description,
            Notes = request.Notes,
            Tags = request.Tags,
            DueDate = request.DueDate,
            Priority = request.Priority,
            Person = request.Person,
            Status = request.Status,
            Category = request.Category,
            ProjectId = request.ProjectId,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, MapToDto(task));
    }

    // PUT: api/tasks/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskRequest request)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (task == null) return NotFound();

        // Update fields if provided
        if (request.Name != null) task.Name = request.Name;
        if (request.Description != null) task.Description = request.Description;
        if (request.Notes != null) task.Notes = request.Notes;
        if (request.Tags != null) task.Tags = request.Tags;
        if (request.DueDate.HasValue) task.DueDate = request.DueDate;
        if (request.Priority != null) task.Priority = request.Priority;
        if (request.Person != null) task.Person = request.Person;
        if (request.Category != null) task.Category = request.Category;

        // Handle status change
        if (request.Status != null)
        {
            var oldStatus = task.Status;
            task.Status = request.Status;
            
            // Set CompletedAt when task is completed
            if (request.Status == "Completed" && oldStatus != "Completed")
            {
                task.CompletedAt = DateTime.UtcNow;
            }
            else if (request.Status != "Completed")
            {
                task.CompletedAt = null;
            }
        }

        task.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/tasks/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var task = await _context.Tasks
            .Include(t => t.Subtasks)
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        
        if (task == null) return NotFound();

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // ==================== Subtasks ====================

    // POST: api/tasks/{taskId}/subtasks
    [HttpPost("{taskId}/subtasks")]
    public async Task<ActionResult<SubtaskDto>> CreateSubtask(int taskId, [FromBody] CreateSubtaskRequest request)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
        if (task == null) return NotFound();

        var subtask = new Subtask
        {
            Name = request.Name,
            TaskItemId = taskId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Subtasks.Add(subtask);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTask), new { id = taskId }, new SubtaskDto
        {
            Id = subtask.Id,
            Name = subtask.Name,
            Completed = subtask.Completed
        });
    }

    // PUT: api/tasks/{taskId}/subtasks/{subtaskId}
    [HttpPut("{taskId}/subtasks/{subtaskId}")]
    public async Task<IActionResult> UpdateSubtask(int taskId, int subtaskId, [FromBody] UpdateSubtaskRequest request)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
        if (task == null) return NotFound();

        var subtask = await _context.Subtasks.FirstOrDefaultAsync(s => s.Id == subtaskId && s.TaskItemId == taskId);
        if (subtask == null) return NotFound();

        if (request.Name != null) subtask.Name = request.Name;
        if (request.Completed.HasValue) subtask.Completed = request.Completed.Value;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/tasks/{taskId}/subtasks/{subtaskId}
    [HttpDelete("{taskId}/subtasks/{subtaskId}")]
    public async Task<IActionResult> DeleteSubtask(int taskId, int subtaskId)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
        if (task == null) return NotFound();

        var subtask = await _context.Subtasks.FirstOrDefaultAsync(s => s.Id == subtaskId && s.TaskItemId == taskId);
        if (subtask == null) return NotFound();

        _context.Subtasks.Remove(subtask);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // ==================== Statistics ====================

    // GET: api/tasks/stats
    [HttpGet("stats")]
    public async Task<ActionResult> GetTaskStats()
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var tasks = await _context.Tasks
            .Include(t => t.Subtasks)
            .Where(t => t.UserId == userId)
            .ToListAsync();

        var totalSubtasks = tasks.Sum(t => t.Subtasks.Count);
        var completedSubtasks = tasks.Sum(t => t.Subtasks.Count(s => s.Completed));

        var stats = new
        {
            Total = tasks.Count,
            Pending = tasks.Count(t => t.Status == "Pending"),
            InProgress = tasks.Count(t => t.Status == "In Progress"),
            Completed = tasks.Count(t => t.Status == "Completed"),
            Overdue = tasks.Count(t => t.DueDate < DateTime.UtcNow && t.Status != "Completed"),
            TotalSubtasks = totalSubtasks,
            CompletedSubtasks = completedSubtasks,
            SubtaskProgress = totalSubtasks > 0 ? Math.Round((double)completedSubtasks / totalSubtasks * 100, 1) : 0,
            ByCategory = tasks.GroupBy(t => t.Category)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToList(),
            ByPriority = tasks.GroupBy(t => t.Priority)
                .Select(g => new { Priority = g.Key, Count = g.Count() })
                .ToList(),
            ByPerson = tasks.Where(t => !string.IsNullOrEmpty(t.Person))
                .GroupBy(t => t.Person)
                .Select(g => new { Person = g.Key, Count = g.Count() })
                .ToList()
        };

        return Ok(stats);
    }

    // GET: api/tasks/people
    [HttpGet("people")]
    public async Task<ActionResult<IEnumerable<string>>> GetPeople()
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var people = await _context.Tasks
            .Where(t => t.UserId == userId && !string.IsNullOrEmpty(t.Person))
            .Select(t => t.Person!)
            .Distinct()
            .ToListAsync();

        return Ok(people);
    }
}

