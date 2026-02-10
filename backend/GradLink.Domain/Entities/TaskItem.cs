namespace GradLink.Domain.Entities;

public class TaskItem
{
    public int Id { get; set; }
    
    // Basic Information
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public string? Tags { get; set; } // Comma-separated tags
    
    // Timeline & Priority
    public DateTime? DueDate { get; set; }
    public string Priority { get; set; } = "Medium"; // Low, Medium, High
    
    // Assignment & Status
    public string? Person { get; set; } // Responsible person
    public string Status { get; set; } = "Pending"; // Pending (Not Started), In Progress, Completed
    
    // Category
    public string Category { get; set; } = "Other"; // Frontend, Backend, Presentation, Other
    
    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    // Foreign key to User who owns the task
    public required string UserId { get; set; }
    public ApplicationUser? User { get; set; }

    // Optional: Foreign key to Project
    public int? ProjectId { get; set; }
    public Project? Project { get; set; }

    // Navigation property for subtasks
    public ICollection<Subtask> Subtasks { get; set; } = new List<Subtask>();
}
