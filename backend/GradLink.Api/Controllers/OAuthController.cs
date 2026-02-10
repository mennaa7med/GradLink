using System.Net.Http.Headers;
using System.Text.Json;
using GradLink.Application.DTOs.Auth;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GradLink.Api.Controllers;

/// <summary>
/// Controller for handling OAuth/Social Login
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OAuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtTokenService _jwtTokenService;
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<OAuthController> _logger;

    public OAuthController(
        UserManager<ApplicationUser> userManager,
        JwtTokenService jwtTokenService,
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory,
        ILogger<OAuthController> logger)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    /// <summary>
    /// Login or register using a social provider (Google/GitHub)
    /// </summary>
    /// <param name="request">Social login request with provider and access token</param>
    /// <returns>JWT tokens if successful</returns>
    [HttpPost("social-login")]
    public async Task<IActionResult> SocialLogin([FromBody] SocialLoginRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Provider) || string.IsNullOrEmpty(request.AccessToken))
            {
                return BadRequest(new { message = "Provider and access token are required" });
            }

            string email;
            string name;
            string? pictureUrl;
            string providerKey;

            switch (request.Provider.ToLower())
            {
                case "google":
                    var googleUser = await GetGoogleUserInfo(request.AccessToken);
                    if (googleUser == null)
                    {
                        return BadRequest(new { message = "Invalid Google token" });
                    }
                    email = googleUser.Email;
                    name = googleUser.Name;
                    pictureUrl = googleUser.Picture;
                    providerKey = googleUser.Id;
                    break;

                case "github":
                    var githubUser = await GetGitHubUserInfo(request.AccessToken);
                    if (githubUser == null)
                    {
                        return BadRequest(new { message = "Invalid GitHub token" });
                    }
                    email = githubUser.Email;
                    name = githubUser.Name ?? githubUser.Login;
                    pictureUrl = githubUser.Avatar_Url;
                    providerKey = githubUser.Id.ToString();
                    break;

                default:
                    return BadRequest(new { message = "Unsupported provider. Use 'google' or 'github'" });
            }

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { message = "Could not retrieve email from provider. Please ensure email access is granted." });
            }

            // Find or create user
            var user = await _userManager.FindByEmailAsync(email);
            
            if (user == null)
            {
                // Create new user
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FullName = name,
                    ProfilePicture = pictureUrl,
                    EmailConfirmed = true, // OAuth emails are verified
                    IsVerified = true,
                    CreatedAt = DateTime.UtcNow
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    _logger.LogError("Failed to create user via OAuth: {Errors}", errors);
                    return BadRequest(new { message = "Failed to create user", errors });
                }

                // Add default role
                await _userManager.AddToRoleAsync(user, "Student");

                // Add login provider
                await _userManager.AddLoginAsync(user, new UserLoginInfo(
                    request.Provider.ToLower(),
                    providerKey,
                    request.Provider));
            }
            else
            {
                // Check if this provider is already linked
                var logins = await _userManager.GetLoginsAsync(user);
                var hasProvider = logins.Any(l => l.LoginProvider == request.Provider.ToLower());
                
                if (!hasProvider)
                {
                    await _userManager.AddLoginAsync(user, new UserLoginInfo(
                        request.Provider.ToLower(),
                        providerKey,
                        request.Provider));
                }

                // Update profile picture if not set
                if (string.IsNullOrEmpty(user.ProfilePicture) && !string.IsNullOrEmpty(pictureUrl))
                {
                    user.ProfilePicture = pictureUrl;
                    await _userManager.UpdateAsync(user);
                }
            }

            // Generate tokens
            var accessToken = await _jwtTokenService.GenerateAccessTokenAsync(user);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            // Get user roles
            var userRoles = await _userManager.GetRolesAsync(user);

            // Update user
            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                accessToken,
                refreshToken,
                expiresIn = 3600,
                user = new
                {
                    id = user.Id,
                    email = user.Email,
                    fullName = user.FullName,
                    profilePicture = user.ProfilePicture,
                    roles = userRoles
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during social login");
            return StatusCode(500, new { message = "An error occurred during authentication" });
        }
    }

    /// <summary>
    /// Get OAuth provider URLs for frontend redirect
    /// </summary>
    [HttpGet("providers")]
    public IActionResult GetProviders()
    {
        var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:5177";
        var googleClientId = _configuration["OAuth:Google:ClientId"];
        var githubClientId = _configuration["OAuth:GitHub:ClientId"];

        return Ok(new
        {
            google = new
            {
                enabled = !string.IsNullOrEmpty(googleClientId),
                clientId = googleClientId,
                authUrl = $"https://accounts.google.com/o/oauth2/v2/auth",
                scope = "openid email profile",
                redirectUri = $"{frontendUrl}/oauth/callback/google"
            },
            github = new
            {
                enabled = !string.IsNullOrEmpty(githubClientId),
                clientId = githubClientId,
                authUrl = "https://github.com/login/oauth/authorize",
                scope = "user:email",
                redirectUri = $"{frontendUrl}/oauth/callback/github"
            }
        });
    }

    /// <summary>
    /// Exchange authorization code for access token (for OAuth callback)
    /// </summary>
    [HttpPost("exchange-code")]
    public async Task<IActionResult> ExchangeCode([FromBody] ExchangeCodeRequest request)
    {
        try
        {
            string accessToken;

            switch (request.Provider.ToLower())
            {
                case "google":
                    accessToken = await ExchangeGoogleCode(request.Code, request.RedirectUri);
                    break;
                case "github":
                    accessToken = await ExchangeGitHubCode(request.Code, request.RedirectUri);
                    break;
                default:
                    return BadRequest(new { message = "Unsupported provider" });
            }

            if (string.IsNullOrEmpty(accessToken))
            {
                return BadRequest(new { message = "Failed to exchange authorization code" });
            }

            // Now use this access token to login/register
            var loginRequest = new SocialLoginRequest
            {
                Provider = request.Provider,
                AccessToken = accessToken
            };

            return await SocialLogin(loginRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exchanging authorization code");
            return StatusCode(500, new { message = "Failed to exchange authorization code" });
        }
    }

    private async Task<GoogleUserInfo?> GetGoogleUserInfo(string accessToken)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            
            var response = await client.GetAsync("https://www.googleapis.com/oauth2/v2/userinfo");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to get Google user info: {StatusCode}", response.StatusCode);
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<GoogleUserInfo>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting Google user info");
            return null;
        }
    }

    private async Task<GitHubUserInfo?> GetGitHubUserInfo(string accessToken)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("GradLink");
            
            // Get user info
            var response = await client.GetAsync("https://api.github.com/user");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to get GitHub user info: {StatusCode}", response.StatusCode);
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var userInfo = JsonSerializer.Deserialize<GitHubUserInfo>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // If email is null, fetch from emails endpoint
            if (userInfo != null && string.IsNullOrEmpty(userInfo.Email))
            {
                var emailsResponse = await client.GetAsync("https://api.github.com/user/emails");
                if (emailsResponse.IsSuccessStatusCode)
                {
                    var emailsContent = await emailsResponse.Content.ReadAsStringAsync();
                    var emails = JsonSerializer.Deserialize<List<GitHubEmail>>(emailsContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    
                    var primaryEmail = emails?.FirstOrDefault(e => e.Primary && e.Verified);
                    if (primaryEmail != null)
                    {
                        userInfo.Email = primaryEmail.Email;
                    }
                }
            }

            return userInfo;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting GitHub user info");
            return null;
        }
    }

    private async Task<string> ExchangeGoogleCode(string code, string redirectUri)
    {
        var client = _httpClientFactory.CreateClient();
        var clientId = _configuration["OAuth:Google:ClientId"];
        var clientSecret = _configuration["OAuth:Google:ClientSecret"];

        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "code", code },
            { "client_id", clientId ?? "" },
            { "client_secret", clientSecret ?? "" },
            { "redirect_uri", redirectUri },
            { "grant_type", "authorization_code" }
        });

        var response = await client.PostAsync("https://oauth2.googleapis.com/token", content);
        if (!response.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var json = await response.Content.ReadAsStringAsync();
        var tokenData = JsonSerializer.Deserialize<JsonElement>(json);
        return tokenData.GetProperty("access_token").GetString() ?? string.Empty;
    }

    private async Task<string> ExchangeGitHubCode(string code, string redirectUri)
    {
        var client = _httpClientFactory.CreateClient();
        var clientId = _configuration["OAuth:GitHub:ClientId"];
        var clientSecret = _configuration["OAuth:GitHub:ClientSecret"];

        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "code", code },
            { "client_id", clientId ?? "" },
            { "client_secret", clientSecret ?? "" },
            { "redirect_uri", redirectUri }
        });

        var response = await client.PostAsync("https://github.com/login/oauth/access_token", content);
        if (!response.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var json = await response.Content.ReadAsStringAsync();
        var tokenData = JsonSerializer.Deserialize<JsonElement>(json);
        return tokenData.GetProperty("access_token").GetString() ?? string.Empty;
    }
}

/// <summary>
/// Request DTO for exchanging authorization code
/// </summary>
public class ExchangeCodeRequest
{
    public string Provider { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string RedirectUri { get; set; } = string.Empty;
}

