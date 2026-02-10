using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradLink.Domain.Entities;

public class Notification
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [ForeignKey(nameof(UserId))]
    public ApplicationUser? User { get; set; }

    [Required]
    [MaxLength(100)]
    public string Type { get; set; } = string.Empty; // JobApplication, Message, ProjectUpdate, etc.

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Message { get; set; }

    [MaxLength(500)]
    public string? Link { get; set; } // URL to navigate to when clicked

    public bool IsRead { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ReadAt { get; set; }

    // Optional reference to related entity
    public string? RelatedEntityType { get; set; } // Job, Internship, Project, etc.
    
    public int? RelatedEntityId { get; set; }
}

















