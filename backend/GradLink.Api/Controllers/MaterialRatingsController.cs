using GradLink.Application.DTOs.Materials;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Controllers;

/// <summary>
/// Controller for managing material ratings and reviews
/// </summary>
[ApiController]
[Route("api/materials/{materialId}/ratings")]
[Produces("application/json")]
public class MaterialRatingsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public MaterialRatingsController(
        AppDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    /// Get all ratings for a material
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<MaterialRatingDto>>> GetRatings(int materialId)
    {
        var userId = User.Identity?.IsAuthenticated == true
            ? _userManager.GetUserId(User)
            : null;

        var ratings = await _context.MaterialRatings
            .Where(r => r.MaterialId == materialId)
            .Include(r => r.User)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return Ok(ratings.Select(r => new MaterialRatingDto
        {
            Id = r.Id,
            MaterialId = r.MaterialId,
            UserId = r.UserId,
            UserName = r.User.FullName ?? r.User.UserName ?? "Anonymous",
            UserAvatar = r.User.ProfilePicture,
            Rating = r.Rating,
            ReviewText = r.ReviewText,
            HelpfulCount = r.HelpfulCount,
            CreatedAt = r.CreatedAt,
            IsOwnReview = r.UserId == userId
        }));
    }

    /// <summary>
    /// Add or update a rating for a material
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<MaterialRatingDto>> AddRating(int materialId, [FromBody] CreateRatingRequest request)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var material = await _context.Materials.FindAsync(materialId);
        if (material == null)
            return NotFound(new { message = "Material not found" });

        // Check if user already rated
        var existingRating = await _context.MaterialRatings
            .FirstOrDefaultAsync(r => r.MaterialId == materialId && r.UserId == userId);

        if (existingRating != null)
        {
            // Update existing rating
            existingRating.Rating = request.Rating;
            existingRating.ReviewText = request.ReviewText;
            existingRating.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            // Create new rating
            existingRating = new MaterialRating
            {
                MaterialId = materialId,
                UserId = userId,
                Rating = request.Rating,
                ReviewText = request.ReviewText,
                CreatedAt = DateTime.UtcNow
            };
            _context.MaterialRatings.Add(existingRating);
        }

        await _context.SaveChangesAsync();

        // Update material's average rating
        var allRatings = await _context.MaterialRatings
            .Where(r => r.MaterialId == materialId)
            .ToListAsync();

        material.RatingCount = allRatings.Count;
        material.AverageRating = allRatings.Average(r => r.Rating);

        await _context.SaveChangesAsync();

        var user = await _userManager.FindByIdAsync(userId);

        return Ok(new MaterialRatingDto
        {
            Id = existingRating.Id,
            MaterialId = existingRating.MaterialId,
            UserId = existingRating.UserId,
            UserName = user?.FullName ?? user?.UserName ?? "Anonymous",
            UserAvatar = user?.ProfilePicture,
            Rating = existingRating.Rating,
            ReviewText = existingRating.ReviewText,
            HelpfulCount = existingRating.HelpfulCount,
            CreatedAt = existingRating.CreatedAt,
            IsOwnReview = true
        });
    }

    /// <summary>
    /// Delete a rating (owner only)
    /// </summary>
    [HttpDelete("{ratingId}")]
    [Authorize]
    public async Task<IActionResult> DeleteRating(int materialId, int ratingId)
    {
        var userId = _userManager.GetUserId(User);

        var rating = await _context.MaterialRatings.FindAsync(ratingId);
        if (rating == null || rating.MaterialId != materialId)
            return NotFound(new { message = "Rating not found" });

        if (rating.UserId != userId && !User.IsInRole("Admin"))
            return Forbid();

        _context.MaterialRatings.Remove(rating);
        await _context.SaveChangesAsync();

        // Update material's average rating
        var material = await _context.Materials.FindAsync(materialId);
        if (material != null)
        {
            var allRatings = await _context.MaterialRatings
                .Where(r => r.MaterialId == materialId)
                .ToListAsync();

            material.RatingCount = allRatings.Count;
            material.AverageRating = allRatings.Count > 0 ? allRatings.Average(r => r.Rating) : 0;

            await _context.SaveChangesAsync();
        }

        return Ok(new { message = "Rating deleted successfully" });
    }

    /// <summary>
    /// Mark a rating as helpful
    /// </summary>
    [HttpPost("{ratingId}/helpful")]
    [Authorize]
    public async Task<IActionResult> MarkHelpful(int materialId, int ratingId)
    {
        var rating = await _context.MaterialRatings.FindAsync(ratingId);
        if (rating == null || rating.MaterialId != materialId)
            return NotFound(new { message = "Rating not found" });

        rating.HelpfulCount++;
        await _context.SaveChangesAsync();

        return Ok(new { helpfulCount = rating.HelpfulCount });
    }
}















