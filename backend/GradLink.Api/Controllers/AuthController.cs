using GradLink.Application.DTOs.Auth;
using GradLink.Application.DTOs.Sponsors;
using GradLink.Application.Common.Interfaces;
using GradLink.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using GradLink.Infrastructure.Identity;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Controllers;

/// <summary>
/// Authentication controller for user registration, login, and password management
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly JwtTokenService _tokenService;
    private readonly AppDbContext _context;
    private readonly ILogger<AuthController> _logger;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        JwtTokenService tokenService,
        AppDbContext context,
        ILogger<AuthController> logger,
        IEmailService emailService,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _context = context;
        _logger = logger;
        _emailService = emailService;
        _configuration = configuration;
    }

    /// <summary>
    /// Register a new user account
    /// </summary>
    /// <param name="request">Registration details including email, password, and role</param>
    /// <returns>Message indicating OTP was sent</returns>
    /// <response code="200">OTP sent successfully</response>
    /// <response code="400">Email already registered or validation error</response>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            return BadRequest(new { error = "Email is already registered" });

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            Company = request.CompanyName,
            University = request.University,
            Faculty = request.Faculty,
            Department = request.Major,
            AcademicYear = request.AcademicYear,
            GraduationYear = request.GraduationYear,
            Specialization = request.Specialization,
            ExperienceYears = request.ExperienceYears,
            JobTitle = request.JobTitle,
            CreatedAt = DateTime.UtcNow,
            EmailConfirmed = false // Email not confirmed yet
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });

        var role = request.Role ?? "Student";
        var validRoles = new[] { "Student", "Company", "Mentor", "Admin" };
        if (!validRoles.Contains(role)) role = "Student";
        
        await _userManager.AddToRoleAsync(user, role);
        await CreateProfileForRole(user, request, role);

        // Send OTP email instead of verification link
        var otpSent = await SendOtpEmailAsync(user);

        _logger.LogInformation("New {Role} registered: {Email}, OTP sent: {OtpSent}", role, request.Email, otpSent);
        
        return Ok(new { 
            message = "Account created! Please check your email for the verification code.",
            email = request.Email,
            requiresVerification = true
        });
    }

    /// <summary>
    /// Verify OTP code
    /// </summary>
    [HttpPost("verify-otp")]
    public async Task<ActionResult<AuthResponse>> VerifyOtp([FromBody] VerifyOtpRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return BadRequest(new { error = "Invalid email address" });
        }

        if (user.EmailConfirmed)
        {
            return BadRequest(new { error = "Email is already verified" });
        }

        // Find valid OTP
        var otp = await _context.OtpVerifications
            .Where(o => o.Email == request.Email && o.OtpCode == request.OtpCode && !o.IsUsed)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();

        if (otp == null)
        {
            // Track failed attempts
            var recentOtp = await _context.OtpVerifications
                .Where(o => o.Email == request.Email && !o.IsUsed)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();
            
            if (recentOtp != null)
            {
                recentOtp.AttemptCount++;
                await _context.SaveChangesAsync();
                
                if (recentOtp.AttemptCount >= 5)
                {
                    return BadRequest(new { error = "Too many failed attempts. Please request a new code." });
                }
            }
            
            return BadRequest(new { error = "Invalid verification code" });
        }

        if (otp.ExpiresAt < DateTime.UtcNow)
        {
            return BadRequest(new { error = "Verification code has expired. Please request a new one." });
        }

        // Mark OTP as used and verify user
        otp.IsUsed = true;
        user.EmailConfirmed = true;
        user.IsVerified = true;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Email verified via OTP for {Email}", user.Email);

        // Return auth response with tokens
        return await GenerateAuthResponse(user);
    }

    /// <summary>
    /// Resend OTP code
    /// </summary>
    [HttpPost("resend-otp")]
    public async Task<IActionResult> ResendOtp([FromBody] ResendVerificationRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user == null)
        {
            return Ok(new { message = "If an account exists with this email, a verification code has been sent." });
        }

        if (user.EmailConfirmed)
        {
            return BadRequest(new { error = "Email is already verified" });
        }

        // Invalidate old OTPs
        var oldOtps = await _context.OtpVerifications
            .Where(o => o.Email == request.Email && !o.IsUsed)
            .ToListAsync();
        
        foreach (var o in oldOtps)
        {
            o.IsUsed = true;
        }
        await _context.SaveChangesAsync();

        // Send new OTP
        var otpSent = await SendOtpEmailAsync(user);

        return Ok(new { 
            message = "A new verification code has been sent to your email.",
            sent = otpSent
        });
    }

    private async Task<bool> SendOtpEmailAsync(ApplicationUser user)
    {
        // Generate 6-digit OTP
        var random = new Random();
        var otpCode = random.Next(100000, 999999).ToString();
        
        // Invalidate old OTPs
        var oldOtps = await _context.OtpVerifications
            .Where(o => o.Email == user.Email && !o.IsUsed)
            .ToListAsync();
        foreach (var o in oldOtps)
        {
            o.IsUsed = true;
        }

        // Save new OTP
        var otp = new OtpVerification
        {
            UserId = user.Id,
            Email = user.Email!,
            OtpCode = otpCode,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10), // OTP valid for 10 minutes
            CreatedAt = DateTime.UtcNow
        };

        _context.OtpVerifications.Add(otp);
        await _context.SaveChangesAsync();

        try
        {
            await _emailService.SendEmailAsync(
                user.Email!,
                "🔐 Your GradLink Verification Code",
                GetOtpEmailHtml(user.FullName ?? "Friend", otpCode)
            );
            _logger.LogInformation("OTP email sent to {Email}", user.Email);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send OTP email to {Email}", user.Email);
            return false;
        }
    }

    private string GetOtpEmailHtml(string userName, string otpCode)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
