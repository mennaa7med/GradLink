using GradLink.Application.DTOs.Projects;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProjectsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var projects = await _context.Projects
            .Where(p => p.OwnerId == userId)
            .Include(p => p.Owner)
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Category = p.Category,
                Status = p.Status,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                OwnerId = p.OwnerId,
                OwnerName = p.Owner!.FullName
            })
            .ToListAsync();

        return Ok(projects);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDto>> GetProject(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var project = await _context.Projects
            .Where(p => p.Id == id && p.OwnerId == userId)
            .Include(p => p.Owner)
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Category = p.Category,
                Status = p.Status,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                OwnerId = p.OwnerId,
                OwnerName = p.Owner!.FullName
            })
            .FirstOrDefaultAsync();

        if (project == null)
        {
            return NotFound();
        }

        return Ok(project);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> CreateProject([FromBody] CreateProjectRequest request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;

        var project = new Project
        {
            Title = request.Title,
            Description = request.Description,
            Category = request.Category,
            OwnerId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        var projectDto = new ProjectDto
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            Category = project.Category,
            Status = project.Status,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt,
            OwnerId = project.OwnerId
        };

        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, projectDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(int id, [FromBody] UpdateProjectRequest request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == id && p.OwnerId == userId);

        if (project == null)
        {
            return NotFound();
        }

        if (request.Title != null) project.Title = request.Title;
        if (request.Description != null) project.Description = request.Description;
        if (request.Category != null) project.Category = request.Category;
        if (request.Status != null) project.Status = request.Status;
        project.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == id && p.OwnerId == userId);

        if (project == null)
        {
            return NotFound();
        }

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

