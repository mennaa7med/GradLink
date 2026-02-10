namespace GradLink.Application.DTOs.Auth;

/// <summary>
/// Request DTO for social login (Google/GitHub)
/// </summary>
public class SocialLoginRequest
{
    /// <summary>
    /// The OAuth provider (google, github)
    /// </summary>
    public string Provider { get; set; } = string.Empty;
    
    /// <summary>
    /// The access token received from the OAuth provider
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;
}

/// <summary>
/// Google user info response
/// </summary>
public class GoogleUserInfo
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Picture { get; set; } = string.Empty;
    public bool Verified_Email { get; set; }
}

/// <summary>
/// GitHub user info response
/// </summary>
public class GitHubUserInfo
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Avatar_Url { get; set; } = string.Empty;
}

/// <summary>
/// GitHub email response
/// </summary>
public class GitHubEmail
{
    public string Email { get; set; } = string.Empty;
    public bool Primary { get; set; }
    public bool Verified { get; set; }
}















