using System;
using System.Collections.Generic;

namespace GradLink.Domain.Entities
{
    /// <summary>
    /// Represents a learning material/resource in the system
    /// </summary>
    public class Material
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Type of material: template, tool, book, dataset, video
        /// </summary>
        public string Type { get; set; } = string.Empty;
        
        public string Title { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// Author name (for books)
        /// </summary>
        public string? Author { get; set; }
        
        /// <summary>
        /// Field/category: computer science, engineering, business, medicine, arts
        /// </summary>
        public string Field { get; set; } = string.Empty;
        
        /// <summary>
        /// External link or download URL
        /// </summary>
        public string? Link { get; set; }
        
        /// <summary>
        /// Local file path for downloadable templates
        /// </summary>
        public string? FilePath { get; set; }
        
        /// <summary>
        /// File type for templates (DOCX, PPTX, PDF, etc.)
        /// </summary>
        public string? FileType { get; set; }
        
        /// <summary>
        /// Thumbnail/preview image URL
        /// </summary>
        public string? ThumbnailUrl { get; set; }
        
        /// <summary>
        /// Video duration in minutes (for videos)
        /// </summary>
        public int? DurationMinutes { get; set; }
        
        /// <summary>
        /// Number of downloads/views
        /// </summary>
        public int DownloadCount { get; set; } = 0;
        
        /// <summary>
        /// Average rating (1-5)
        /// </summary>
        public double AverageRating { get; set; } = 0;
        
        /// <summary>
        /// Total number of ratings
        /// </summary>
        public int RatingCount { get; set; } = 0;
        
        /// <summary>
        /// Tags for advanced search
        /// </summary>
        public string? Tags { get; set; }
        
        /// <summary>
        /// Status: pending, approved, rejected
        /// </summary>
        public string Status { get; set; } = "pending";
        
        /// <summary>
        /// User who submitted this material
        /// </summary>
        public string? SubmittedById { get; set; }
        public ApplicationUser? SubmittedBy { get; set; }
        
        /// <summary>
        /// Admin who approved/rejected
        /// </summary>
        public string? ReviewedById { get; set; }
        public ApplicationUser? ReviewedBy { get; set; }
        
        /// <summary>
        /// Review notes from admin
        /// </summary>
        public string? ReviewNotes { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        
        /// <summary>
        /// Is this a featured/recommended material
        /// </summary>
        public bool IsFeatured { get; set; } = false;
        
        /// <summary>
        /// Difficulty level: beginner, intermediate, advanced
        /// </summary>
        public string? DifficultyLevel { get; set; }
        
        /// <summary>
        /// Estimated time to complete in minutes
        /// </summary>
        public int? EstimatedTimeMinutes { get; set; }
        
        // Navigation properties (existing)
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<RecentlyViewed> RecentlyViewed { get; set; } = new List<RecentlyViewed>();
        
        // Navigation properties (new)
        public ICollection<MaterialRating> Ratings { get; set; } = new List<MaterialRating>();
        public ICollection<MaterialProgress> ProgressRecords { get; set; } = new List<MaterialProgress>();
        public ICollection<MaterialNote> Notes { get; set; } = new List<MaterialNote>();
        public ICollection<CollectionMaterial> CollectionMaterials { get; set; } = new List<CollectionMaterial>();
    }
}
