using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using GradLink.Application.Common.Interfaces;
using GradLink.Application.DTOs.MentorApplications;
using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Controllers
{
    /// <summary>
    /// Handles mentor application process including test submission and auto-grading
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Mentor Applications")]
    public class MentorApplicationsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private const decimal PASSING_SCORE = 70m;

        public MentorApplicationsController(
            AppDbContext context, 
            IEmailService emailService,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _context = context;
            _emailService = emailService;
            _userManager = userManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Get available specializations for mentor application
        /// </summary>
        [HttpGet("specializations")]
        public ActionResult<string[]> GetSpecializations()
        {
            return Ok(MentorSpecializations.All);
        }

        /// <summary>
        /// Submit a new mentor application
        /// </summary>
        [HttpPost("apply")]
        public async Task<ActionResult<ApplicationSubmittedResponse>> Apply([FromBody] CreateMentorApplicationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check for existing application with same email
            var existingApplication = await _context.MentorApplications
                .FirstOrDefaultAsync(a => a.Email.ToLower() == request.Email.ToLower());

            if (existingApplication != null)
            {
                // Check if they can retry
                if (existingApplication.Status == "Rejected" && 
                    existingApplication.RetryAllowedAt.HasValue && 
                    existingApplication.RetryAllowedAt.Value <= DateTime.UtcNow)
                {
                    // Allow retry - update existing application
                    existingApplication.FullName = request.FullName;
                    existingApplication.PhoneNumber = request.PhoneNumber;
                    existingApplication.Specialization = request.Specialization;
                    existingApplication.YearsOfExperience = request.YearsOfExperience;
                    existingApplication.LinkedInUrl = request.LinkedInUrl;
                    existingApplication.Bio = request.Bio;
                    existingApplication.CurrentPosition = request.CurrentPosition;
                    existingApplication.Company = request.Company;
                    existingApplication.Status = "Pending";
                    existingApplication.UpdatedAt = DateTime.UtcNow;
                    existingApplication.RetryAllowedAt = null;
                    
                    await _context.SaveChangesAsync();
                    
                    // Send test email
                    await SendTestEmail(existingApplication);
                    
                    return Ok(new ApplicationSubmittedResponse
                    {
                        ApplicationId = existingApplication.Id,
                        Message = "Application resubmitted successfully! Check your email for the test link.",
                        Status = "TestSent"
                    });
                }
                else if (existingApplication.Status == "Approved")
                {
                    return BadRequest(new { message = "You are already an approved mentor!" });
                }
                else if (existingApplication.Status == "Rejected" && existingApplication.RetryAllowedAt.HasValue)
                {
                    var daysLeft = (existingApplication.RetryAllowedAt.Value - DateTime.UtcNow).Days;
                    return BadRequest(new { message = $"You can retry after {daysLeft} days." });
                }
                else
                {
                    return BadRequest(new { message = "You already have a pending application." });
                }
            }

            // Create new application
            var application = new MentorApplication
            {
                FullName = request.FullName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Specialization = request.Specialization,
                YearsOfExperience = request.YearsOfExperience,
                LinkedInUrl = request.LinkedInUrl,
                Bio = request.Bio,
                CurrentPosition = request.CurrentPosition,
                Company = request.Company,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.MentorApplications.Add(application);
            await _context.SaveChangesAsync();

            // Send test email
            await SendTestEmail(application);

            return Ok(new ApplicationSubmittedResponse
            {
                ApplicationId = application.Id,
                Message = "Application submitted successfully! Check your email for the test link.",
                Status = "TestSent"
            });
        }

        /// <summary>
        /// Verify test token and get test info
        /// </summary>
        [HttpPost("verify-token")]
        public async Task<ActionResult<VerifyTestTokenResponse>> VerifyTestToken([FromBody] VerifyTestTokenRequest request)
        {
            var test = await _context.MentorTests
                .Include(t => t.Application)
                .FirstOrDefaultAsync(t => t.Token == request.Token);

            if (test == null)
            {
                return Ok(new VerifyTestTokenResponse
                {
                    IsValid = false,
                    Message = "Invalid test token."
                });
            }

            if (test.Status == "Completed")
            {
                return Ok(new VerifyTestTokenResponse
                {
                    IsValid = false,
                    Message = "This test has already been completed."
                });
            }

            if (test.ExpiresAt < DateTime.UtcNow)
            {
                return Ok(new VerifyTestTokenResponse
                {
                    IsValid = false,
                    Message = "This test link has expired."
                });
            }

            return Ok(new VerifyTestTokenResponse
            {
                IsValid = true,
                ApplicantName = test.Application.FullName,
                Specialization = test.Application.Specialization,
                TimeLimitMinutes = test.TimeLimitMinutes,
                TotalQuestions = test.TotalQuestions,
                ExpiresAt = test.ExpiresAt
            });
        }

        /// <summary>
        /// Start the mentor test
        /// </summary>
        [HttpPost("start-test")]
        public async Task<ActionResult<StartTestResponse>> StartTest([FromBody] StartTestRequest request)
        {
            var test = await _context.MentorTests
                .Include(t => t.Application)
                .FirstOrDefaultAsync(t => t.Token == request.Token);

            if (test == null)
                return BadRequest(new { message = "Invalid test token." });

            if (test.Status == "Completed")
                return BadRequest(new { message = "This test has already been completed." });

            if (test.ExpiresAt < DateTime.UtcNow)
                return BadRequest(new { message = "This test link has expired." });

            // If test was already started, return the same questions
            if (test.Status == "InProgress" && test.StartedAt.HasValue)
            {
                var existingQuestionIds = JsonSerializer.Deserialize<List<int>>(test.QuestionIds ?? "[]");
                var existingQuestions = await _context.TestQuestions
                    .Where(q => existingQuestionIds!.Contains(q.Id))
                    .Select(q => new TestQuestionDto
                    {
                        Id = q.Id,
                        Category = q.Category,
                        QuestionText = q.QuestionText,
                        OptionA = q.OptionA,
                        OptionB = q.OptionB,
                        OptionC = q.OptionC,
                        OptionD = q.OptionD,
                        Difficulty = q.Difficulty
                    })
                    .ToListAsync();

                var mustSubmitBy = test.StartedAt.Value.AddMinutes(test.TimeLimitMinutes);
                if (DateTime.UtcNow > mustSubmitBy)
                {
                    return BadRequest(new { message = "Time has expired for this test." });
                }

                return Ok(new StartTestResponse
                {
                    TestId = test.Id,
                    Questions = existingQuestions,
                    TimeLimitMinutes = test.TimeLimitMinutes,
                    StartedAt = test.StartedAt.Value,
                    MustSubmitBy = mustSubmitBy
                });
            }

            // Get questions for the specialization
            var specialization = test.Application.Specialization;
            var questions = await _context.TestQuestions
                .Where(q => q.IsActive && (q.Category == specialization || q.Category == "General"))
                .OrderBy(q => Guid.NewGuid()) // Random order
                .Take(15)
                .Select(q => new TestQuestionDto
                {
                    Id = q.Id,
                    Category = q.Category,
                    QuestionText = q.QuestionText,
                    OptionA = q.OptionA,
                    OptionB = q.OptionB,
                    OptionC = q.OptionC,
                    OptionD = q.OptionD,
                    Difficulty = q.Difficulty
                })
                .ToListAsync();

            // If not enough questions, get from any category
            if (questions.Count < 10)
            {
                var additionalQuestions = await _context.TestQuestions
                    .Where(q => q.IsActive && !questions.Select(x => x.Id).Contains(q.Id))
                    .OrderBy(q => Guid.NewGuid())
                    .Take(15 - questions.Count)
                    .Select(q => new TestQuestionDto
                    {
                        Id = q.Id,
                        Category = q.Category,
                        QuestionText = q.QuestionText,
                        OptionA = q.OptionA,
                        OptionB = q.OptionB,
                        OptionC = q.OptionC,
                        OptionD = q.OptionD,
                        Difficulty = q.Difficulty
                    })
                    .ToListAsync();

                questions.AddRange(additionalQuestions);
            }

            // Update test
            test.Status = "InProgress";
            test.StartedAt = DateTime.UtcNow;
            test.TotalQuestions = questions.Count;
            test.QuestionIds = JsonSerializer.Serialize(questions.Select(q => q.Id).ToList());

            await _context.SaveChangesAsync();

            return Ok(new StartTestResponse
            {
                TestId = test.Id,
                Questions = questions,
                TimeLimitMinutes = test.TimeLimitMinutes,
                StartedAt = test.StartedAt.Value,
                MustSubmitBy = test.StartedAt.Value.AddMinutes(test.TimeLimitMinutes)
            });
        }

        /// <summary>
        /// Submit test answers and get auto-graded result
        /// </summary>
        [HttpPost("submit-test")]
        public async Task<ActionResult<TestResultResponse>> SubmitTest([FromBody] SubmitTestRequest request)
        {
            var test = await _context.MentorTests
                .Include(t => t.Application)
                .FirstOrDefaultAsync(t => t.Token == request.Token);

            if (test == null)
                return BadRequest(new { message = "Invalid test token." });

            if (test.Status == "Completed")
                return BadRequest(new { message = "This test has already been submitted." });

            if (test.Status != "InProgress")
                return BadRequest(new { message = "Test was not started properly." });

            // Check time limit (with 1 minute grace period)
            var timeLimit = test.StartedAt!.Value.AddMinutes(test.TimeLimitMinutes + 1);
            if (DateTime.UtcNow > timeLimit)
            {
                test.Status = "Expired";
                await _context.SaveChangesAsync();
                return BadRequest(new { message = "Time has expired for this test." });
            }

            // Get correct answers
            var questionIds = JsonSerializer.Deserialize<List<int>>(test.QuestionIds ?? "[]");
            var correctAnswers = await _context.TestQuestions
                .Where(q => questionIds!.Contains(q.Id))
                .ToDictionaryAsync(q => q.Id, q => q.CorrectAnswer);

            // Grade the test
            int correctCount = 0;
            foreach (var answer in request.Answers)
            {
                if (correctAnswers.TryGetValue(answer.QuestionId, out var correct) && 
                    answer.Answer.ToUpper() == correct.ToUpper())
                {
                    correctCount++;
                }
            }

            decimal score = test.TotalQuestions > 0 
                ? Math.Round((decimal)correctCount / test.TotalQuestions * 100, 2) 
                : 0;

            // Update test
            test.Status = "Completed";
            test.CompletedAt = DateTime.UtcNow;
            test.CorrectAnswers = correctCount;
            test.Score = score;
            test.SubmittedAnswers = JsonSerializer.Serialize(request.Answers);

            // Update application
            var application = test.Application;
            application.TestAttempts++;
            application.FinalScore = score;
            application.UpdatedAt = DateTime.UtcNow;

            bool passed = score >= PASSING_SCORE;
            string message;
            DateTime? retryAllowedAt = null;

            if (passed)
            {
                application.Status = "Approved";
                message = $"Congratulations! You passed with {score}%! You are now a GradLink Mentor.";
                
                // Create user account or update existing and get password
                var generatedPassword = await CreateOrUpdateMentorAccount(application);
                
                // Send approval email with password
                await SendApprovalEmail(application, score, generatedPassword);
            }
            else
            {
                application.Status = "Rejected";
                
                if (score < 50)
                {
                    retryAllowedAt = DateTime.UtcNow.AddDays(14);
                    message = $"You scored {score}%. You need at least 70% to pass. You can retry after 14 days.";
                }
                else
                {
                    retryAllowedAt = DateTime.UtcNow.AddDays(7);
                    message = $"You scored {score}%. You need at least 70% to pass. You can retry after 7 days.";
                }
                
                application.RetryAllowedAt = retryAllowedAt;
                
                // Send rejection email
                await SendRejectionEmail(application, score, retryAllowedAt.Value);
            }

            await _context.SaveChangesAsync();

            return Ok(new TestResultResponse
            {
                TestId = test.Id,
                TotalQuestions = test.TotalQuestions,
                CorrectAnswers = correctCount,
                Score = score,
                Passed = passed,
                Message = message,
                Status = passed ? "Approved" : "Rejected",
                RetryAllowedAt = retryAllowedAt
            });
        }

        /// <summary>
        /// Check application status by email
        /// </summary>
        [HttpGet("status/{email}")]
        public async Task<ActionResult<MentorApplicationDto>> GetApplicationStatus(string email)
        {
            var application = await _context.MentorApplications
                .FirstOrDefaultAsync(a => a.Email.ToLower() == email.ToLower());

            if (application == null)
                return NotFound(new { message = "No application found with this email." });

            return Ok(new MentorApplicationDto
            {
                Id = application.Id,
                FullName = application.FullName,
                Email = application.Email,
                PhoneNumber = application.PhoneNumber,
                Specialization = application.Specialization,
                YearsOfExperience = application.YearsOfExperience,
                LinkedInUrl = application.LinkedInUrl,
                Bio = application.Bio,
                CurrentPosition = application.CurrentPosition,
                Company = application.Company,
                Status = application.Status,
                CreatedAt = application.CreatedAt,
                RetryAllowedAt = application.RetryAllowedAt,
                TestAttempts = application.TestAttempts,
                FinalScore = application.FinalScore
            });
        }

        /// <summary>
        /// Admin: Get all applications
        /// </summary>
        [HttpGet("admin/all")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<MentorApplicationDto>>> GetAllApplications(
            [FromQuery] string? status = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var query = _context.MentorApplications.AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(a => a.Status == status);

            var total = await query.CountAsync();
            var applications = await query
                .OrderByDescending(a => a.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new MentorApplicationDto
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    Email = a.Email,
                    PhoneNumber = a.PhoneNumber,
                    Specialization = a.Specialization,
                    YearsOfExperience = a.YearsOfExperience,
                    LinkedInUrl = a.LinkedInUrl,
                    Bio = a.Bio,
                    CurrentPosition = a.CurrentPosition,
                    Company = a.Company,
                    Status = a.Status,
                    CreatedAt = a.CreatedAt,
                    RetryAllowedAt = a.RetryAllowedAt,
                    TestAttempts = a.TestAttempts,
                    FinalScore = a.FinalScore
                })
                .ToListAsync();

            return Ok(new { data = applications, total, page, pageSize });
        }

        // ==================== Private Methods ====================

        private async Task SendTestEmail(MentorApplication application)
        {
            // Generate unique token
            var token = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N").Substring(0, 8);

            // Create test record
            var test = new MentorTest
            {
                ApplicationId = application.Id,
                Token = token,
                Status = "Pending",
                ExpiresAt = DateTime.UtcNow.AddHours(48),
                TimeLimitMinutes = 20,
                TotalQuestions = 15,
                CreatedAt = DateTime.UtcNow
            };

            _context.MentorTests.Add(test);
            
            // Update application status
            application.Status = "TestSent";
            application.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();

            // Send email
            var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:5173";
            var testUrl = $"{frontendUrl}/mentor-test?token={token}";

            var emailBody = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
</head>
<body style='margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f4f4f4;'>
    <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
        <div style='background: linear-gradient(135deg, #0c2737 0%, #1a4a5e 100%); padding: 30px; text-align: center; border-radius: 10px 10px 0 0;'>
            <h1 style='color: #FFCB66; margin: 0; font-size: 28px;'>🎓 GradLink</h1>
            <p style='color: #ffffff; margin: 10px 0 0 0; font-size: 14px;'>Mentor Application Test</p>
        </div>
        
        <div style='background-color: #ffffff; padding: 30px; border-radius: 0 0 10px 10px;'>
            <h2 style='color: #0c2737; margin-top: 0;'>Hello {application.FullName}! 👋</h2>
            
            <p style='color: #555; line-height: 1.6;'>
                Thank you for applying to become a <strong>GradLink Mentor</strong>! 
                We're excited to have someone with your expertise in <strong>{application.Specialization}</strong> interested in joining our mentorship program.
            </p>
            
            <div style='background-color: #f8f9fa; padding: 20px; border-radius: 8px; margin: 20px 0;'>
                <h3 style='color: #0c2737; margin-top: 0;'>📝 Test Information</h3>
                <ul style='color: #555; line-height: 1.8;'>
                    <li><strong>Duration:</strong> 20 minutes</li>
                    <li><strong>Questions:</strong> 15 multiple choice</li>
                    <li><strong>Passing Score:</strong> 70%</li>
                    <li><strong>Link Valid For:</strong> 48 hours</li>
                </ul>
            </div>
            
            <div style='text-align: center; margin: 30px 0;'>
                <a href='{testUrl}' style='display: inline-block; background: linear-gradient(135deg, #FFCB66 0%, #f0ad4e 100%); color: #0c2737; padding: 15px 40px; text-decoration: none; border-radius: 8px; font-weight: bold; font-size: 16px;'>
                    Start Your Test 🚀
                </a>
            </div>
            
            <div style='background-color: #fff3cd; padding: 15px; border-radius: 8px; margin: 20px 0;'>
                <p style='color: #856404; margin: 0; font-size: 14px;'>
                    ⚠️ <strong>Important:</strong> Make sure you have a stable internet connection and 20 uninterrupted minutes before starting the test. Once started, the timer cannot be paused.
                </p>
            </div>
            
            <p style='color: #555; line-height: 1.6;'>
                Good luck! We believe in your potential to make a difference in students' lives. 💪
            </p>
            
            <hr style='border: none; border-top: 1px solid #eee; margin: 30px 0;'>
            
            <p style='color: #999; font-size: 12px; text-align: center;'>
                If you didn't apply to become a mentor, please ignore this email.<br>
                © 2024 GradLink. All rights reserved.
            </p>
        </div>
    </div>
</body>
</html>";

            await _emailService.SendEmailAsync(
                application.Email,
                "🎓 GradLink Mentor Test - Your Application",
                emailBody
            );
        }

        private async Task SendApprovalEmail(MentorApplication application, decimal score, string? password)
        {
            var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:5173";
            var loginUrl = $"{frontendUrl}/signin";

            var passwordSection = password != null 
                ? $@"
                <p style='color: #555;'><strong>Password:</strong> <code style='background: #e9ecef; padding: 5px 10px; border-radius: 4px; font-size: 16px;'>{password}</code></p>
                <p style='color: #dc3545; font-size: 12px;'>⚠️ Please change your password after first login for security.</p>"
                : "<p style='color: #555;'><strong>Password:</strong> Use your existing account password</p>";

            var emailBody = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
</head>
<body style='margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f4f4f4;'>
    <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
        <div style='background: linear-gradient(135deg, #28a745 0%, #20c997 100%); padding: 30px; text-align: center; border-radius: 10px 10px 0 0;'>
            <h1 style='color: #ffffff; margin: 0; font-size: 28px;'>🎉 Congratulations!</h1>
        </div>
        
        <div style='background-color: #ffffff; padding: 30px; border-radius: 0 0 10px 10px;'>
            <h2 style='color: #28a745; margin-top: 0;'>Welcome to GradLink Mentors, {application.FullName}!</h2>
            
            <p style='color: #555; line-height: 1.6;'>
                You have successfully passed the mentor assessment test with a score of <strong>{score}%</strong>! 
            </p>
            
            <div style='background-color: #d4edda; padding: 20px; border-radius: 8px; margin: 20px 0; text-align: center;'>
                <p style='color: #155724; margin: 0; font-size: 18px;'>
                    🏆 Your Score: <strong>{score}%</strong>
                </p>
            </div>
            
            <p style='color: #555; line-height: 1.6;'>
                You can now log in to your Mentor Dashboard and start making a difference in students' lives!
            </p>
            
            <div style='background-color: #f8f9fa; padding: 20px; border-radius: 8px; margin: 20px 0;'>
                <h3 style='color: #0c2737; margin-top: 0;'>🔐 Your Login Credentials</h3>
                <p style='color: #555;'><strong>Email:</strong> {application.Email}</p>
                {passwordSection}
            </div>
            
            <div style='text-align: center; margin: 30px 0;'>
                <a href='{loginUrl}' style='display: inline-block; background: linear-gradient(135deg, #0c2737 0%, #1a4a5e 100%); color: #FFCB66; padding: 15px 40px; text-decoration: none; border-radius: 8px; font-weight: bold;'>
                    Go to Mentor Dashboard →
                </a>
            </div>
            
            <hr style='border: none; border-top: 1px solid #eee; margin: 30px 0;'>
            
            <p style='color: #999; font-size: 12px; text-align: center;'>
                © 2024 GradLink. All rights reserved.
            </p>
        </div>
    </div>
</body>
</html>";

            await _emailService.SendEmailAsync(
                application.Email,
                "🎉 Welcome to GradLink Mentors!",
                emailBody
            );
        }

        private async Task SendRejectionEmail(MentorApplication application, decimal score, DateTime retryDate)
        {
            var emailBody = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
</head>
<body style='margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f4f4f4;'>
    <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
        <div style='background: linear-gradient(135deg, #0c2737 0%, #1a4a5e 100%); padding: 30px; text-align: center; border-radius: 10px 10px 0 0;'>
            <h1 style='color: #FFCB66; margin: 0; font-size: 28px;'>🎓 GradLink</h1>
        </div>
        
        <div style='background-color: #ffffff; padding: 30px; border-radius: 0 0 10px 10px;'>
            <h2 style='color: #0c2737; margin-top: 0;'>Hello {application.FullName},</h2>
            
            <p style='color: #555; line-height: 1.6;'>
                Thank you for taking the mentor assessment test. Unfortunately, your score of <strong>{score}%</strong> 
                did not meet the minimum passing requirement of <strong>70%</strong>.
            </p>
            
            <div style='background-color: #fff3cd; padding: 20px; border-radius: 8px; margin: 20px 0; text-align: center;'>
                <p style='color: #856404; margin: 0; font-size: 18px;'>
                    📊 Your Score: <strong>{score}%</strong> (Required: 70%)
                </p>
            </div>
            
            <p style='color: #555; line-height: 1.6;'>
                Don't be discouraged! You can reapply and take the test again after:
            </p>
            
            <div style='background-color: #e7f3ff; padding: 20px; border-radius: 8px; margin: 20px 0; text-align: center;'>
                <p style='color: #0066cc; margin: 0; font-size: 16px;'>
                    📅 <strong>{retryDate:MMMM dd, yyyy}</strong>
                </p>
            </div>
            
            <p style='color: #555; line-height: 1.6;'>
                We encourage you to review your knowledge in <strong>{application.Specialization}</strong> and try again. 
                We believe in your potential! 💪
            </p>
            
            <hr style='border: none; border-top: 1px solid #eee; margin: 30px 0;'>
            
            <p style='color: #999; font-size: 12px; text-align: center;'>
                © 2024 GradLink. All rights reserved.
            </p>
        </div>
    </div>
</body>
</html>";

            await _emailService.SendEmailAsync(
                application.Email,
                "GradLink Mentor Test Results",
                emailBody
            );
        }

        private async Task<string?> CreateOrUpdateMentorAccount(MentorApplication application)
        {
            // Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(application.Email);
            
            if (existingUser != null)
            {
                // Update existing user role to Mentor
                existingUser.Role = "Mentor";
                existingUser.FullName = application.FullName;
                existingUser.PhoneNumber = application.PhoneNumber;
                existingUser.Specialization = application.Specialization;
                existingUser.Bio = application.Bio;
                existingUser.JobTitle = application.CurrentPosition;
                existingUser.Company = application.Company;
                existingUser.LinkedInUrl = application.LinkedInUrl;
                existingUser.ExperienceYears = application.YearsOfExperience;
                existingUser.UpdatedAt = DateTime.UtcNow;
                
                await _userManager.UpdateAsync(existingUser);
                
                // Update roles
                var currentRoles = await _userManager.GetRolesAsync(existingUser);
                if (!currentRoles.Contains("Mentor"))
                {
                    await _userManager.AddToRoleAsync(existingUser, "Mentor");
                }
                
                application.UserId = existingUser.Id;
                return null; // User already has a password
            }
            else
            {
                // Create new mentor user
                var newUser = new ApplicationUser
                {
                    UserName = application.Email,
                    Email = application.Email,
                    FullName = application.FullName,
                    PhoneNumber = application.PhoneNumber,
                    Role = "Mentor",
                    Specialization = application.Specialization,
                    Bio = application.Bio,
                    JobTitle = application.CurrentPosition,
                    Company = application.Company,
                    LinkedInUrl = application.LinkedInUrl,
                    ExperienceYears = application.YearsOfExperience,
                    EmailConfirmed = true,
                    AccountStatus = "Active",
                    CreatedAt = DateTime.UtcNow
                };

                // Generate a secure password
                var generatedPassword = GenerateSecurePassword();
                var result = await _userManager.CreateAsync(newUser, generatedPassword);
                
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, "Mentor");
                    application.UserId = newUser.Id;
                    return generatedPassword; // Return the password to send via email
                }
                
                return null;
            }
        }

        private string GenerateSecurePassword()
        {
            // Generate a secure random password
            var random = new Random();
            var uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var lowercase = "abcdefghijklmnopqrstuvwxyz";
            var digits = "0123456789";
            var special = "@#$%&*!";
            
            var password = new char[12];
            password[0] = uppercase[random.Next(uppercase.Length)];
            password[1] = lowercase[random.Next(lowercase.Length)];
            password[2] = digits[random.Next(digits.Length)];
            password[3] = special[random.Next(special.Length)];
            
            var allChars = uppercase + lowercase + digits + special;
            for (int i = 4; i < 12; i++)
            {
                password[i] = allChars[random.Next(allChars.Length)];
            }
            
            // Shuffle the password
            return new string(password.OrderBy(x => random.Next()).ToArray());
        }
    }
}

