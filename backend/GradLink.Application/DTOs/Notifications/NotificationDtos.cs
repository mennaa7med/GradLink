namespace GradLink.Application.DTOs.Notifications;

public class NotificationDto
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Message { get; set; }
    public string? Link { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }
    public string? RelatedEntityType { get; set; }
    public int? RelatedEntityId { get; set; }
}

public class CreateNotificationRequest
{
    public required string UserId { get; set; }
    public required string Type { get; set; }
    public required string Title { get; set; }
    public string? Message { get; set; }
    public string? Link { get; set; }
    public string? RelatedEntityType { get; set; }
    public int? RelatedEntityId { get; set; }
}

public class NotificationCountDto
{
    public int Total { get; set; }
    public int Unread { get; set; }
}

















