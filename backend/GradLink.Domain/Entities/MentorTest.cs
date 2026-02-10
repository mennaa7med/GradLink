using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradLink.Domain.Entities
{
    /// <summary>
    /// Represents a test instance sent to a mentor applicant
    /// </summary>
    public class MentorTest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ApplicationId { get; set; }

        [ForeignKey("ApplicationId")]
        public virtual MentorApplication Application { get; set; } = null!;

        /// <summary>
        /// Unique token for accessing the test
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Test status: Pending, InProgress, Completed, Expired
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        /// <summary>
        /// When the test link expires
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// When the applicant started the test
        /// </summary>
        public DateTime? StartedAt { get; set; }

        /// <summary>
        /// When the applicant completed the test
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// Time limit in minutes
        /// </summary>
        public int TimeLimitMinutes { get; set; } = 20;

        /// <summary>
        /// Total questions in this test
        /// </summary>
        public int TotalQuestions { get; set; }

        /// <summary>
        /// Number of correct answers
        /// </summary>
        public int CorrectAnswers { get; set; }

        /// <summary>
        /// Score as percentage
        /// </summary>
        public decimal? Score { get; set; }

        /// <summary>
        /// JSON string of question IDs included in this test
        /// </summary>
        public string? QuestionIds { get; set; }

        /// <summary>
        /// JSON string of answers submitted
        /// </summary>
        public string? SubmittedAnswers { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}















