using Microsoft.AspNetCore.Identity;

namespace GradLink.Domain.Entities;

/// <summary>
/// ApplicationUser - Core User table for the GradLink platform
/// Supports: Students, Graduates, Companies, Sponsors, Mentors, Admins
/// Inherits from IdentityUser which provides: Id, UserName, Email, PasswordHash, PhoneNumber, etc.
/// </summary>
public class ApplicationUser : IdentityUser
{
    // ============================================
    // A) CORE AUTHENTICATION DATA
    // ============================================
    // Note: Id, UserName, Email, PasswordHash, PhoneNumber, EmailConfirmed, PhoneNumberConfirmed
    // are inherited from IdentityUser

    /// <summary>
    /// User role: Student, Graduate, Company, Sponsor, Mentor, Admin
    /// </summary>
    public string Role { get; set; } = "Student";

    /// <summary>
    /// Account status: Active, Suspended, Deactivated, Pending
    /// </summary>
    public string AccountStatus { get; set; } = "Active";

    /// <summary>
    /// Whether the user account is verified (email/phone verified)
    /// </summary>
    public bool IsVerified { get; set; } = false;

    /// <summary>
    /// Whether the profile is publicly visible
    /// </summary>
    public bool IsProfilePublic { get; set; } = true;

    // ============================================
    // B) PERSONAL PROFILE DATA
    // ============================================

    /// <summary>
    /// Full name of the user
    /// Used by: All user types
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// Short biography/description
    /// Used by: All user types
    /// </summary>
    public string? Bio { get; set; }

    /// <summary>
    /// Profile picture URL or path
    /// Used by: All user types
    /// </summary>
    public string? ProfilePicture { get; set; }

    /// <summary>
    /// Cover/banner image URL
    /// Used by: All user types (optional)
    /// </summary>
    public string? CoverImage { get; set; }

    /// <summary>
    /// Date of birth
    /// Used by: Students, Graduates, Mentors
    /// </summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// Gender: Male, Female, Other, PreferNotToSay
    /// Used by: Students, Graduates
    /// </summary>
    public string? Gender { get; set; }

    /// <summary>
    /// Nationality
    /// Used by: Students, Graduates
    /// </summary>
    public string? Nationality { get; set; }

    // ============================================
    // C) EDUCATION & ACADEMIC DATA (Students/Graduates)
    // ============================================

    /// <summary>
    /// University/Institution name
    /// Used by: Students, Graduates
    /// </summary>
    public string? University { get; set; }

    /// <summary>
    /// Faculty/College name
    /// Used by: Students, Graduates
    /// </summary>
    public string? Faculty { get; set; }

    /// <summary>
    /// Department name
    /// Used by: Students, Graduates
    /// </summary>
    public string? Department { get; set; }

    /// <summary>
    /// Major/Field of study
    /// Used by: Students, Graduates
    /// </summary>
    public string? Major { get; set; }

    /// <summary>
    /// Academic year (1st, 2nd, 3rd, 4th, 5th)
    /// Used by: Students
    /// </summary>
    public string? AcademicYear { get; set; }

    /// <summary>
    /// Expected or actual graduation year
    /// Used by: Students, Graduates
    /// </summary>
    public int? GraduationYear { get; set; }

    /// <summary>
    /// GPA (Grade Point Average)
    /// Used by: Students, Graduates
    /// </summary>
    public decimal? GPA { get; set; }

    /// <summary>
    /// Student ID number
    /// Used by: Students
    /// </summary>
    public string? StudentId { get; set; }

    // ============================================
    // D) PROFESSIONAL DATA (All user types)
    // ============================================

    /// <summary>
    /// Current job title or position
    /// Used by: Graduates, Mentors, Company representatives
    /// </summary>
    public string? JobTitle { get; set; }

    /// <summary>
    /// Company/Organization name (where user works)
    /// Used by: Graduates, Mentors
    /// </summary>
    public string? Company { get; set; }

    /// <summary>
    /// Industry/Field of work
    /// Used by: All professional users
    /// </summary>
    public string? Industry { get; set; }

    /// <summary>
    /// Years of experience
    /// Used by: Graduates, Mentors
    /// </summary>
    public int? ExperienceYears { get; set; }

