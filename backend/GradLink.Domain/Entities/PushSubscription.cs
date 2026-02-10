namespace GradLink.Domain.Entities;

/// <summary>
/// Represents a push notification subscription
/// </summary>
public class PushSubscription
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public string P256dh { get; set; } = string.Empty;
    public string Auth { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public virtual ApplicationUser? User { get; set; }
}