</head>
<body style='margin: 0; padding: 0; font-family: -apple-system, BlinkMacSystemFont, Segoe UI, Roboto, Oxygen, Ubuntu, sans-serif; background-color: #f5f7fa;'>
    <table width='100%' cellpadding='0' cellspacing='0' style='max-width: 600px; margin: 0 auto; background-color: #ffffff;'>
        <!-- Header with Logo -->
        <tr>
            <td style='background: linear-gradient(135deg, #0c2737 0%, #1a4a5e 50%, #2d6a7a 100%); padding: 40px 30px; text-align: center;'>
                <div style='margin-bottom: 15px;'>
                    <span style='font-size: 50px;'>🎓</span>
                </div>
                <h1 style='color: #ffffff; margin: 0; font-size: 32px; font-weight: 700; letter-spacing: -0.5px;'>GradLink</h1>
                <p style='color: rgba(255,255,255,0.85); margin: 8px 0 0 0; font-size: 14px;'>Your Gateway to Success ✨</p>
            </td>
        </tr>
        
        <!-- Main Content -->
        <tr>
            <td style='padding: 50px 40px;'>
                <h2 style='color: #1a1a2e; margin: 0 0 20px 0; font-size: 26px; font-weight: 600;'>
                    Hey {userName}! 👋
                </h2>
                <p style='color: #4a5568; font-size: 16px; line-height: 1.7; margin: 0 0 30px 0;'>
                    Welcome to <strong>GradLink</strong>! We're so excited to have you join our community of ambitious students and graduates. 🚀
                </p>
                <p style='color: #4a5568; font-size: 16px; line-height: 1.7; margin: 0 0 25px 0;'>
                    Use this verification code to complete your registration:
                </p>
                
                <!-- OTP Code Box -->
                <div style='background: linear-gradient(135deg, #f8fafc 0%, #e2e8f0 100%); border: 2px dashed #0c2737; border-radius: 16px; padding: 30px; text-align: center; margin: 30px 0;'>
                    <p style='color: #64748b; font-size: 12px; text-transform: uppercase; letter-spacing: 2px; margin: 0 0 15px 0;'>Your Verification Code</p>
                    <div style='font-size: 42px; font-weight: 800; letter-spacing: 12px; color: #0c2737; font-family: Monaco, Consolas, monospace;'>
                        {otpCode}
                    </div>
                    <p style='color: #94a3b8; font-size: 13px; margin: 20px 0 0 0;'>
                        ⏱️ This code expires in <strong>10 minutes</strong>
                    </p>
                </div>

                <!-- Security Note -->
                <div style='background: #fef3c7; border-left: 4px solid #f59e0b; border-radius: 0 8px 8px 0; padding: 15px 20px; margin: 25px 0;'>
                    <p style='color: #92400e; font-size: 14px; margin: 0; display: flex; align-items: center;'>
                        🔒 <strong style='margin-left: 8px;'>Security Tip:</strong>&nbsp;Never share this code with anyone. GradLink will never ask for it.
                    </p>
                </div>

                <p style='color: #64748b; font-size: 14px; line-height: 1.6; margin: 25px 0 0 0;'>
                    If you didn't create an account with GradLink, you can safely ignore this email.
                </p>
            </td>
        </tr>
        
        <!-- Footer -->
        <tr>
            <td style='background: #f8fafc; padding: 30px 40px; text-align: center; border-top: 1px solid #e2e8f0;'>
                <p style='color: #0c2737; font-size: 18px; font-weight: 600; margin: 0 0 5px 0;'>🎓 GradLink</p>
                <p style='color: #94a3b8; font-size: 13px; margin: 0 0 15px 0;'>Connecting Students with Opportunities</p>
                <p style='color: #cbd5e1; font-size: 12px; margin: 0;'>© 2024 GradLink. All rights reserved.</p>
            </td>
        </tr>
    </table>
