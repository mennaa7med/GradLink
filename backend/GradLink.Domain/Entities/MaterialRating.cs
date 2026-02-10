using System;

namespace GradLink.Domain.Entities
{
    /// <summary>
    /// User rating and review for a material
    /// </summary>
    public class MaterialRating
    {
        public int Id { get; set; }
        
        public int MaterialId { get; set; }
        public Material Material { get; set; } = null!;
        
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
        
        /// <summary>
        /// Rating value (1-5 stars)
        /// </summary>
        public int Rating { get; set; }
        
        /// <summary>
        /// Optional review text
        /// </summary>
        public string? ReviewText { get; set; }
        
        /// <summary>
        /// Was this review helpful? (upvotes)
        /// </summary>
        public int HelpfulCount { get; set; } = 0;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}















