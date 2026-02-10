using System;
using System.Collections.Generic;

namespace GradLink.Domain.Entities
{
    /// <summary>
    /// A collection/playlist of materials (can be user-created or system learning paths)
    /// </summary>
    public class MaterialCollection
    {
        public int Id { get; set; }
        
        public string Title { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        /// <summary>
        /// Cover image URL
        /// </summary>
        public string? CoverImageUrl { get; set; }
        
        /// <summary>
        /// Type: personal, shared, learning_path
        /// </summary>
        public string Type { get; set; } = "personal";
        
        /// <summary>
        /// Field/track this collection is for
        /// </summary>
        public string? Field { get; set; }
        
        /// <summary>
        /// Difficulty level for learning paths
        /// </summary>
        public string? DifficultyLevel { get; set; }
        
        /// <summary>
        /// Estimated total time in minutes
        /// </summary>
        public int? EstimatedTimeMinutes { get; set; }
        
        /// <summary>
        /// Is this a system-created learning path
        /// </summary>
        public bool IsSystemPath { get; set; } = false;
        
        /// <summary>
        /// Is this collection public/shareable
        /// </summary>
        public bool IsPublic { get; set; } = false;
        
        /// <summary>
        /// Owner of the collection
        /// </summary>
        public string OwnerId { get; set; } = string.Empty;
        public ApplicationUser Owner { get; set; } = null!;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public ICollection<CollectionMaterial> Materials { get; set; } = new List<CollectionMaterial>();
        public ICollection<CollectionFollower> Followers { get; set; } = new List<CollectionFollower>();
    }
    
    /// <summary>
    /// Junction table for materials in a collection
    /// </summary>
    public class CollectionMaterial
    {
        public int Id { get; set; }
        
        public int CollectionId { get; set; }
        public MaterialCollection Collection { get; set; } = null!;
        
        public int MaterialId { get; set; }
        public Material Material { get; set; } = null!;
        
        /// <summary>
        /// Order in the collection
        /// </summary>
        public int Order { get; set; }
        
        /// <summary>
        /// Is this a prerequisite (must complete before next)
        /// </summary>
        public bool IsPrerequisite { get; set; } = false;
        
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Users following a collection
    /// </summary>
    public class CollectionFollower
    {
        public int Id { get; set; }
        
        public int CollectionId { get; set; }
        public MaterialCollection Collection { get; set; } = null!;
        
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
        
        /// <summary>
        /// Progress in this collection (0-100)
        /// </summary>
        public int ProgressPercent { get; set; } = 0;
        
        public DateTime FollowedAt { get; set; } = DateTime.UtcNow;
    }
}















