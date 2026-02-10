using System.ComponentModel.DataAnnotations;

namespace GradLink.Application.DTOs.Auth;

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [MinLength(6)]
    public required string Password { get; set; }

    [Required]
    public required string FullName { get; set; }

    public string? PhoneNumber { get; set; }
    
    public string? Role { get; set; } // "Student", "Company", "Mentor"
    
    // ============================================
    // COMPANY FIELDS
    // ============================================
    public string? CompanyName { get; set; }
    public string? Industry { get; set; }
    public string? CompanyWebsite { get; set; }
    public string? CompanySize { get; set; }
    public string? CompanyDescription { get; set; }

    // ============================================
    // STUDENT FIELDS
    // ============================================
    public string? University { get; set; }
    public string? Faculty { get; set; }
    public string? Major { get; set; }
    public string? AcademicYear { get; set; }
    public int? GraduationYear { get; set; }

    // ============================================
    // MENTOR FIELDS
    // ============================================
    public string? Specialization { get; set; }
    public int? ExperienceYears { get; set; }
    public string? JobTitle { get; set; }
}

