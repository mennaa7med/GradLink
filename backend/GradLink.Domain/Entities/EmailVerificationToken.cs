namespace GradLink.Domain.Entities;

/// <summary>
/// Represents an email verification token
/// </summary>
public class EmailVerificationToken
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsUsed { get; set; }

    public virtual ApplicationUser? User { get; set; }
}















