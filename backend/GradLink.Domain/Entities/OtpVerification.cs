using System.ComponentModel.DataAnnotations;

namespace GradLink.Domain.Entities;

/// <summary>
/// Represents an OTP verification code for email verification
/// </summary>
public class OtpVerification
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;
    public virtual ApplicationUser? User { get; set; }

    [MaxLength(6)]
    public string OtpCode { get; set; } = string.Empty;

    [MaxLength(256)]
    public string Email { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    
    public bool IsUsed { get; set; }
    public int AttemptCount { get; set; } = 0;
}

