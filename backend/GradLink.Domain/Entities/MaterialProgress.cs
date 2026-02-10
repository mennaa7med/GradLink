using System;

namespace GradLink.Domain.Entities
{
    /// <summary>
    /// Tracks user progress on a material
    /// </summary>
    public class MaterialProgress
    {
        public int Id { get; set; }
        
        public int MaterialId { get; set; }
        public Material Material { get; set; } = null!;
        
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
        
        /// <summary>
        /// Status: not_started, in_progress, completed
        /// </summary>
        public string Status { get; set; } = "not_started";
        
        /// <summary>
        /// Progress percentage (0-100)
        /// </summary>
        public int ProgressPercent { get; set; } = 0;
        
        /// <summary>
        /// Time spent in minutes
        /// </summary>
        public int TimeSpentMinutes { get; set; } = 0;
        
        /// <summary>
        /// Last position (for videos: timestamp, for books: page number)
        /// </summary>
        public string? LastPosition { get; set; }
        
        /// <summary>
        /// Is this bookmarked/favorited
        /// </summary>
        public bool IsBookmarked { get; set; } = false;
        
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime LastAccessedAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}















