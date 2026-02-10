using System;
using System.ComponentModel.DataAnnotations;

namespace GradLink.Application.DTOs.MentorApplications
{
    /// <summary>
    /// DTO for mentor application data
    /// </summary>
    public class MentorApplicationDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Specialization { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? Bio { get; set; }
        public string? CurrentPosition { get; set; }
        public string? Company { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? RetryAllowedAt { get; set; }
        public int TestAttempts { get; set; }
        public decimal? FinalScore { get; set; }
    }

    /// <summary>
    /// Request to create a new mentor application
    /// </summary>
    public class CreateMentorApplicationRequest
    {
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Specialization is required")]
        [StringLength(100, ErrorMessage = "Specialization cannot exceed 100 characters")]
        public string Specialization { get; set; } = string.Empty;

        [Required(ErrorMessage = "Years of experience is required")]
        [Range(0, 50, ErrorMessage = "Years of experience must be between 0 and 50")]
        public int YearsOfExperience { get; set; }

        [Url(ErrorMessage = "Invalid LinkedIn URL")]
        public string? LinkedInUrl { get; set; }

        [StringLength(2000, ErrorMessage = "Bio cannot exceed 2000 characters")]
        public string? Bio { get; set; }

        [StringLength(500, ErrorMessage = "Current position cannot exceed 500 characters")]
        public string? CurrentPosition { get; set; }

        [StringLength(500, ErrorMessage = "Company name cannot exceed 500 characters")]
        public string? Company { get; set; }
    }

    /// <summary>
    /// Response after submitting application
    /// </summary>
    public class ApplicationSubmittedResponse
    {
        public int ApplicationId { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO for test information
    /// </summary>
    public class MentorTestDto
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int TimeLimitMinutes { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public decimal? Score { get; set; }
    }

    /// <summary>
    /// DTO for test question (without correct answer for client)
    /// </summary>
    public class TestQuestionDto
    {
        public int Id { get; set; }
        public string Category { get; set; } = string.Empty;
        public string QuestionText { get; set; } = string.Empty;
        public string OptionA { get; set; } = string.Empty;
        public string OptionB { get; set; } = string.Empty;
        public string OptionC { get; set; } = string.Empty;
        public string OptionD { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request to start a test
    /// </summary>
    public class StartTestRequest
    {
        [Required]
        public string Token { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response when starting a test
    /// </summary>
    public class StartTestResponse
    {
        public int TestId { get; set; }
        public List<TestQuestionDto> Questions { get; set; } = new();
        public int TimeLimitMinutes { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime MustSubmitBy { get; set; }
    }

    /// <summary>
    /// Single answer submission
    /// </summary>
    public class QuestionAnswer
    {
        public int QuestionId { get; set; }
        public string Answer { get; set; } = string.Empty; // A, B, C, or D
    }

    /// <summary>
    /// Request to submit test answers
    /// </summary>
    public class SubmitTestRequest
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        public List<QuestionAnswer> Answers { get; set; } = new();
    }

    /// <summary>
    /// Response after submitting test
    /// </summary>
    public class TestResultResponse
    {
        public int TestId { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public decimal Score { get; set; }
        public bool Passed { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? RetryAllowedAt { get; set; }
    }

    /// <summary>
    /// Request to verify test token
    /// </summary>
    public class VerifyTestTokenRequest
    {
        [Required]
        public string Token { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response for token verification
    /// </summary>
    public class VerifyTestTokenResponse
    {
        public bool IsValid { get; set; }
        public string? ApplicantName { get; set; }
        public string? Specialization { get; set; }
        public int TimeLimitMinutes { get; set; }
        public int TotalQuestions { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public string? Message { get; set; }
    }

    /// <summary>
    /// Available specializations for mentor application
    /// </summary>
    public static class MentorSpecializations
    {
        public static readonly string[] All = new[]
        {
            "Software Engineering",
            "Data Science",
            "Machine Learning",
            "Web Development",
            "Mobile Development",
            "UI/UX Design",
            "DevOps",
            "Cybersecurity",
            "Cloud Computing",
            "Project Management",
            "Product Management",
            "Business Analysis",
            "Digital Marketing",
            "Other"
        };
    }
}















