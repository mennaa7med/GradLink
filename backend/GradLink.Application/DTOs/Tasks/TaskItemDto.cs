namespace GradLink.Application.DTOs.Tasks;

public class TaskItemDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public List<string> Tags { get; set; } = new();
    public DateTime? DueDate { get; set; }
    public string Priority { get; set; } = "Medium";
    public string? Person { get; set; }
    public string Status { get; set; } = "Pending";
    public string Category { get; set; } = "Other";
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int? ProjectId { get; set; }
    public List<SubtaskDto> Subtasks { get; set; } = new();
}

public class SubtaskDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool Completed { get; set; }
}

public class CreateTaskRequest
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public string? Tags { get; set; } // Comma-separated
    public DateTime? DueDate { get; set; }
    public string Priority { get; set; } = "Medium";
    public string? Person { get; set; }
    public string Status { get; set; } = "Pending";
    public string Category { get; set; } = "Other";
    public int? ProjectId { get; set; }
}

public class UpdateTaskRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public string? Tags { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Priority { get; set; }
    public string? Person { get; set; }
    public string? Status { get; set; }
    public string? Category { get; set; }
}

public class CreateSubtaskRequest
{
    public required string Name { get; set; }
}

public class UpdateSubtaskRequest
{
    public string? Name { get; set; }
    public bool? Completed { get; set; }
}