    /// <summary>
    /// Skills (comma-separated or JSON array)
    /// Used by: Students, Graduates, Mentors
    /// </summary>
    public string? Skills { get; set; }

    /// <summary>
    /// Specialization/Expertise area
    /// Used by: Mentors, Graduates
    /// </summary>
    public string? Specialization { get; set; }

    /// <summary>
    /// Certifications (JSON array)
    /// Used by: All user types
    /// </summary>
    public string? Certifications { get; set; }

    /// <summary>
    /// Languages spoken (comma-separated)
    /// Used by: All user types
    /// </summary>
    public string? Languages { get; set; }

    // ============================================
    // E) COMPANY/SPONSOR DATA
    // ============================================

    /// <summary>
    /// Company name (for Company/Sponsor accounts)
    /// Used by: Companies, Sponsors
    /// </summary>
    public string? CompanyName { get; set; }

    /// <summary>
    /// Company website
    /// Used by: Companies, Sponsors
    /// </summary>
    public string? CompanyWebsite { get; set; }

    /// <summary>
    /// Company logo URL
    /// Used by: Companies, Sponsors
    /// </summary>
    public string? CompanyLogo { get; set; }

    /// <summary>
    /// Company size: Small, Medium, Large, Enterprise
    /// Used by: Companies, Sponsors
    /// </summary>
    public string? CompanySize { get; set; }

    /// <summary>
    /// Company description
    /// Used by: Companies, Sponsors
    /// </summary>
    public string? CompanyDescription { get; set; }

    // ============================================
    // F) CONTACT & SOCIAL LINKS
    // ============================================

    /// <summary>
    /// Location/City
    /// Used by: All user types
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Full address
    /// Used by: Companies, Sponsors
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Country
    /// Used by: All user types
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// WhatsApp number
    /// Used by: All user types
    /// </summary>
    public string? WhatsApp { get; set; }

    /// <summary>
    /// LinkedIn profile URL
    /// Used by: All user types
    /// </summary>
    public string? LinkedInUrl { get; set; }

    /// <summary>
    /// GitHub profile URL
    /// Used by: Students, Graduates, Mentors
    /// </summary>
    public string? GitHubUrl { get; set; }

    /// <summary>
    /// Twitter/X profile URL
    /// Used by: All user types
    /// </summary>
    public string? TwitterUrl { get; set; }

    /// <summary>
    /// Facebook profile URL
    /// Used by: All user types
    /// </summary>
    public string? FacebookUrl { get; set; }

    /// <summary>
    /// Personal website/Portfolio URL
    /// Used by: Students, Graduates, Mentors
    /// </summary>
    public string? WebsiteUrl { get; set; }

    // ============================================
    // G) ACTIVITY & STATISTICS
    // ============================================

    /// <summary>
    /// Number of projects created/participated
    /// Used by: Students, Graduates
    /// </summary>
    public int ProjectsCount { get; set; } = 0;

    /// <summary>
    /// Number of completed projects
    /// Used by: Students, Graduates
    /// </summary>
    public int CompletedProjectsCount { get; set; } = 0;

    /// <summary>
    /// Number of jobs posted
    /// Used by: Companies
    /// </summary>
    public int JobsPostedCount { get; set; } = 0;

    /// <summary>
    /// Number of job applications
    /// Used by: Students, Graduates
    /// </summary>
    public int ApplicationsCount { get; set; } = 0;

    /// <summary>
    /// Profile view count
    /// Used by: All user types
    /// </summary>
    public int ProfileViewsCount { get; set; } = 0;

    /// <summary>
    /// Profile completion percentage (0-100)
    /// Used by: All user types
    /// </summary>
    public int ProfileCompletionPercentage { get; set; } = 0;

    // ============================================
    // H) PREMIUM & SUBSCRIPTION
    // ============================================

    /// <summary>
    /// Whether the user has a premium subscription
    /// </summary>
    public bool IsPremium { get; set; } = false;

    /// <summary>
    /// Premium plan type: Free, Basic, Pro, Enterprise
    /// </summary>
    public string PlanType { get; set; } = "Free";

