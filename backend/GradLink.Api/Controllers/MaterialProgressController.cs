using GradLink.Application.DTOs.Materials;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Controllers;

/// <summary>
/// Controller for managing material progress and bookmarks
/// </summary>
[ApiController]
[Route("api/materials")]
[Produces("application/json")]
[Authorize]
public class MaterialProgressController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public MaterialProgressController(
        AppDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    /// Get user's progress on a specific material
    /// </summary>
    [HttpGet("{materialId}/progress")]
    public async Task<ActionResult<MaterialProgressDto>> GetProgress(int materialId)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var progress = await _context.MaterialProgresses
            .Include(p => p.Material)
            .FirstOrDefaultAsync(p => p.MaterialId == materialId && p.UserId == userId);

        if (progress == null)
            return Ok(new MaterialProgressDto
            {
                MaterialId = materialId,
                Status = "not_started",
                ProgressPercent = 0,
                IsBookmarked = false
            });

        return Ok(new MaterialProgressDto
        {
            Id = progress.Id,
            MaterialId = progress.MaterialId,
            MaterialTitle = progress.Material.Title,
            MaterialType = progress.Material.Type,
            Status = progress.Status,
            ProgressPercent = progress.ProgressPercent,
            TimeSpentMinutes = progress.TimeSpentMinutes,
            LastPosition = progress.LastPosition,
            IsBookmarked = progress.IsBookmarked,
            StartedAt = progress.StartedAt,
            CompletedAt = progress.CompletedAt,
            LastAccessedAt = progress.LastAccessedAt
        });
    }

    /// <summary>
    /// Update progress on a material
    /// </summary>
    [HttpPut("{materialId}/progress")]
    public async Task<ActionResult<MaterialProgressDto>> UpdateProgress(int materialId, [FromBody] UpdateProgressRequest request)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var material = await _context.Materials.FindAsync(materialId);
        if (material == null)
            return NotFound(new { message = "Material not found" });

        var progress = await _context.MaterialProgresses
            .FirstOrDefaultAsync(p => p.MaterialId == materialId && p.UserId == userId);

        if (progress == null)
        {
            progress = new MaterialProgress
            {
                MaterialId = materialId,
                UserId = userId,
                Status = "not_started",
                CreatedAt = DateTime.UtcNow
            };
            _context.MaterialProgresses.Add(progress);
        }

        if (!string.IsNullOrEmpty(request.Status))
        {
            progress.Status = request.Status;

            if (request.Status == "in_progress" && progress.StartedAt == null)
            {
                progress.StartedAt = DateTime.UtcNow;
            }
            else if (request.Status == "completed" && progress.CompletedAt == null)
            {
                progress.CompletedAt = DateTime.UtcNow;
                progress.ProgressPercent = 100;
            }
        }

        if (request.ProgressPercent.HasValue)
        {
            progress.ProgressPercent = request.ProgressPercent.Value;

            if (progress.ProgressPercent > 0 && progress.Status == "not_started")
            {
                progress.Status = "in_progress";
                progress.StartedAt = DateTime.UtcNow;
            }

            if (progress.ProgressPercent >= 100)
            {
                progress.Status = "completed";
                progress.CompletedAt ??= DateTime.UtcNow;
            }
        }

        if (request.TimeSpentMinutes.HasValue)
        {
            progress.TimeSpentMinutes = request.TimeSpentMinutes.Value;
        }

        if (request.LastPosition != null)
        {
            progress.LastPosition = request.LastPosition;
        }

        if (request.IsBookmarked.HasValue)
        {
            progress.IsBookmarked = request.IsBookmarked.Value;
        }

        progress.LastAccessedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new MaterialProgressDto
        {
            Id = progress.Id,
            MaterialId = progress.MaterialId,
            MaterialTitle = material.Title,
            MaterialType = material.Type,
            Status = progress.Status,
            ProgressPercent = progress.ProgressPercent,
            TimeSpentMinutes = progress.TimeSpentMinutes,
            LastPosition = progress.LastPosition,
            IsBookmarked = progress.IsBookmarked,
            StartedAt = progress.StartedAt,
            CompletedAt = progress.CompletedAt,
            LastAccessedAt = progress.LastAccessedAt
        });
    }

    /// <summary>
    /// Toggle bookmark on a material
    /// </summary>
    [HttpPost("{materialId}/bookmark")]
    public async Task<IActionResult> ToggleBookmark(int materialId)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var material = await _context.Materials.FindAsync(materialId);
        if (material == null)
            return NotFound(new { message = "Material not found" });

        var progress = await _context.MaterialProgresses
            .FirstOrDefaultAsync(p => p.MaterialId == materialId && p.UserId == userId);

        if (progress == null)
        {
            progress = new MaterialProgress
            {
                MaterialId = materialId,
                UserId = userId,
                Status = "not_started",
                IsBookmarked = true,
                CreatedAt = DateTime.UtcNow,
                LastAccessedAt = DateTime.UtcNow
            };
            _context.MaterialProgresses.Add(progress);
        }
        else
        {
            progress.IsBookmarked = !progress.IsBookmarked;
            progress.LastAccessedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        return Ok(new { 
            isBookmarked = progress.IsBookmarked,
            message = progress.IsBookmarked ? "Added to bookmarks" : "Removed from bookmarks"
        });
    }

    /// <summary>
    /// Get all bookmarked materials
    /// </summary>
    [HttpGet("bookmarked")]
    public async Task<ActionResult<List<MaterialDto>>> GetBookmarked()
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var bookmarked = await _context.MaterialProgresses
            .Where(p => p.UserId == userId && p.IsBookmarked)
            .Include(p => p.Material)
            .OrderByDescending(p => p.LastAccessedAt)
            .ToListAsync();

        return Ok(bookmarked.Select(p => new MaterialDto
        {
            Id = p.Material.Id,
            Type = p.Material.Type,
            Title = p.Material.Title,
            Description = p.Material.Description,
            Author = p.Material.Author,
            Field = p.Material.Field,
            Link = p.Material.Link,
            FileType = p.Material.FileType,
            ThumbnailUrl = p.Material.ThumbnailUrl,
            DownloadCount = p.Material.DownloadCount,
            AverageRating = p.Material.AverageRating,
            RatingCount = p.Material.RatingCount,
            IsFeatured = p.Material.IsFeatured,
            IsBookmarked = true,
            UserProgress = p.ProgressPercent
        }));
    }

    /// <summary>
    /// Get materials in progress
    /// </summary>
    [HttpGet("in-progress")]
    public async Task<ActionResult<List<MaterialProgressDto>>> GetInProgress()
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var inProgress = await _context.MaterialProgresses
            .Where(p => p.UserId == userId && p.Status == "in_progress")
            .Include(p => p.Material)
            .OrderByDescending(p => p.LastAccessedAt)
            .ToListAsync();

        return Ok(inProgress.Select(p => new MaterialProgressDto
        {
            Id = p.Id,
            MaterialId = p.MaterialId,
            MaterialTitle = p.Material.Title,
            MaterialType = p.Material.Type,
            Status = p.Status,
            ProgressPercent = p.ProgressPercent,
            TimeSpentMinutes = p.TimeSpentMinutes,
            LastPosition = p.LastPosition,
            IsBookmarked = p.IsBookmarked,
            StartedAt = p.StartedAt,
            LastAccessedAt = p.LastAccessedAt
        }));
    }

    /// <summary>
    /// Get completed materials
    /// </summary>
    [HttpGet("completed")]
    public async Task<ActionResult<List<MaterialProgressDto>>> GetCompleted()
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var completed = await _context.MaterialProgresses
            .Where(p => p.UserId == userId && p.Status == "completed")
            .Include(p => p.Material)
            .OrderByDescending(p => p.CompletedAt)
            .ToListAsync();

        return Ok(completed.Select(p => new MaterialProgressDto
        {
            Id = p.Id,
            MaterialId = p.MaterialId,
            MaterialTitle = p.Material.Title,
            MaterialType = p.Material.Type,
            Status = p.Status,
            ProgressPercent = p.ProgressPercent,
            TimeSpentMinutes = p.TimeSpentMinutes,
            IsBookmarked = p.IsBookmarked,
            StartedAt = p.StartedAt,
            CompletedAt = p.CompletedAt,
            LastAccessedAt = p.LastAccessedAt
        }));
    }

    /// <summary>
    /// Get recently viewed materials
    /// </summary>
    [HttpGet("recently-viewed")]
    public async Task<ActionResult<List<MaterialProgressDto>>> GetRecentlyViewed([FromQuery] int count = 10)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var recent = await _context.MaterialProgresses
            .Where(p => p.UserId == userId)
            .Include(p => p.Material)
            .OrderByDescending(p => p.LastAccessedAt)
            .Take(count)
            .ToListAsync();

        return Ok(recent.Select(p => new MaterialProgressDto
        {
            Id = p.Id,
            MaterialId = p.MaterialId,
            MaterialTitle = p.Material.Title,
            MaterialType = p.Material.Type,
            Status = p.Status,
            ProgressPercent = p.ProgressPercent,
            TimeSpentMinutes = p.TimeSpentMinutes,
            LastPosition = p.LastPosition,
            IsBookmarked = p.IsBookmarked,
            StartedAt = p.StartedAt,
            CompletedAt = p.CompletedAt,
            LastAccessedAt = p.LastAccessedAt
        }));
    }
}















