using GradLink.Api.DTOs.MentorDashboard;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Controllers.MentorDashboard;

/// <summary>
/// Controller for managing mentor reviews
/// </summary>
[ApiController]
[Route("api/mentor-reviews")]
[Authorize]
public class MentorReviewController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public MentorReviewController(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    /// Get all reviews for a specific mentor (public)
    /// </summary>
    [HttpGet("mentor/{mentorId}")]
    [AllowAnonymous]
    public async Task<ActionResult<List<MentorReviewDto>>> GetMentorReviews(string mentorId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var reviews = await _context.MentorReviews
            .Include(mr => mr.Reviewer)
            .Where(mr => mr.MentorId == mentorId && mr.IsApproved && !mr.IsHidden)
            .OrderByDescending(mr => mr.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(mr => new MentorReviewDto
            {
                Id = mr.Id,
                MentorId = mr.MentorId,
                ReviewerId = mr.ReviewerId,
                ReviewerName = mr.ShowReviewerName && mr.Reviewer != null ? mr.Reviewer.FullName ?? "Anonymous" : "Anonymous",
                ReviewerProfilePicture = mr.ShowReviewerName && mr.Reviewer != null ? mr.Reviewer.ProfilePicture : null,
                Rating = mr.Rating,
                KnowledgeRating = mr.KnowledgeRating,
                CommunicationRating = mr.CommunicationRating,
                AvailabilityRating = mr.AvailabilityRating,
                HelpfulnessRating = mr.HelpfulnessRating,
                Title = mr.Title,
                Comment = mr.Comment,
                Pros = mr.Pros,
                Cons = mr.Cons,
                WouldRecommend = mr.WouldRecommend,
                MentorshipType = mr.MentorshipType,
                IsVerified = mr.IsVerified,
                HelpfulCount = mr.HelpfulCount,
                HasMentorResponse = mr.HasMentorResponse,
                MentorResponse = mr.MentorResponse,
                CreatedAt = mr.CreatedAt
            })
            .ToListAsync();

        return Ok(reviews);
    }

    /// <summary>
    /// Get reviews received by the current mentor
    /// </summary>
    [HttpGet("my-reviews")]
    public async Task<ActionResult<List<MentorReviewDto>>> GetMyReviews()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var reviews = await _context.MentorReviews
            .Include(mr => mr.Reviewer)
            .Where(mr => mr.MentorId == userId)
            .OrderByDescending(mr => mr.CreatedAt)
            .Select(mr => new MentorReviewDto
            {
                Id = mr.Id,
                MentorId = mr.MentorId,
                ReviewerId = mr.ReviewerId,
                ReviewerName = mr.Reviewer != null ? mr.Reviewer.FullName ?? "" : "",
                ReviewerProfilePicture = mr.Reviewer != null ? mr.Reviewer.ProfilePicture : null,
                Rating = mr.Rating,
                KnowledgeRating = mr.KnowledgeRating,
                CommunicationRating = mr.CommunicationRating,
                AvailabilityRating = mr.AvailabilityRating,
                HelpfulnessRating = mr.HelpfulnessRating,
                Title = mr.Title,
                Comment = mr.Comment,
                Pros = mr.Pros,
                Cons = mr.Cons,
                WouldRecommend = mr.WouldRecommend,
                MentorshipType = mr.MentorshipType,
                IsVerified = mr.IsVerified,
                HelpfulCount = mr.HelpfulCount,
                HasMentorResponse = mr.HasMentorResponse,
                MentorResponse = mr.MentorResponse,
                CreatedAt = mr.CreatedAt
            })
            .ToListAsync();

        return Ok(reviews);
    }

    /// <summary>
    /// Get review statistics for a mentor
    /// </summary>
    [HttpGet("mentor/{mentorId}/stats")]
    [AllowAnonymous]
    public async Task<ActionResult> GetMentorReviewStats(string mentorId)
    {
        var reviews = await _context.MentorReviews
            .Where(mr => mr.MentorId == mentorId && mr.IsApproved && !mr.IsHidden)
            .ToListAsync();

        if (!reviews.Any())
        {
            return Ok(new
            {
                TotalReviews = 0,
                AverageRating = 0,
                RatingDistribution = new { Five = 0, Four = 0, Three = 0, Two = 0, One = 0 },
                RecommendationRate = 0,
                AverageKnowledgeRating = 0,
                AverageCommunicationRating = 0,
                AverageAvailabilityRating = 0,
                AverageHelpfulnessRating = 0
            });
        }

        var stats = new
        {
            TotalReviews = reviews.Count,
            AverageRating = Math.Round(reviews.Average(r => r.Rating), 1),
            RatingDistribution = new
            {
                Five = reviews.Count(r => r.Rating == 5),
                Four = reviews.Count(r => r.Rating == 4),
                Three = reviews.Count(r => r.Rating == 3),
                Two = reviews.Count(r => r.Rating == 2),
                One = reviews.Count(r => r.Rating == 1)
            },
            RecommendationRate = Math.Round((double)reviews.Count(r => r.WouldRecommend) / reviews.Count * 100, 0),
            AverageKnowledgeRating = reviews.Where(r => r.KnowledgeRating.HasValue).Any() 
                ? Math.Round(reviews.Where(r => r.KnowledgeRating.HasValue).Average(r => r.KnowledgeRating!.Value), 1) : 0,
            AverageCommunicationRating = reviews.Where(r => r.CommunicationRating.HasValue).Any()
                ? Math.Round(reviews.Where(r => r.CommunicationRating.HasValue).Average(r => r.CommunicationRating!.Value), 1) : 0,
            AverageAvailabilityRating = reviews.Where(r => r.AvailabilityRating.HasValue).Any()
                ? Math.Round(reviews.Where(r => r.AvailabilityRating.HasValue).Average(r => r.AvailabilityRating!.Value), 1) : 0,
            AverageHelpfulnessRating = reviews.Where(r => r.HelpfulnessRating.HasValue).Any()
                ? Math.Round(reviews.Where(r => r.HelpfulnessRating.HasValue).Average(r => r.HelpfulnessRating!.Value), 1) : 0
        };

        return Ok(stats);
    }

    /// <summary>
    /// Create a review for a mentor (student action)
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<MentorReviewDto>> CreateReview([FromBody] CreateReviewDto dto)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        // Check if mentor exists
        var mentor = await _userManager.FindByIdAsync(dto.MentorId);
        if (mentor == null) return NotFound("Mentor not found");

        // Check if user already reviewed this mentor
        var existingReview = await _context.MentorReviews
            .FirstOrDefaultAsync(mr => mr.MentorId == dto.MentorId && mr.ReviewerId == userId);
        
        if (existingReview != null)
        {
            return BadRequest("You have already reviewed this mentor");
        }

        // Check if user has a mentorship relation with this mentor (for verification)
        var hasRelation = await _context.MentorshipRelations
            .AnyAsync(mr => mr.MentorId == dto.MentorId && mr.MenteeId == userId);

        var review = new MentorReview
        {
            MentorId = dto.MentorId,
            ReviewerId = userId,
            MentorshipRelationId = dto.MentorshipRelationId,
            MentoringSessionId = dto.MentoringSessionId,
            Rating = dto.Rating,
            KnowledgeRating = dto.KnowledgeRating,
            CommunicationRating = dto.CommunicationRating,
            AvailabilityRating = dto.AvailabilityRating,
            HelpfulnessRating = dto.HelpfulnessRating,
            Title = dto.Title,
            Comment = dto.Comment,
            Pros = dto.Pros,
            Cons = dto.Cons,
            WouldRecommend = dto.WouldRecommend,
            MentorshipType = dto.MentorshipType,
            ShowReviewerName = dto.ShowReviewerName,
            IsVerified = hasRelation,
            IsApproved = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.MentorReviews.Add(review);

        // Update mentor profile stats
        var mentorProfile = await _context.MentorProfiles
            .FirstOrDefaultAsync(mp => mp.UserId == dto.MentorId);
        
        if (mentorProfile != null)
        {
            var allReviews = await _context.MentorReviews
                .Where(mr => mr.MentorId == dto.MentorId && mr.IsApproved)
                .ToListAsync();
            
            allReviews.Add(review);
            mentorProfile.ReviewsCount = allReviews.Count;
            mentorProfile.AverageRating = (decimal)allReviews.Average(r => r.Rating);
            mentorProfile.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetReview), new { id = review.Id },
            new MentorReviewDto
            {
                Id = review.Id,
                MentorId = review.MentorId,
                ReviewerId = review.ReviewerId,
                Rating = review.Rating,
                Title = review.Title,
                Comment = review.Comment,
                WouldRecommend = review.WouldRecommend,
                IsVerified = review.IsVerified,
                CreatedAt = review.CreatedAt
            });
    }

    /// <summary>
    /// Get a specific review
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<MentorReviewDto>> GetReview(int id)
    {
        var review = await _context.MentorReviews
            .Include(mr => mr.Reviewer)
            .FirstOrDefaultAsync(mr => mr.Id == id && mr.IsApproved && !mr.IsHidden);

        if (review == null) return NotFound();

        return Ok(new MentorReviewDto
        {
            Id = review.Id,
            MentorId = review.MentorId,
            ReviewerId = review.ReviewerId,
            ReviewerName = review.ShowReviewerName && review.Reviewer != null ? review.Reviewer.FullName ?? "Anonymous" : "Anonymous",
            Rating = review.Rating,
            Title = review.Title,
            Comment = review.Comment,
            Pros = review.Pros,
            Cons = review.Cons,
            WouldRecommend = review.WouldRecommend,
            IsVerified = review.IsVerified,
            HelpfulCount = review.HelpfulCount,
            HasMentorResponse = review.HasMentorResponse,
            MentorResponse = review.MentorResponse,
            CreatedAt = review.CreatedAt
        });
    }

    /// <summary>
    /// Respond to a review (mentor action)
    /// </summary>
    [HttpPost("{id}/respond")]
    public async Task<ActionResult> RespondToReview(int id, [FromBody] MentorResponseDto dto)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var review = await _context.MentorReviews
            .FirstOrDefaultAsync(mr => mr.Id == id && mr.MentorId == userId);

        if (review == null) return NotFound();

        review.MentorResponse = dto.Response;
        review.HasMentorResponse = true;
        review.MentorRespondedAt = DateTime.UtcNow;
        review.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Response added successfully" });
    }

    /// <summary>
    /// Mark a review as helpful
    /// </summary>
    [HttpPost("{id}/helpful")]
    public async Task<ActionResult> MarkHelpful(int id)
    {
        var review = await _context.MentorReviews
            .FirstOrDefaultAsync(mr => mr.Id == id);

        if (review == null) return NotFound();

        review.HelpfulCount++;
        await _context.SaveChangesAsync();

        return Ok(new { helpfulCount = review.HelpfulCount });
    }

    /// <summary>
    /// Flag a review for moderation
    /// </summary>
    [HttpPost("{id}/flag")]
    public async Task<ActionResult> FlagReview(int id, [FromBody] string reason)
    {
        var review = await _context.MentorReviews
            .FirstOrDefaultAsync(mr => mr.Id == id);

        if (review == null) return NotFound();

        review.IsFlagged = true;
        review.FlagReason = reason;
        review.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Review flagged for moderation" });
    }

    /// <summary>
    /// Delete a review (only by the reviewer)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteReview(int id)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var review = await _context.MentorReviews
            .FirstOrDefaultAsync(mr => mr.Id == id && mr.ReviewerId == userId);

        if (review == null) return NotFound();

        _context.MentorReviews.Remove(review);

        // Update mentor profile stats
        var mentorProfile = await _context.MentorProfiles
            .FirstOrDefaultAsync(mp => mp.UserId == review.MentorId);
        
        if (mentorProfile != null)
        {
            var remainingReviews = await _context.MentorReviews
                .Where(mr => mr.MentorId == review.MentorId && mr.Id != id && mr.IsApproved)
                .ToListAsync();
            
            mentorProfile.ReviewsCount = remainingReviews.Count;
            mentorProfile.AverageRating = remainingReviews.Any() 
                ? (decimal)remainingReviews.Average(r => r.Rating) : null;
            mentorProfile.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        return Ok(new { message = "Review deleted successfully" });
    }
}