    /// <summary>
    /// Premium subscription start date
    /// </summary>
    public DateTime? PremiumStartDate { get; set; }

    /// <summary>
    /// Premium subscription end date
    /// </summary>
    public DateTime? PremiumEndDate { get; set; }

    // ============================================
    // I) SYSTEM & TRACKING
    // ============================================

    /// <summary>
    /// Account creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Last profile update timestamp
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Last login timestamp
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// Last activity timestamp
    /// </summary>
    public DateTime? LastActivityAt { get; set; }

    /// <summary>
    /// Whether the user is currently online
    /// </summary>
    public bool IsOnline { get; set; } = false;

    /// <summary>
    /// IP address of last login (for security)
    /// </summary>
    public string? LastLoginIp { get; set; }

    /// <summary>
    /// Device info of last login
    /// </summary>
    public string? LastLoginDevice { get; set; }

    /// <summary>
    /// User preferences (JSON)
    /// </summary>
    public string? Preferences { get; set; }

    /// <summary>
    /// Notification settings (JSON)
    /// </summary>
    public string? NotificationSettings { get; set; }

    /// <summary>
    /// Reason for account deactivation/suspension
    /// </summary>
    public string? DeactivationReason { get; set; }

    // ============================================
    // NAVIGATION PROPERTIES
    // ============================================

    /// <summary>
    /// Projects owned/created by this user
    /// </summary>
    public ICollection<Project> Projects { get; set; } = new List<Project>();

    /// <summary>
    /// Resumes uploaded by this user
    /// </summary>
    public ICollection<Resume> Resumes { get; set; } = new List<Resume>();

    /// <summary>
    /// Jobs posted by this user (Company/Sponsor)
    /// </summary>
    public ICollection<JobPosting> JobPostings { get; set; } = new List<JobPosting>();

    /// <summary>
    /// Refresh tokens for authentication
    /// </summary>
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    /// <summary>
    /// Conversations where user is participant 1
    /// </summary>
    public ICollection<Conversation> ConversationsAsUser1 { get; set; } = new List<Conversation>();

    /// <summary>
    /// Conversations where user is participant 2
    /// </summary>
    public ICollection<Conversation> ConversationsAsUser2 { get; set; } = new List<Conversation>();

    /// <summary>
    /// User's favorite materials
    /// </summary>
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    /// <summary>
    /// Materials recently viewed by user
    /// </summary>
    public ICollection<RecentlyViewed> RecentlyViewed { get; set; } = new List<RecentlyViewed>();

    /// <summary>
    /// Tasks owned by this user
    /// </summary>
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

    // ============================================
    // MENTORSHIP NAVIGATION PROPERTIES
    // ============================================

    /// <summary>
    /// Mentorship relations where this user is the Mentor
    /// Used by: Mentors to see their mentees
    /// </summary>
    public ICollection<MentorshipRelation> MentorshipRelationsAsMentor { get; set; } = new List<MentorshipRelation>();

    /// <summary>
    /// Mentorship relations where this user is the Mentee (Student)
    /// Used by: Students to see their mentors
    /// </summary>
    public ICollection<MentorshipRelation> MentorshipRelationsAsMentee { get; set; } = new List<MentorshipRelation>();

    /// <summary>
    /// Mentoring sessions where this user is the Mentor
    /// Used by: Mentors to see their scheduled sessions
    /// </summary>
    public ICollection<MentoringSession> MentoringSessionsAsMentor { get; set; } = new List<MentoringSession>();

    /// <summary>
    /// Mentoring sessions where this user is the Mentee (Student)
    /// Used by: Students to see their scheduled sessions
    /// </summary>
    public ICollection<MentoringSession> MentoringSessionsAsMentee { get; set; } = new List<MentoringSession>();

    /// <summary>
    /// Reviews received by this user (as Mentor)
    /// Used by: Mentors to see their reviews
    /// </summary>
    public ICollection<MentorReview> ReviewsReceived { get; set; } = new List<MentorReview>();

    /// <summary>
    /// Reviews written by this user (as Student/Mentee)
    /// Used by: Students to see reviews they've written
    /// </summary>
    public ICollection<MentorReview> ReviewsWritten { get; set; } = new List<MentorReview>();
}
