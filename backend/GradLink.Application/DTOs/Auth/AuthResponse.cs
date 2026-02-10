namespace GradLink.Application.DTOs.Auth;

public class AuthResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public required UserDto User { get; set; }
}

public class UserDto
{
    public required string Id { get; set; }
    public required string Email { get; set; }
    public required string FullName { get; set; }
    public string? ProfilePicture { get; set; }
    public string? CompanyName { get; set; } // For company users
    public List<string> Roles { get; set; } = new();
}

