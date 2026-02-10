using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradLink.Domain.Entities
{
    /// <summary>
    /// Represents a mentor application submitted by a user
    /// </summary>
    public class MentorApplication
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string Specialization { get; set; } = string.Empty;

        [Range(0, 50)]
        public int YearsOfExperience { get; set; }

        [StringLength(500)]
        public string? LinkedInUrl { get; set; }

        [StringLength(2000)]
        public string? Bio { get; set; }

        [StringLength(500)]
        public string? CurrentPosition { get; set; }

        [StringLength(500)]
        public string? Company { get; set; }

        /// <summary>
        /// Application status: Pending, TestSent, TestCompleted, Approved, Rejected
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Date when the applicant can retry (if rejected)
        /// </summary>
        public DateTime? RetryAllowedAt { get; set; }

        /// <summary>
        /// Number of test attempts
        /// </summary>
        public int TestAttempts { get; set; } = 0;

        /// <summary>
        /// Final test score (percentage)
        /// </summary>
        public decimal? FinalScore { get; set; }

        /// <summary>
        /// User ID if account was created after approval
        /// </summary>
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }

        /// <summary>
        /// Related test records
        /// </summary>
        public virtual ICollection<MentorTest> Tests { get; set; } = new List<MentorTest>();
    }
}















