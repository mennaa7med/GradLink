namespace GradLink.Domain.Entities;

/// <summary>
/// Represents a file attachment for a task
/// </summary>
public class TaskAttachment
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string StoragePath { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string UploadedById { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }

    public virtual TaskItem? Task { get; set; }
    public virtual ApplicationUser? UploadedBy { get; set; }
}