</body>
</html>";
    }

    private async Task CreateProfileForRole(ApplicationUser user, RegisterRequest request, string role)
    {
        try
        {
            if (role == "Mentor")
            {
                _context.MentorProfiles.Add(new MentorProfile
                {
                    UserId = user.Id,
                    ExpertiseAreas = request.Specialization, // Map specialization to expertise areas
                    IsAcceptingMentees = true,
                    MaxMentees = 10,
                    CurrentMenteesCount = 0,
                    IsVerified = false
                });
            }
            else if (role == "Company" && !string.IsNullOrEmpty(request.CompanyName))
            {
                _context.CompanyProfiles.Add(new CompanyProfile
                {
                    UserId = user.Id,
                    CompanyName = request.CompanyName,
                    Industry = request.Industry,
                    CompanySize = request.CompanySize,
                    Website = request.CompanyWebsite,
                    CompanyDescription = request.CompanyDescription,
                    HiringStatus = true
                });
            }
            else if (role == "Student")
            {
                _context.StudentProfiles.Add(new StudentProfile { UserId = user.Id });
            }
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Profile creation failed: {Error}", ex.Message);
        }
    }

    /// <summary>
    /// Login with email and password
    /// </summary>
    /// <param name="request">Login credentials</param>
    /// <returns>Authentication response with JWT token</returns>
    /// <response code="200">Login successful</response>
    /// <response code="401">Invalid credentials or email not verified</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Unauthorized(new { error = "Invalid email or password" });
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            return Unauthorized(new { error = "Invalid email or password" });
        }

        // Check if email is verified
        if (!user.EmailConfirmed)
        {
            // Send a new OTP for convenience
            await SendOtpEmailAsync(user);
            
            return Unauthorized(new { 
                error = "Please verify your email first. A new verification code has been sent.",
                requiresVerification = true,
                email = user.Email
            });
        }

        user.LastLoginAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        return await GenerateAuthResponse(user);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponse>> Refresh([FromBody] RefreshTokenRequest request)
    {
        var refreshToken = await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken && !rt.IsRevoked);

        if (refreshToken == null || refreshToken.ExpiresAt < DateTime.UtcNow)
        {
            return Unauthorized(new { error = "Invalid or expired refresh token" });
        }

        // Revoke old token
        refreshToken.IsRevoked = true;
        refreshToken.RevokedAt = DateTime.UtcNow;

        return await GenerateAuthResponse(refreshToken.User!);
    }

    /// <summary>
    /// Register as a Sponsor (Company)
    /// </summary>
    [HttpPost("register/sponsor")]
    public async Task<ActionResult<AuthResponse>> RegisterSponsor([FromBody] SponsorRegistrationRequest request)
    {
        // Check if email already exists
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return BadRequest(new { error = "Email is already registered" });
        }

        // Create the user account
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.ContactPersonName,
            Company = request.CompanyName,
            PhoneNumber = request.ContactPhone,
            Location = request.Address,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }

        // Assign Sponsor role
        await _userManager.AddToRoleAsync(user, "Sponsor");

        // Create the sponsor profile
        var sponsorProfile = new SponsorProfile
        {
            CompanyName = request.CompanyName,
            CompanyDescription = request.CompanyDescription,
            Industry = request.Industry,
            Website = request.Website,
            ContactPersonName = request.ContactPersonName,
            ContactEmail = request.Email,
            ContactPhone = request.ContactPhone,
            Address = request.Address,
            FieldsOfInterest = request.FieldsOfInterest,
            TotalBudget = request.InitialBudget,
            RemainingBudget = request.InitialBudget,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow
        };

        _context.SponsorProfiles.Add(sponsorProfile);
        await _context.SaveChangesAsync();

        _logger.LogInformation("New sponsor registered: {CompanyName} ({Email})", request.CompanyName, request.Email);

        return await GenerateAuthResponse(user);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId != null)
        {
            // Revoke all refresh tokens for this user
            var tokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && !rt.IsRevoked)
                .ToListAsync();

            foreach (var token in tokens)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        return Ok(new { message = "Logged out successfully" });
    }

    /// <summary>
    /// Request password reset
    /// </summary>
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        // Always return success to prevent email enumeration
        if (user == null)
        {
            _logger.LogWarning("Password reset requested for non-existent email: {Email}", request.Email);
            return Ok(new { message = "If an account exists with this email, a password reset link has been sent." });
        }

        // Generate reset token
        var token = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
        var resetToken = new PasswordResetToken
        {
            UserId = user.Id,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddHours(1), // Token valid for 1 hour
            CreatedAt = DateTime.UtcNow
        };

        _context.PasswordResetTokens.Add(resetToken);
        await _context.SaveChangesAsync();

        // Build the reset link
        var frontendUrl = _configuration["Cors:AllowedOrigins:0"] ?? "http://localhost:5173";
        var resetLink = $"{frontendUrl}/reset-password?token={token}";
        
        _logger.LogInformation("Password reset token generated for {Email}", request.Email);

        // Send the email
        try
        {
            await _emailService.SendPasswordResetEmailAsync(request.Email, resetLink);
            _logger.LogInformation("Password reset email sent successfully to {Email}", request.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send password reset email to {Email}", request.Email);
            // Don't fail the request - the token is still valid
        }

        return Ok(new { 
            message = "If an account exists with this email, a password reset link has been sent."
        });
    }

    /// <summary>
    /// Reset password with token
    /// </summary>
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var resetToken = await _context.PasswordResetTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Token == request.Token && !t.IsUsed);

        if (resetToken == null || resetToken.ExpiresAt < DateTime.UtcNow)
        {
            return BadRequest(new { error = "Invalid or expired reset token" });
        }

        var user = resetToken.User!;
        
        // Reset the password
        var removeResult = await _userManager.RemovePasswordAsync(user);
        if (!removeResult.Succeeded)
        {
            return BadRequest(new { error = "Failed to reset password", errors = removeResult.Errors.Select(e => e.Description) });
        }

        var addResult = await _userManager.AddPasswordAsync(user, request.NewPassword);
        if (!addResult.Succeeded)
        {
            return BadRequest(new { error = "Failed to set new password", errors = addResult.Errors.Select(e => e.Description) });
        }

        // Mark token as used
        resetToken.IsUsed = true;
        await _context.SaveChangesAsync();

        // Revoke all refresh tokens for security
        var tokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == user.Id && !rt.IsRevoked)
            .ToListAsync();

        foreach (var token in tokens)
        {
            token.IsRevoked = true;
            token.RevokedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Password reset successful for {Email}", user.Email);

        return Ok(new { message = "Password has been reset successfully. Please login with your new password." });
    }

    /// <summary>
    /// Verify reset token is valid
    /// </summary>
    [HttpGet("verify-reset-token")]
    public async Task<IActionResult> VerifyResetToken([FromQuery] string token)
    {
        var resetToken = await _context.PasswordResetTokens
            .FirstOrDefaultAsync(t => t.Token == token && !t.IsUsed);

        if (resetToken == null || resetToken.ExpiresAt < DateTime.UtcNow)
        {
            return BadRequest(new { valid = false, error = "Invalid or expired reset token" });
        }

        return Ok(new { valid = true });
    }

    private async Task<AuthResponse> GenerateAuthResponse(ApplicationUser user)
    {
        var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        // Save refresh token to database
        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        };

        _context.RefreshTokens.Add(refreshTokenEntity);
        await _context.SaveChangesAsync();

        var roles = await _userManager.GetRolesAsync(user);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                FullName = user.FullName ?? string.Empty,
                ProfilePicture = user.ProfilePicture,
                CompanyName = user.Company, // Include company name
                Roles = roles.ToList()
            }
        };
    }

    /// <summary>
    /// Make user an admin (Development only - remove in production!)
    /// </summary>
    [HttpPost("make-admin")]
    public async Task<IActionResult> MakeAdmin([FromBody] MakeAdminRequest request)
    {
        // Simple secret key for development - REMOVE IN PRODUCTION!
        if (request.SecretKey != "gradlink-dev-2024")
        {
            return Unauthorized(new { error = "Invalid secret key" });
        }

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return NotFound(new { error = "User not found" });
        }

        // Check if Admin role exists, create if not
        var roleManager = HttpContext.RequestServices.GetRequiredService<RoleManager<IdentityRole>>();
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        // Add user to Admin role
        if (!await _userManager.IsInRoleAsync(user, "Admin"))
        {
            await _userManager.AddToRoleAsync(user, "Admin");
        }

        var roles = await _userManager.GetRolesAsync(user);
        return Ok(new { 
            message = $"User {user.Email} is now an Admin",
            roles = roles
        });
    }
}

