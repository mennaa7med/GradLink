namespace GradLink.Domain.Entities;

public class RefreshToken
{
    public int Id { get; set; }
    public required string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRevoked { get; set; }
    public string? RevokedByIp { get; set; }
    public DateTime? RevokedAt { get; set; }

    // Foreign key
    public required string UserId { get; set; }
    public ApplicationUser? User { get; set; }
}

