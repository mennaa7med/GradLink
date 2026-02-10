using System.ComponentModel.DataAnnotations;

namespace GradLink.Application.DTOs.Auth;

public class ResetPasswordRequest
{
    [Required]
    public required string Token { get; set; }

    [Required]
    [MinLength(6)]
    public required string NewPassword { get; set; }
}

















