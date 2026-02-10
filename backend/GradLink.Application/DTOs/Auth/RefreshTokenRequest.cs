using System.ComponentModel.DataAnnotations;

namespace GradLink.Application.DTOs.Auth;

public class RefreshTokenRequest
{
    [Required]
    public required string RefreshToken { get; set; }
}

