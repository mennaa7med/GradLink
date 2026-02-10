using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradLink.Domain.Entities;

public class Internship
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(3000)]
    public string? Description { get; set; }

    [MaxLength(500)]
    public string? Requirements { get; set; }

    [MaxLength(500)]
    public string? Skills { get; set; }

    [MaxLength(200)]
    public string? Location { get; set; }

    [MaxLength(100)]
    public string? Duration { get; set; } // e.g., "3 months", "6 months"

    public bool IsPaid { get; set; }

    public decimal? Stipend { get; set; }

    [MaxLength(200)]
    public string? CompanyName { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "Active"; // Active, Closed, Draft

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }

    // Foreign key to user who posted
    [Required]
    public string PostedById { get; set; } = string.Empty;

    [ForeignKey(nameof(PostedById))]
    public ApplicationUser? PostedBy { get; set; }

    // Navigation property for applications
    public ICollection<InternshipApplication>? Applications { get; set; }
}

















