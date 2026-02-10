namespace GradLink.Application.DTOs.Tasks;

/// <summary>
/// DTO for task attachment
/// </summary>
public class TaskAttachmentDto
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string? Description { get; set; }
    public string UploadedById { get; set; } = string.Empty;
    public string? UploadedByName { get; set; }
    public DateTime UploadedAt { get; set; }
    public string DownloadUrl { get; set; } = string.Empty;
}















