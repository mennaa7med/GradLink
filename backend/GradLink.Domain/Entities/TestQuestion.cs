using System;
using System.ComponentModel.DataAnnotations;

namespace GradLink.Domain.Entities
{
    /// <summary>
    /// Represents a question in the mentor test question bank
    /// </summary>
    public class TestQuestion
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Category/Specialization this question belongs to
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// The question text
        /// </summary>
        [Required]
        [StringLength(1000)]
        public string QuestionText { get; set; } = string.Empty;

        /// <summary>
        /// Option A
        /// </summary>
        [Required]
        [StringLength(500)]
        public string OptionA { get; set; } = string.Empty;

        /// <summary>
        /// Option B
        /// </summary>
        [Required]
        [StringLength(500)]
        public string OptionB { get; set; } = string.Empty;

        /// <summary>
        /// Option C
        /// </summary>
        [Required]
        [StringLength(500)]
        public string OptionC { get; set; } = string.Empty;

        /// <summary>
        /// Option D
        /// </summary>
        [Required]
        [StringLength(500)]
        public string OptionD { get; set; } = string.Empty;

        /// <summary>
        /// Correct answer (A, B, C, or D)
        /// </summary>
        [Required]
        [StringLength(1)]
        public string CorrectAnswer { get; set; } = string.Empty;

        /// <summary>
        /// Difficulty level: Easy, Medium, Hard
        /// </summary>
        [StringLength(20)]
        public string Difficulty { get; set; } = "Medium";

        /// <summary>
        /// Optional explanation for the correct answer
        /// </summary>
        [StringLength(1000)]
        public string? Explanation { get; set; }

        /// <summary>
        /// Whether this question is active
        /// </summary>
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}















