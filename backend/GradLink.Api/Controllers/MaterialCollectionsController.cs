using GradLink.Application.DTOs.Materials;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Controllers;

/// <summary>
/// Controller for managing material collections (playlists and learning paths)
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MaterialCollectionsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public MaterialCollectionsController(
        AppDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    /// Get all public collections and user's own collections
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<MaterialCollectionDto>>> GetCollections([FromQuery] string? type = null, [FromQuery] string? field = null)
    {
        var userId = User.Identity?.IsAuthenticated == true
            ? _userManager.GetUserId(User)
            : null;

        var query = _context.MaterialCollections
            .Include(c => c.Owner)
            .Include(c => c.Materials)
            .Include(c => c.Followers)
            .AsQueryable();

        // Show public collections and user's own collections
        if (!string.IsNullOrEmpty(userId))
        {
            query = query.Where(c => c.IsPublic || c.OwnerId == userId);
        }
        else
        {
            query = query.Where(c => c.IsPublic);
        }

        if (!string.IsNullOrEmpty(type))
        {
            query = query.Where(c => c.Type == type);
        }

        if (!string.IsNullOrEmpty(field))
        {
            query = query.Where(c => c.Field == field);
        }

        var collections = await query
            .OrderByDescending(c => c.IsSystemPath)
            .ThenByDescending(c => c.Followers.Count)
            .ToListAsync();

        return Ok(collections.Select(c => new MaterialCollectionDto
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            CoverImageUrl = c.CoverImageUrl,
            Type = c.Type,
            Field = c.Field,
            DifficultyLevel = c.DifficultyLevel,
            EstimatedTimeMinutes = c.EstimatedTimeMinutes,
            IsSystemPath = c.IsSystemPath,
            IsPublic = c.IsPublic,
            OwnerName = c.Owner.FullName ?? c.Owner.UserName ?? "Unknown",
            MaterialCount = c.Materials.Count,
            FollowerCount = c.Followers.Count,
            CreatedAt = c.CreatedAt,
            IsFollowing = !string.IsNullOrEmpty(userId) && c.Followers.Any(f => f.UserId == userId),
            UserProgress = !string.IsNullOrEmpty(userId) 
                ? c.Followers.FirstOrDefault(f => f.UserId == userId)?.ProgressPercent ?? 0 
                : 0,
            IsOwner = c.OwnerId == userId
        }));
    }

    /// <summary>
    /// Get a single collection with its materials
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<CollectionDetailDto>> GetCollection(int id)
    {
        var userId = User.Identity?.IsAuthenticated == true
            ? _userManager.GetUserId(User)
            : null;

        var collection = await _context.MaterialCollections
            .Include(c => c.Owner)
            .Include(c => c.Materials)
                .ThenInclude(cm => cm.Material)
            .Include(c => c.Followers)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (collection == null)
            return NotFound(new { message = "Collection not found" });

        // Check access
        if (!collection.IsPublic && collection.OwnerId != userId && !User.IsInRole("Admin"))
            return Forbid();

        // Get user's progress on materials in this collection
        Dictionary<int, int>? materialProgress = null;
        if (!string.IsNullOrEmpty(userId))
        {
            var materialIds = collection.Materials.Select(m => m.MaterialId).ToList();
            materialProgress = await _context.MaterialProgresses
                .Where(p => p.UserId == userId && materialIds.Contains(p.MaterialId))
                .ToDictionaryAsync(p => p.MaterialId, p => p.ProgressPercent);
        }

        var dto = new CollectionDetailDto
        {
            Id = collection.Id,
            Title = collection.Title,
            Description = collection.Description,
            CoverImageUrl = collection.CoverImageUrl,
            Type = collection.Type,
            Field = collection.Field,
            DifficultyLevel = collection.DifficultyLevel,
            EstimatedTimeMinutes = collection.EstimatedTimeMinutes,
            IsSystemPath = collection.IsSystemPath,
            IsPublic = collection.IsPublic,
            OwnerName = collection.Owner.FullName ?? collection.Owner.UserName ?? "Unknown",
            MaterialCount = collection.Materials.Count,
            FollowerCount = collection.Followers.Count,
            CreatedAt = collection.CreatedAt,
            IsFollowing = !string.IsNullOrEmpty(userId) && collection.Followers.Any(f => f.UserId == userId),
            UserProgress = !string.IsNullOrEmpty(userId)
                ? collection.Followers.FirstOrDefault(f => f.UserId == userId)?.ProgressPercent ?? 0
                : 0,
            IsOwner = collection.OwnerId == userId,
            Materials = collection.Materials
                .OrderBy(cm => cm.Order)
                .Select(cm => new CollectionMaterialDto
                {
                    Id = cm.Id,
                    Order = cm.Order,
                    IsPrerequisite = cm.IsPrerequisite,
                    Material = new MaterialDto
                    {
                        Id = cm.Material.Id,
                        Type = cm.Material.Type,
                        Title = cm.Material.Title,
                        Description = cm.Material.Description,
                        Author = cm.Material.Author,
                        Field = cm.Material.Field,
                        Link = cm.Material.Link,
                        FileType = cm.Material.FileType,
                        ThumbnailUrl = cm.Material.ThumbnailUrl,
                        DurationMinutes = cm.Material.DurationMinutes,
                        DownloadCount = cm.Material.DownloadCount,
                        AverageRating = cm.Material.AverageRating,
                        RatingCount = cm.Material.RatingCount,
                        DifficultyLevel = cm.Material.DifficultyLevel,
                        EstimatedTimeMinutes = cm.Material.EstimatedTimeMinutes,
                        UserProgress = materialProgress?.GetValueOrDefault(cm.MaterialId)
                    },
                    IsCompleted = (materialProgress?.GetValueOrDefault(cm.MaterialId) ?? 0) >= 100
                })
                .ToList()
        };

        return Ok(dto);
    }

    /// <summary>
    /// Create a new collection
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<MaterialCollectionDto>> CreateCollection([FromBody] CreateCollectionRequest request)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var collection = new MaterialCollection
        {
            Title = request.Title,
            Description = request.Description,
            Field = request.Field,
            DifficultyLevel = request.DifficultyLevel,
            IsPublic = request.IsPublic,
            Type = "personal",
            OwnerId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.MaterialCollections.Add(collection);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCollection), new { id = collection.Id }, new MaterialCollectionDto
        {
            Id = collection.Id,
            Title = collection.Title,
            Description = collection.Description,
            Type = collection.Type,
            Field = collection.Field,
            DifficultyLevel = collection.DifficultyLevel,
            IsPublic = collection.IsPublic,
            MaterialCount = 0,
            FollowerCount = 0,
            CreatedAt = collection.CreatedAt,
            IsOwner = true
        });
    }

    /// <summary>
    /// Update a collection (owner only)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateCollection(int id, [FromBody] UpdateCollectionRequest request)
    {
        var userId = _userManager.GetUserId(User);
        var collection = await _context.MaterialCollections.FindAsync(id);

        if (collection == null)
            return NotFound(new { message = "Collection not found" });

        if (collection.OwnerId != userId && !User.IsInRole("Admin"))
            return Forbid();

        if (!string.IsNullOrEmpty(request.Title))
            collection.Title = request.Title;

        if (request.Description != null)
            collection.Description = request.Description;

        if (request.Field != null)
            collection.Field = request.Field;

        if (request.DifficultyLevel != null)
            collection.DifficultyLevel = request.DifficultyLevel;

        if (request.IsPublic.HasValue)
            collection.IsPublic = request.IsPublic.Value;

        collection.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Collection updated successfully" });
    }

    /// <summary>
    /// Delete a collection (owner only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteCollection(int id)
    {
        var userId = _userManager.GetUserId(User);
        var collection = await _context.MaterialCollections.FindAsync(id);

        if (collection == null)
            return NotFound(new { message = "Collection not found" });

        if (collection.OwnerId != userId && !User.IsInRole("Admin"))
            return Forbid();

        _context.MaterialCollections.Remove(collection);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Collection deleted successfully" });
    }

    /// <summary>
    /// Add a material to a collection
    /// </summary>
    [HttpPost("{id}/materials")]
    [Authorize]
    public async Task<IActionResult> AddMaterial(int id, [FromBody] AddMaterialToCollectionRequest request)
    {
        var userId = _userManager.GetUserId(User);
        var collection = await _context.MaterialCollections
            .Include(c => c.Materials)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (collection == null)
            return NotFound(new { message = "Collection not found" });

        if (collection.OwnerId != userId && !User.IsInRole("Admin"))
            return Forbid();

        var material = await _context.Materials.FindAsync(request.MaterialId);
        if (material == null)
            return NotFound(new { message = "Material not found" });

        // Check if already in collection
        if (collection.Materials.Any(m => m.MaterialId == request.MaterialId))
            return BadRequest(new { message = "Material already in collection" });

        var order = request.Order ?? (collection.Materials.Count > 0 
            ? collection.Materials.Max(m => m.Order) + 1 
            : 0);

        var collectionMaterial = new CollectionMaterial
        {
            CollectionId = id,
            MaterialId = request.MaterialId,
            Order = order,
            IsPrerequisite = request.IsPrerequisite,
            AddedAt = DateTime.UtcNow
        };

        _context.CollectionMaterials.Add(collectionMaterial);

        // Update estimated time
        var totalTime = await _context.CollectionMaterials
            .Where(cm => cm.CollectionId == id)
            .Include(cm => cm.Material)
            .SumAsync(cm => cm.Material.EstimatedTimeMinutes ?? 0);

        collection.EstimatedTimeMinutes = totalTime + (material.EstimatedTimeMinutes ?? 0);
        collection.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Material added to collection" });
    }

    /// <summary>
    /// Remove a material from a collection
    /// </summary>
    [HttpDelete("{id}/materials/{materialId}")]
    [Authorize]
    public async Task<IActionResult> RemoveMaterial(int id, int materialId)
    {
        var userId = _userManager.GetUserId(User);
        var collection = await _context.MaterialCollections.FindAsync(id);

        if (collection == null)
            return NotFound(new { message = "Collection not found" });

        if (collection.OwnerId != userId && !User.IsInRole("Admin"))
            return Forbid();

        var collectionMaterial = await _context.CollectionMaterials
            .FirstOrDefaultAsync(cm => cm.CollectionId == id && cm.MaterialId == materialId);

        if (collectionMaterial == null)
            return NotFound(new { message = "Material not in collection" });

        _context.CollectionMaterials.Remove(collectionMaterial);
        collection.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Material removed from collection" });
    }

    /// <summary>
    /// Reorder materials in a collection
    /// </summary>
    [HttpPut("{id}/materials/reorder")]
    [Authorize]
    public async Task<IActionResult> ReorderMaterials(int id, [FromBody] List<int> materialIds)
    {
        var userId = _userManager.GetUserId(User);
        var collection = await _context.MaterialCollections
            .Include(c => c.Materials)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (collection == null)
            return NotFound(new { message = "Collection not found" });

        if (collection.OwnerId != userId && !User.IsInRole("Admin"))
            return Forbid();

        for (int i = 0; i < materialIds.Count; i++)
        {
            var cm = collection.Materials.FirstOrDefault(m => m.MaterialId == materialIds[i]);
            if (cm != null)
            {
                cm.Order = i;
            }
        }

        collection.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Materials reordered successfully" });
    }

    /// <summary>
    /// Follow a collection
    /// </summary>
    [HttpPost("{id}/follow")]
    [Authorize]
    public async Task<IActionResult> FollowCollection(int id)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var collection = await _context.MaterialCollections.FindAsync(id);
        if (collection == null)
            return NotFound(new { message = "Collection not found" });

        if (!collection.IsPublic && collection.OwnerId != userId)
            return Forbid();

        var existing = await _context.CollectionFollowers
            .FirstOrDefaultAsync(f => f.CollectionId == id && f.UserId == userId);

        if (existing != null)
            return BadRequest(new { message = "Already following this collection" });

        var follower = new CollectionFollower
        {
            CollectionId = id,
            UserId = userId,
            ProgressPercent = 0,
            FollowedAt = DateTime.UtcNow
        };

        _context.CollectionFollowers.Add(follower);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Following collection" });
    }

    /// <summary>
    /// Unfollow a collection
    /// </summary>
    [HttpDelete("{id}/follow")]
    [Authorize]
    public async Task<IActionResult> UnfollowCollection(int id)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var follower = await _context.CollectionFollowers
            .FirstOrDefaultAsync(f => f.CollectionId == id && f.UserId == userId);

        if (follower == null)
            return NotFound(new { message = "Not following this collection" });

        _context.CollectionFollowers.Remove(follower);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Unfollowed collection" });
    }

    /// <summary>
    /// Get my collections
    /// </summary>
    [HttpGet("my-collections")]
    [Authorize]
    public async Task<ActionResult<List<MaterialCollectionDto>>> GetMyCollections()
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var collections = await _context.MaterialCollections
            .Where(c => c.OwnerId == userId)
            .Include(c => c.Materials)
            .Include(c => c.Followers)
            .OrderByDescending(c => c.UpdatedAt ?? c.CreatedAt)
            .ToListAsync();

        return Ok(collections.Select(c => new MaterialCollectionDto
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            CoverImageUrl = c.CoverImageUrl,
            Type = c.Type,
            Field = c.Field,
            DifficultyLevel = c.DifficultyLevel,
            EstimatedTimeMinutes = c.EstimatedTimeMinutes,
            IsSystemPath = c.IsSystemPath,
            IsPublic = c.IsPublic,
            MaterialCount = c.Materials.Count,
            FollowerCount = c.Followers.Count,
            CreatedAt = c.CreatedAt,
            IsOwner = true
        }));
    }

    /// <summary>
    /// Get collections I'm following
    /// </summary>
    [HttpGet("following")]
    [Authorize]
    public async Task<ActionResult<List<MaterialCollectionDto>>> GetFollowingCollections()
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var collections = await _context.CollectionFollowers
            .Where(f => f.UserId == userId)
            .Include(f => f.Collection)
                .ThenInclude(c => c.Owner)
            .Include(f => f.Collection)
                .ThenInclude(c => c.Materials)
            .OrderByDescending(f => f.FollowedAt)
            .ToListAsync();

        return Ok(collections.Select(f => new MaterialCollectionDto
        {
            Id = f.Collection.Id,
            Title = f.Collection.Title,
            Description = f.Collection.Description,
            CoverImageUrl = f.Collection.CoverImageUrl,
            Type = f.Collection.Type,
            Field = f.Collection.Field,
            DifficultyLevel = f.Collection.DifficultyLevel,
            EstimatedTimeMinutes = f.Collection.EstimatedTimeMinutes,
            IsSystemPath = f.Collection.IsSystemPath,
            IsPublic = f.Collection.IsPublic,
            OwnerName = f.Collection.Owner.FullName ?? f.Collection.Owner.UserName ?? "Unknown",
            MaterialCount = f.Collection.Materials.Count,
            CreatedAt = f.Collection.CreatedAt,
            IsFollowing = true,
            UserProgress = f.ProgressPercent,
            IsOwner = false
        }));
    }

    /// <summary>
    /// Get learning paths (system-created collections)
    /// </summary>
    [HttpGet("learning-paths")]
    public async Task<ActionResult<List<MaterialCollectionDto>>> GetLearningPaths([FromQuery] string? field = null)
    {
        var userId = User.Identity?.IsAuthenticated == true
            ? _userManager.GetUserId(User)
            : null;

        var query = _context.MaterialCollections
            .Where(c => c.IsSystemPath || c.Type == "learning_path")
            .Include(c => c.Owner)
            .Include(c => c.Materials)
            .Include(c => c.Followers)
            .AsQueryable();

        if (!string.IsNullOrEmpty(field))
        {
            query = query.Where(c => c.Field == field);
        }

        var collections = await query
            .OrderBy(c => c.DifficultyLevel)
            .ThenByDescending(c => c.Followers.Count)
            .ToListAsync();

        return Ok(collections.Select(c => new MaterialCollectionDto
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            CoverImageUrl = c.CoverImageUrl,
            Type = c.Type,
            Field = c.Field,
            DifficultyLevel = c.DifficultyLevel,
            EstimatedTimeMinutes = c.EstimatedTimeMinutes,
            IsSystemPath = c.IsSystemPath,
            IsPublic = c.IsPublic,
            OwnerName = "GradLink",
            MaterialCount = c.Materials.Count,
            FollowerCount = c.Followers.Count,
            CreatedAt = c.CreatedAt,
            IsFollowing = !string.IsNullOrEmpty(userId) && c.Followers.Any(f => f.UserId == userId),
            UserProgress = !string.IsNullOrEmpty(userId)
                ? c.Followers.FirstOrDefault(f => f.UserId == userId)?.ProgressPercent ?? 0
                : 0,
            IsOwner = false
        }));
    }
}















