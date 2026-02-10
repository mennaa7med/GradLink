using GradLink.Application.DTOs.Materials;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Controllers;

/// <summary>
/// Controller for managing learning materials and resources
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MaterialsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<MaterialsController> _logger;

    public MaterialsController(
        AppDbContext context,
        UserManager<ApplicationUser> userManager,
        ILogger<MaterialsController> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    /// <summary>
    /// Get all materials with filtering, sorting, and pagination
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<MaterialsPagedResponse>> GetMaterials([FromQuery] MaterialFilterDto filter)
    {
        var userId = User.Identity?.IsAuthenticated == true 
            ? _userManager.GetUserId(User) 
            : null;

        var query = _context.Materials.AsQueryable();

        // Only show approved materials for non-admin users
        if (!User.IsInRole("Admin"))
        {
            query = query.Where(m => m.Status == "approved");
        }
        else if (!string.IsNullOrEmpty(filter.Status))
        {
            query = query.Where(m => m.Status == filter.Status);
        }

        // Filter by type
        if (!string.IsNullOrEmpty(filter.Type))
        {
            query = query.Where(m => m.Type == filter.Type);
        }

        // Filter by field
        if (!string.IsNullOrEmpty(filter.Field) && filter.Field != "all")
        {
            query = query.Where(m => m.Field == filter.Field);
        }

        // Filter by difficulty
        if (!string.IsNullOrEmpty(filter.DifficultyLevel))
        {
            query = query.Where(m => m.DifficultyLevel == filter.DifficultyLevel);
        }

        // Filter by featured
        if (filter.IsFeatured.HasValue)
        {
            query = query.Where(m => m.IsFeatured == filter.IsFeatured.Value);
        }

        // Search
        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            var searchLower = filter.SearchTerm.ToLower();
            query = query.Where(m =>
                m.Title.ToLower().Contains(searchLower) ||
                m.Description.ToLower().Contains(searchLower) ||
                (m.Author != null && m.Author.ToLower().Contains(searchLower)) ||
                (m.Tags != null && m.Tags.ToLower().Contains(searchLower)));
        }

        // Sorting
        query = filter.SortBy?.ToLower() switch
        {
            "popular" => query.OrderByDescending(m => m.DownloadCount),
            "newest" => query.OrderByDescending(m => m.CreatedAt),
            "rating" => query.OrderByDescending(m => m.AverageRating),
            "name" => query.OrderBy(m => m.Title),
            _ => query.OrderByDescending(m => m.DownloadCount)
        };

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize);

        var materials = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Include(m => m.SubmittedBy)
            .ToListAsync();

        // Get user-specific data if authenticated
        List<int>? bookmarkedIds = null;
        Dictionary<int, int>? userProgress = null;
        Dictionary<int, int>? userRatings = null;

        if (!string.IsNullOrEmpty(userId))
        {
            var materialIds = materials.Select(m => m.Id).ToList();

            bookmarkedIds = await _context.MaterialProgresses
                .Where(p => p.UserId == userId && materialIds.Contains(p.MaterialId) && p.IsBookmarked)
                .Select(p => p.MaterialId)
                .ToListAsync();

            userProgress = await _context.MaterialProgresses
                .Where(p => p.UserId == userId && materialIds.Contains(p.MaterialId))
                .ToDictionaryAsync(p => p.MaterialId, p => p.ProgressPercent);

            userRatings = await _context.MaterialRatings
                .Where(r => r.UserId == userId && materialIds.Contains(r.MaterialId))
                .ToDictionaryAsync(r => r.MaterialId, r => r.Rating);
        }

        var items = materials.Select(m => new MaterialDto
        {
            Id = m.Id,
            Type = m.Type,
            Title = m.Title,
            Description = m.Description,
            Author = m.Author,
            Field = m.Field,
            Link = m.Link,
            FilePath = m.FilePath,
            FileType = m.FileType,
            ThumbnailUrl = m.ThumbnailUrl,
            DurationMinutes = m.DurationMinutes,
            DownloadCount = m.DownloadCount,
            AverageRating = m.AverageRating,
            RatingCount = m.RatingCount,
            Tags = m.Tags,
            Status = m.Status,
            SubmittedByName = m.SubmittedBy?.FullName,
            CreatedAt = m.CreatedAt,
            IsFeatured = m.IsFeatured,
            DifficultyLevel = m.DifficultyLevel,
            EstimatedTimeMinutes = m.EstimatedTimeMinutes,
            IsBookmarked = bookmarkedIds?.Contains(m.Id) ?? false,
            UserProgress = userProgress?.GetValueOrDefault(m.Id),
            UserRating = userRatings?.GetValueOrDefault(m.Id)
        }).ToList();

        return Ok(new MaterialsPagedResponse
        {
            Items = items,
            TotalCount = totalCount,
            Page = filter.Page,
            PageSize = filter.PageSize,
            TotalPages = totalPages
        });
    }

    /// <summary>
    /// Get a single material by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<MaterialDto>> GetMaterial(int id)
    {
        var userId = User.Identity?.IsAuthenticated == true
            ? _userManager.GetUserId(User)
            : null;

        var material = await _context.Materials
            .Include(m => m.SubmittedBy)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (material == null)
            return NotFound(new { message = "Material not found" });

        // Only admin can see non-approved materials
        if (material.Status != "approved" && !User.IsInRole("Admin"))
            return NotFound(new { message = "Material not found" });

        // Increment download count
        material.DownloadCount++;
        await _context.SaveChangesAsync();

        // Track recently viewed
        if (!string.IsNullOrEmpty(userId))
        {
            var progress = await _context.MaterialProgresses
                .FirstOrDefaultAsync(p => p.MaterialId == id && p.UserId == userId);

            if (progress == null)
            {
                progress = new MaterialProgress
                {
                    MaterialId = id,
                    UserId = userId,
                    Status = "not_started"
                };
                _context.MaterialProgresses.Add(progress);
            }

            progress.LastAccessedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        var dto = new MaterialDto
        {
            Id = material.Id,
            Type = material.Type,
            Title = material.Title,
            Description = material.Description,
            Author = material.Author,
            Field = material.Field,
            Link = material.Link,
            FilePath = material.FilePath,
            FileType = material.FileType,
            ThumbnailUrl = material.ThumbnailUrl,
            DurationMinutes = material.DurationMinutes,
            DownloadCount = material.DownloadCount,
            AverageRating = material.AverageRating,
            RatingCount = material.RatingCount,
            Tags = material.Tags,
            Status = material.Status,
            SubmittedByName = material.SubmittedBy?.FullName,
            CreatedAt = material.CreatedAt,
            IsFeatured = material.IsFeatured,
            DifficultyLevel = material.DifficultyLevel,
            EstimatedTimeMinutes = material.EstimatedTimeMinutes
        };

        if (!string.IsNullOrEmpty(userId))
        {
            var progress = await _context.MaterialProgresses
                .FirstOrDefaultAsync(p => p.MaterialId == id && p.UserId == userId);

            dto.IsBookmarked = progress?.IsBookmarked ?? false;
            dto.UserProgress = progress?.ProgressPercent;

            var rating = await _context.MaterialRatings
                .FirstOrDefaultAsync(r => r.MaterialId == id && r.UserId == userId);
            dto.UserRating = rating?.Rating;
        }

        return Ok(dto);
    }

    /// <summary>
    /// Submit a new material for approval
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<MaterialDto>> CreateMaterial([FromBody] CreateMaterialRequest request)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var material = new Material
        {
            Type = request.Type,
            Title = request.Title,
            Description = request.Description,
            Author = request.Author,
            Field = request.Field,
            Link = request.Link,
            FileType = request.FileType,
            Tags = request.Tags,
            DifficultyLevel = request.DifficultyLevel,
            EstimatedTimeMinutes = request.EstimatedTimeMinutes,
            DurationMinutes = request.DurationMinutes,
            SubmittedById = userId,
            Status = User.IsInRole("Admin") ? "approved" : "pending",
            CreatedAt = DateTime.UtcNow
        };

        _context.Materials.Add(material);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Material {Title} submitted by user {UserId}", material.Title, userId);

        return CreatedAtAction(nameof(GetMaterial), new { id = material.Id }, new MaterialDto
        {
            Id = material.Id,
            Type = material.Type,
            Title = material.Title,
            Description = material.Description,
            Status = material.Status,
            CreatedAt = material.CreatedAt
        });
    }

    /// <summary>
    /// Update a material (owner or admin only)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateMaterial(int id, [FromBody] UpdateMaterialRequest request)
    {
        var userId = _userManager.GetUserId(User);
        var material = await _context.Materials.FindAsync(id);

        if (material == null)
            return NotFound(new { message = "Material not found" });

        // Only owner or admin can update
        if (material.SubmittedById != userId && !User.IsInRole("Admin"))
            return Forbid();

        if (!string.IsNullOrEmpty(request.Title))
            material.Title = request.Title;

        if (!string.IsNullOrEmpty(request.Description))
            material.Description = request.Description;

        if (request.Author != null)
            material.Author = request.Author;

        if (!string.IsNullOrEmpty(request.Field))
            material.Field = request.Field;

        if (request.Link != null)
            material.Link = request.Link;

        if (request.Tags != null)
            material.Tags = request.Tags;

        if (request.DifficultyLevel != null)
            material.DifficultyLevel = request.DifficultyLevel;

        if (request.EstimatedTimeMinutes.HasValue)
            material.EstimatedTimeMinutes = request.EstimatedTimeMinutes;

        // Only admin can set featured
        if (User.IsInRole("Admin") && request.IsFeatured.HasValue)
            material.IsFeatured = request.IsFeatured.Value;

        material.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Material updated successfully" });
    }

    /// <summary>
    /// Delete a material (owner or admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteMaterial(int id)
    {
        var userId = _userManager.GetUserId(User);
        var material = await _context.Materials.FindAsync(id);

        if (material == null)
            return NotFound(new { message = "Material not found" });

        // Only owner or admin can delete
        if (material.SubmittedById != userId && !User.IsInRole("Admin"))
            return Forbid();

        _context.Materials.Remove(material);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Material deleted successfully" });
    }

    /// <summary>
    /// Approve or reject a material (Admin only)
    /// </summary>
    [HttpPost("{id}/review")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ReviewMaterial(int id, [FromBody] ReviewMaterialRequest request)
    {
        var userId = _userManager.GetUserId(User);
        var material = await _context.Materials.FindAsync(id);

        if (material == null)
            return NotFound(new { message = "Material not found" });

        material.Status = request.Status;
        material.ReviewNotes = request.ReviewNotes;
        material.ReviewedById = userId;
        material.ReviewedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Material {Id} {Status} by admin {UserId}", id, request.Status, userId);

        return Ok(new { message = $"Material {request.Status} successfully" });
    }

    /// <summary>
    /// Get pending materials for approval (Admin only)
    /// </summary>
    [HttpGet("pending")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<MaterialDto>>> GetPendingMaterials()
    {
        var materials = await _context.Materials
            .Where(m => m.Status == "pending")
            .Include(m => m.SubmittedBy)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();

        return Ok(materials.Select(m => new MaterialDto
        {
            Id = m.Id,
            Type = m.Type,
            Title = m.Title,
            Description = m.Description,
            Author = m.Author,
            Field = m.Field,
            Link = m.Link,
            FileType = m.FileType,
            Status = m.Status,
            SubmittedByName = m.SubmittedBy?.FullName,
            CreatedAt = m.CreatedAt
        }));
    }

    /// <summary>
    /// Get materials statistics
    /// </summary>
    [HttpGet("stats")]
    public async Task<ActionResult<MaterialStatsDto>> GetStats()
    {
        var query = _context.Materials.Where(m => m.Status == "approved");

        var stats = new MaterialStatsDto
        {
            TotalMaterials = await query.CountAsync(),
            TotalTemplates = await query.CountAsync(m => m.Type == "template"),
            TotalTools = await query.CountAsync(m => m.Type == "tool"),
            TotalBooks = await query.CountAsync(m => m.Type == "book"),
            TotalDatasets = await query.CountAsync(m => m.Type == "dataset"),
            TotalVideos = await query.CountAsync(m => m.Type == "video"),
            TotalDownloads = await query.SumAsync(m => m.DownloadCount),
            PendingApprovals = User.IsInRole("Admin")
                ? await _context.Materials.CountAsync(m => m.Status == "pending")
                : 0
        };

        var byField = await query
            .GroupBy(m => m.Field)
            .Select(g => new { Field = g.Key, Count = g.Count() })
            .ToListAsync();

        stats.ByField = byField.ToDictionary(x => x.Field, x => x.Count);

        return Ok(stats);
    }

    /// <summary>
    /// Get user's material statistics
    /// </summary>
    [HttpGet("my-stats")]
    [Authorize]
    public async Task<ActionResult<UserMaterialStatsDto>> GetMyStats()
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var progresses = await _context.MaterialProgresses
            .Where(p => p.UserId == userId)
            .Include(p => p.Material)
            .ToListAsync();

        var stats = new UserMaterialStatsDto
        {
            BookmarkedCount = progresses.Count(p => p.IsBookmarked),
            CompletedCount = progresses.Count(p => p.Status == "completed"),
            InProgressCount = progresses.Count(p => p.Status == "in_progress"),
            TotalTimeSpentMinutes = progresses.Sum(p => p.TimeSpentMinutes),
            ReviewsGiven = await _context.MaterialRatings.CountAsync(r => r.UserId == userId),
            CollectionsCreated = await _context.MaterialCollections.CountAsync(c => c.OwnerId == userId),
            RecentlyViewed = progresses
                .OrderByDescending(p => p.LastAccessedAt)
                .Take(10)
                .Select(p => new MaterialProgressDto
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
                })
                .ToList()
        };

        return Ok(stats);
    }

    /// <summary>
    /// Get recommended materials for the user
    /// </summary>
    [HttpGet("recommendations")]
    [Authorize]
    public async Task<ActionResult<List<RecommendationDto>>> GetRecommendations()
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Unauthorized();

        // Get user's field of study
        var userField = user.Major ?? user.Department ?? "computer science";

        // Get materials user hasn't interacted with
        var interactedIds = await _context.MaterialProgresses
            .Where(p => p.UserId == userId)
            .Select(p => p.MaterialId)
            .ToListAsync();

        var recommendations = new List<RecommendationDto>();

        // 1. Recommend based on user's field
        var fieldMaterials = await _context.Materials
            .Where(m => m.Status == "approved" && 
                       m.Field.ToLower().Contains(userField.ToLower()) &&
                       !interactedIds.Contains(m.Id))
            .OrderByDescending(m => m.AverageRating)
            .ThenByDescending(m => m.DownloadCount)
            .Take(5)
            .ToListAsync();

        foreach (var m in fieldMaterials)
        {
            recommendations.Add(new RecommendationDto
            {
                Material = MapToDto(m),
                Reason = $"Related to your field: {userField}",
                Score = m.AverageRating * 0.5 + (m.DownloadCount / 100.0) * 0.3 + (m.IsFeatured ? 0.2 : 0)
            });
        }

        // 2. Recommend popular materials
        var popularMaterials = await _context.Materials
            .Where(m => m.Status == "approved" && !interactedIds.Contains(m.Id))
            .OrderByDescending(m => m.DownloadCount)
            .Take(5)
            .ToListAsync();

        foreach (var m in popularMaterials)
        {
            if (!recommendations.Any(r => r.Material.Id == m.Id))
            {
                recommendations.Add(new RecommendationDto
                {
                    Material = MapToDto(m),
                    Reason = "Popular among students",
                    Score = m.DownloadCount / 100.0
                });
            }
        }

        // 3. Recommend featured materials
        var featuredMaterials = await _context.Materials
            .Where(m => m.Status == "approved" && m.IsFeatured && !interactedIds.Contains(m.Id))
            .Take(3)
            .ToListAsync();

        foreach (var m in featuredMaterials)
        {
            if (!recommendations.Any(r => r.Material.Id == m.Id))
            {
                recommendations.Add(new RecommendationDto
                {
                    Material = MapToDto(m),
                    Reason = "Featured by GradLink",
                    Score = 1.0
                });
            }
        }

        return Ok(recommendations.OrderByDescending(r => r.Score).Take(10).ToList());
    }

    /// <summary>
    /// Get top rated materials
    /// </summary>
    [HttpGet("top-rated")]
    public async Task<ActionResult<List<MaterialDto>>> GetTopRated([FromQuery] int count = 10)
    {
        var materials = await _context.Materials
            .Where(m => m.Status == "approved" && m.RatingCount > 0)
            .OrderByDescending(m => m.AverageRating)
            .ThenByDescending(m => m.RatingCount)
            .Take(count)
            .ToListAsync();

        return Ok(materials.Select(MapToDto));
    }

    /// <summary>
    /// Get featured materials
    /// </summary>
    [HttpGet("featured")]
    public async Task<ActionResult<List<MaterialDto>>> GetFeatured()
    {
        var materials = await _context.Materials
            .Where(m => m.Status == "approved" && m.IsFeatured)
            .OrderByDescending(m => m.CreatedAt)
            .Take(10)
            .ToListAsync();

        return Ok(materials.Select(MapToDto));
    }

    private static MaterialDto MapToDto(Material m)
    {
        return new MaterialDto
        {
            Id = m.Id,
            Type = m.Type,
            Title = m.Title,
            Description = m.Description,
            Author = m.Author,
            Field = m.Field,
            Link = m.Link,
            FilePath = m.FilePath,
            FileType = m.FileType,
            ThumbnailUrl = m.ThumbnailUrl,
            DurationMinutes = m.DurationMinutes,
            DownloadCount = m.DownloadCount,
            AverageRating = m.AverageRating,
            RatingCount = m.RatingCount,
            Tags = m.Tags,
            Status = m.Status,
            CreatedAt = m.CreatedAt,
            IsFeatured = m.IsFeatured,
            DifficultyLevel = m.DifficultyLevel,
            EstimatedTimeMinutes = m.EstimatedTimeMinutes
        };
    }
}
