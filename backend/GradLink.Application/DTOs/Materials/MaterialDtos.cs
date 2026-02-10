using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GradLink.Application.DTOs.Materials
{
    /// <summary>
    /// Material response DTO
    /// </summary>
    public class MaterialDto
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Author { get; set; }
        public string Field { get; set; } = string.Empty;
        public string? Link { get; set; }
        public string? FilePath { get; set; }
        public string? FileType { get; set; }
        public string? ThumbnailUrl { get; set; }
        public int? DurationMinutes { get; set; }
        public int DownloadCount { get; set; }
        public double AverageRating { get; set; }
        public int RatingCount { get; set; }
        public string? Tags { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? SubmittedByName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsFeatured { get; set; }
        public string? DifficultyLevel { get; set; }
        public int? EstimatedTimeMinutes { get; set; }
        
        // User-specific data (if authenticated)
        public bool IsBookmarked { get; set; }
        public int? UserProgress { get; set; }
        public int? UserRating { get; set; }
    }
    
    /// <summary>
    /// Request to create a new material
    /// </summary>
    public class CreateMaterialRequest
    {
        [Required]
        public string Type { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string? Author { get; set; }
        
        [Required]
        public string Field { get; set; } = string.Empty;
        
        [Url]
        public string? Link { get; set; }
        
        public string? FileType { get; set; }
        public string? Tags { get; set; }
        public string? DifficultyLevel { get; set; }
        public int? EstimatedTimeMinutes { get; set; }
        public int? DurationMinutes { get; set; }
    }
    
    /// <summary>
    /// Request to update a material
    /// </summary>
    public class UpdateMaterialRequest
    {
        [StringLength(200)]
        public string? Title { get; set; }
        
        [StringLength(2000)]
        public string? Description { get; set; }
        
        [StringLength(200)]
        public string? Author { get; set; }
        
        public string? Field { get; set; }
        
        [Url]
        public string? Link { get; set; }
        
        public string? Tags { get; set; }
        public string? DifficultyLevel { get; set; }
        public int? EstimatedTimeMinutes { get; set; }
        public bool? IsFeatured { get; set; }
    }
    
    /// <summary>
    /// Request to approve/reject a material
    /// </summary>
    public class ReviewMaterialRequest
    {
        [Required]
        public string Status { get; set; } = string.Empty; // approved, rejected
        
        public string? ReviewNotes { get; set; }
    }
    
    /// <summary>
    /// Filter options for materials
    /// </summary>
    public class MaterialFilterDto
    {
        public string? Type { get; set; }
        public string? Field { get; set; }
        public string? SearchTerm { get; set; }
        public string? Status { get; set; }
        public string? DifficultyLevel { get; set; }
        public string? SortBy { get; set; } // popular, newest, rating, name
        public bool? IsFeatured { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
    
    /// <summary>
    /// Paginated response for materials
    /// </summary>
    public class MaterialsPagedResponse
    {
        public List<MaterialDto> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
    
    // ============ RATING DTOs ============
    
    public class MaterialRatingDto
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? UserAvatar { get; set; }
        public int Rating { get; set; }
        public string? ReviewText { get; set; }
        public int HelpfulCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsOwnReview { get; set; }
    }
    
    public class CreateRatingRequest
    {
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        
        [StringLength(1000)]
        public string? ReviewText { get; set; }
    }
    
    // ============ PROGRESS DTOs ============
    
    public class MaterialProgressDto
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public string MaterialTitle { get; set; } = string.Empty;
        public string MaterialType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int ProgressPercent { get; set; }
        public int TimeSpentMinutes { get; set; }
        public string? LastPosition { get; set; }
        public bool IsBookmarked { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime LastAccessedAt { get; set; }
    }
    
    public class UpdateProgressRequest
    {
        public string? Status { get; set; }
        
        [Range(0, 100)]
        public int? ProgressPercent { get; set; }
        
        public int? TimeSpentMinutes { get; set; }
        public string? LastPosition { get; set; }
        public bool? IsBookmarked { get; set; }
    }
    
    // ============ NOTE DTOs ============
    
    public class MaterialNoteDto
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? Position { get; set; }
        public string? Color { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    
    public class CreateNoteRequest
    {
        [Required]
        [StringLength(5000)]
        public string Content { get; set; } = string.Empty;
        
        public string? Position { get; set; }
        public string? Color { get; set; }
    }
    
    public class UpdateNoteRequest
    {
        [StringLength(5000)]
        public string? Content { get; set; }
        
        public string? Position { get; set; }
        public string? Color { get; set; }
    }
    
    // ============ COLLECTION DTOs ============
    
    public class MaterialCollectionDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }
        public string Type { get; set; } = string.Empty;
        public string? Field { get; set; }
        public string? DifficultyLevel { get; set; }
        public int? EstimatedTimeMinutes { get; set; }
        public bool IsSystemPath { get; set; }
        public bool IsPublic { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public int MaterialCount { get; set; }
        public int FollowerCount { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // User-specific
        public bool IsFollowing { get; set; }
        public int UserProgress { get; set; }
        public bool IsOwner { get; set; }
    }
    
    public class CollectionDetailDto : MaterialCollectionDto
    {
        public List<CollectionMaterialDto> Materials { get; set; } = new();
    }
    
    public class CollectionMaterialDto
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public bool IsPrerequisite { get; set; }
        public MaterialDto Material { get; set; } = null!;
        public bool IsCompleted { get; set; }
    }
    
    public class CreateCollectionRequest
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        public string? Field { get; set; }
        public string? DifficultyLevel { get; set; }
        public bool IsPublic { get; set; } = false;
    }
    
    public class UpdateCollectionRequest
    {
        [StringLength(200)]
        public string? Title { get; set; }
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        public string? Field { get; set; }
        public string? DifficultyLevel { get; set; }
        public bool? IsPublic { get; set; }
    }
    
    public class AddMaterialToCollectionRequest
    {
        [Required]
        public int MaterialId { get; set; }
        
        public int? Order { get; set; }
        public bool IsPrerequisite { get; set; } = false;
    }
    
    // ============ STATS & RECOMMENDATIONS ============
    
    public class MaterialStatsDto
    {
        public int TotalMaterials { get; set; }
        public int TotalTemplates { get; set; }
        public int TotalTools { get; set; }
        public int TotalBooks { get; set; }
        public int TotalDatasets { get; set; }
        public int TotalVideos { get; set; }
        public int TotalDownloads { get; set; }
        public int PendingApprovals { get; set; }
        public Dictionary<string, int> ByField { get; set; } = new();
    }
    
    public class UserMaterialStatsDto
    {
        public int BookmarkedCount { get; set; }
        public int CompletedCount { get; set; }
        public int InProgressCount { get; set; }
        public int TotalTimeSpentMinutes { get; set; }
        public int ReviewsGiven { get; set; }
        public int CollectionsCreated { get; set; }
        public List<MaterialProgressDto> RecentlyViewed { get; set; } = new();
    }
    
    public class RecommendationDto
    {
        public MaterialDto Material { get; set; } = null!;
        public string Reason { get; set; } = string.Empty;
        public double Score { get; set; }
    }
}















