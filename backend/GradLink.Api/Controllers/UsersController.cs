using GradLink.Application.Common.Interfaces;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IFileStorage _fileStorage;

    public UsersController(
        AppDbContext context,
        UserManager<ApplicationUser> userManager,
        IFileStorage fileStorage)
    {
        _context = context;
        _userManager = userManager;
        _fileStorage = fileStorage;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        var roles = await _userManager.GetRolesAsync(user);

        return Ok(new
        {
            user.Id,
            user.Email,
            user.FullName,
            user.Bio,
            user.ProfilePicture,
            user.Major,
            user.GraduationYear,
            user.Company,
            user.Location,
            Roles = roles,
            user.CreatedAt
        });
    }

    /// <summary>
    /// Get public profile of a user
    /// </summary>
    [AllowAnonymous]
    [HttpGet("{userId}/public")]
    public async Task<ActionResult> GetPublicProfile(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound(new { error = "User not found" });
        }

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Student";

        // Count user's projects and applications
        var projectsCount = await _context.Projects
            .CountAsync(p => p.TeamMembers.Any(tm => tm.UserId == userId));
        
        var applicationsCount = await _context.JobApplications
            .CountAsync(a => a.ApplicantId == userId);

        var completedProjectsCount = await _context.Projects
            .CountAsync(p => p.TeamMembers.Any(tm => tm.UserId == userId) && p.Status == "Completed");

        return Ok(new
        {
            user.Id,
            user.FullName,
            ProfilePicture = user.ProfilePicture,
            user.Bio,
            user.Location,
            Role = role,
            user.University,
            user.Faculty,
            Major = user.Major ?? user.Department,
            user.GraduationYear,
            Company = user.Company,
            Skills = user.Skills,
            LinkedInUrl = user.LinkedInUrl,
            GitHubUrl = user.GitHubUrl,
            WebsiteUrl = user.WebsiteUrl,
            CreatedAt = user.CreatedAt,
            ProjectsCount = projectsCount,
            ApplicationsCount = applicationsCount,
            CompletedProjectsCount = completedProjectsCount
        });
    }

    [HttpGet("me")]
    public async Task<ActionResult> GetCurrentUser()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        var roles = await _userManager.GetRolesAsync(user);

        return Ok(new
        {
            user.Id,
            user.Email,
            user.FullName,
            user.Bio,
            user.ProfilePicture,
            user.Major,
            user.GraduationYear,
            user.Company,
            user.Location,
            user.PhoneNumber,
            Roles = roles,
            user.CreatedAt,
            user.LastLoginAt
        });
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        if (request.FullName != null) user.FullName = request.FullName;
        if (request.Bio != null) user.Bio = request.Bio;
        if (request.Major != null) user.Major = request.Major;
        if (request.GraduationYear.HasValue) user.GraduationYear = request.GraduationYear;
        if (request.Company != null) user.Company = request.Company;
        if (request.Location != null) user.Location = request.Location;
        if (request.PhoneNumber != null) user.PhoneNumber = request.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }

        return NoContent();
    }

    [HttpPost("me/avatar")]
    public async Task<ActionResult> UploadAvatar(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { error = "No file uploaded" });
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(fileExtension))
        {
            return BadRequest(new { error = "Invalid file type. Only images are allowed." });
        }

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        // Delete old avatar if exists
        if (!string.IsNullOrEmpty(user.ProfilePicture))
        {
            try
            {
                await _fileStorage.DeleteFileAsync(user.ProfilePicture);
            }
            catch { /* Ignore errors */ }
        }

        // Save new avatar
        using var stream = file.OpenReadStream();
        var filePath = await _fileStorage.SaveFileAsync(stream, file.FileName, "avatars");

        user.ProfilePicture = filePath;
        await _userManager.UpdateAsync(user);

        return Ok(new { profilePicture = filePath, profilePictureUrl = filePath });
    }

    /// <summary>
    /// Upload profile picture (alias for avatar)
    /// </summary>
    [HttpPost("profile-picture")]
    public async Task<ActionResult> UploadProfilePicture(IFormFile file)
    {
        return await UploadAvatar(file);
    }

    /// <summary>
    /// Delete profile picture
    /// </summary>
    [HttpDelete("profile-picture")]
    public async Task<ActionResult> DeleteProfilePicture()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        if (!string.IsNullOrEmpty(user.ProfilePicture))
        {
            try
            {
                await _fileStorage.DeleteFileAsync(user.ProfilePicture);
            }
            catch { /* Ignore errors */ }
        }

        user.ProfilePicture = null;
        await _userManager.UpdateAsync(user);

        return NoContent();
    }
}

public class UpdateProfileRequest
{
    public string? FullName { get; set; }
    public string? Bio { get; set; }
    public string? Major { get; set; }
    public int? GraduationYear { get; set; }
    public string? Company { get; set; }
    public string? Location { get; set; }
    public string? PhoneNumber { get; set; }
}

