namespace GradLink.Domain.Entities;

public class Subtask
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool Completed { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign key to TaskItem
    public int TaskItemId { get; set; }
    public TaskItem? TaskItem { get; set; }
}

