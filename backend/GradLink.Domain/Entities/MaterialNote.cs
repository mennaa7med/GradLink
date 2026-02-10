using System;

namespace GradLink.Domain.Entities
{
    /// <summary>
    /// Personal notes on a material
    /// </summary>
    public class MaterialNote
    {
        public int Id { get; set; }
        
        public int MaterialId { get; set; }
        public Material Material { get; set; } = null!;
        
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
        
        /// <summary>
        /// Note content
        /// </summary>
        public string Content { get; set; } = string.Empty;
        
        /// <summary>
        /// Optional position reference (page, timestamp, section)
        /// </summary>
        public string? Position { get; set; }
        
        /// <summary>
        /// Highlight color (if applicable)
        /// </summary>
        public string? Color { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}















