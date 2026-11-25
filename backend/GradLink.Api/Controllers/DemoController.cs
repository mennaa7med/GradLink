using GradLink.Application.DTOs.Auth;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GradLink.Api.Controllers;

/// <summary>
/// Passwordless entry for the read-only demo. Only works when "Demo:Enabled" is true;
/// otherwise the route returns 404. The demo user has a random password nobody knows.
/// </summary>
[ApiController]
[Route("api/demo")]
public class DemoController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtTokenService _tokenService;

    public DemoController(
        IConfiguration configuration,
        UserManager<ApplicationUser> userManager,
        JwtTokenService tokenService)
    {
        _configuration = configuration;
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [HttpPost("session")]
    public async Task<ActionResult<AuthResponse>> CreateSession()
    {
        if (!_configuration.GetValue<bool>("Demo:Enabled"))
        {
            return NotFound();
        }

        var user = await DemoData.GetOrCreateDemoUserAsync(_userManager);
        if (user == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Demo is unavailable." });
        }

        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = await _tokenService.GenerateAccessTokenAsync(user);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = string.Empty, // refresh is disabled in demo mode
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                FullName = user.FullName ?? string.Empty,
                Roles = roles.ToList()
            }
        };
    }
}
