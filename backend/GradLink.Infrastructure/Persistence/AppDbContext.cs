using GradLink.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Core Entities
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    public DbSet<Resume> Resumes => Set<Resume>();
    public DbSet<JobPosting> JobPostings => Set<JobPosting>();
    public DbSet<Match> Matches => Set<Match>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<Mentor> Mentors => Set<Mentor>();
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
    public DbSet<TeamMemberTask> TeamMemberTasks => Set<TeamMemberTask>();
    public DbSet<Material> Materials => Set<Material>();
    public DbSet<Favorite> Favorites => Set<Favorite>();
    public DbSet<RecentlyViewed> RecentlyViewed => Set<RecentlyViewed>();
    public DbSet<MaterialField> MaterialFields => Set<MaterialField>();
    public DbSet<Sponsor> Sponsors => Set<Sponsor>();
    public DbSet<SponsorApplication> SponsorApplications => Set<SponsorApplication>();
    public DbSet<Subtask> Subtasks => Set<Subtask>();
    
    // Profile Entities (Role-Specific)
    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();
    public DbSet<CompanyProfile> CompanyProfiles => Set<CompanyProfile>();
    public DbSet<MentorProfile> MentorProfiles => Set<MentorProfile>();
    
    // Sponsor Dashboard Entities
    public DbSet<SponsorProfile> SponsorProfiles => Set<SponsorProfile>();
    public DbSet<SponsoredProject> SponsoredProjects => Set<SponsoredProject>();
    public DbSet<SponsorFunding> SponsorFundings => Set<SponsorFunding>();
    public DbSet<SponsorMessage> SponsorMessages => Set<SponsorMessage>();

    // Mentor Dashboard Entities
    public DbSet<MentorshipRelation> MentorshipRelations => Set<MentorshipRelation>();
    public DbSet<MentoringSession> MentoringSessions => Set<MentoringSession>();
    public DbSet<MentorReview> MentorReviews => Set<MentorReview>();

    // Internships
    public DbSet<Internship> Internships => Set<Internship>();
    public DbSet<InternshipApplication> InternshipApplications => Set<InternshipApplication>();

    // Job Applications
    public DbSet<JobApplication> JobApplications => Set<JobApplication>();

    // Notifications
    public DbSet<Notification> Notifications => Set<Notification>();

    // Password Reset
    public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();

    // Mentor Application System
    public DbSet<MentorApplication> MentorApplications => Set<MentorApplication>();
    public DbSet<MentorTest> MentorTests => Set<MentorTest>();
    public DbSet<TestQuestion> TestQuestions => Set<TestQuestion>();

    // Enhanced Materials System
    public DbSet<MaterialRating> MaterialRatings => Set<MaterialRating>();
    public DbSet<MaterialProgress> MaterialProgresses => Set<MaterialProgress>();
    public DbSet<MaterialNote> MaterialNotes => Set<MaterialNote>();
    public DbSet<MaterialCollection> MaterialCollections => Set<MaterialCollection>();
    public DbSet<CollectionMaterial> CollectionMaterials => Set<CollectionMaterial>();
    public DbSet<CollectionFollower> CollectionFollowers => Set<CollectionFollower>();

    // Email Verification
    public DbSet<EmailVerificationToken> EmailVerificationTokens => Set<EmailVerificationToken>();
    
    // OTP Verification
    public DbSet<OtpVerification> OtpVerifications => Set<OtpVerification>();

    // Task Attachments
    public DbSet<TaskAttachment> TaskAttachments => Set<TaskAttachment>();

    // Push Subscriptions
    public DbSet<PushSubscription> PushSubscriptions => Set<PushSubscription>();

    // Payments
    public DbSet<Payment> Payments => Set<Payment>();

    // Team Matchmaking
    public DbSet<TeamRequest> TeamRequests => Set<TeamRequest>();
    public DbSet<JoinRequest> JoinRequests => Set<JoinRequest>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ============================================
        // CONFIGURE ApplicationUser (Core User Table)
        // ============================================
        builder.Entity<ApplicationUser>(entity =>
        {
            // Core Authentication
            entity.Property(u => u.Role).HasMaxLength(50).HasDefaultValue("Student");
            entity.Property(u => u.AccountStatus).HasMaxLength(20).HasDefaultValue("Active");
            
            // Personal Profile
            entity.Property(u => u.FullName).HasMaxLength(100);
            entity.Property(u => u.Bio).HasMaxLength(2000);
            entity.Property(u => u.ProfilePicture).HasMaxLength(500);
            entity.Property(u => u.CoverImage).HasMaxLength(500);
            entity.Property(u => u.Gender).HasMaxLength(20);
            entity.Property(u => u.Nationality).HasMaxLength(100);
            
            // Education & Academic
            entity.Property(u => u.University).HasMaxLength(200);
            entity.Property(u => u.Faculty).HasMaxLength(200);
            entity.Property(u => u.Department).HasMaxLength(200);
            entity.Property(u => u.Major).HasMaxLength(100);
            entity.Property(u => u.AcademicYear).HasMaxLength(20);
            entity.Property(u => u.GPA).HasPrecision(3, 2);
            entity.Property(u => u.StudentId).HasMaxLength(50);
            
            // Professional
            entity.Property(u => u.JobTitle).HasMaxLength(100);
            entity.Property(u => u.Company).HasMaxLength(200);
            entity.Property(u => u.Industry).HasMaxLength(100);
            entity.Property(u => u.Skills).HasMaxLength(1000);
            entity.Property(u => u.Specialization).HasMaxLength(200);
            entity.Property(u => u.Certifications).HasMaxLength(2000);
            entity.Property(u => u.Languages).HasMaxLength(200);
            
            // Company/Sponsor
            entity.Property(u => u.CompanyName).HasMaxLength(200);
            entity.Property(u => u.CompanyWebsite).HasMaxLength(500);
            entity.Property(u => u.CompanyLogo).HasMaxLength(500);
            entity.Property(u => u.CompanySize).HasMaxLength(50);
            entity.Property(u => u.CompanyDescription).HasMaxLength(2000);
            
            // Contact & Social
            entity.Property(u => u.Location).HasMaxLength(200);
            entity.Property(u => u.Address).HasMaxLength(500);
            entity.Property(u => u.Country).HasMaxLength(100);
            entity.Property(u => u.WhatsApp).HasMaxLength(20);
            entity.Property(u => u.LinkedInUrl).HasMaxLength(500);
            entity.Property(u => u.GitHubUrl).HasMaxLength(500);
            entity.Property(u => u.TwitterUrl).HasMaxLength(500);
            entity.Property(u => u.FacebookUrl).HasMaxLength(500);
            entity.Property(u => u.WebsiteUrl).HasMaxLength(500);
            
            // Premium & Subscription
            entity.Property(u => u.PlanType).HasMaxLength(20).HasDefaultValue("Free");
            
            // System & Tracking
            entity.Property(u => u.LastLoginIp).HasMaxLength(50);
            entity.Property(u => u.LastLoginDevice).HasMaxLength(200);
            entity.Property(u => u.Preferences).HasMaxLength(5000);
            entity.Property(u => u.NotificationSettings).HasMaxLength(2000);
            entity.Property(u => u.DeactivationReason).HasMaxLength(500);
            
            // Indexes for performance
            entity.HasIndex(u => u.Role);
            entity.HasIndex(u => u.AccountStatus);
            entity.HasIndex(u => u.University);
            entity.HasIndex(u => u.CompanyName);
            entity.HasIndex(u => u.CreatedAt);
        });

        // ============================================
        // CONFIGURE StudentProfile
        // ============================================
        builder.Entity<StudentProfile>(entity =>
        {
            entity.HasKey(sp => sp.Id);
            entity.Property(sp => sp.ProjectTitle).HasMaxLength(200);
            entity.Property(sp => sp.ProjectDescription).HasMaxLength(2000);
            entity.Property(sp => sp.ProjectCategory).HasMaxLength(100);
            entity.Property(sp => sp.ProjectStatus).HasMaxLength(20).HasDefaultValue("Idea");
            entity.Property(sp => sp.TeamName).HasMaxLength(100);
            entity.Property(sp => sp.SupervisorName).HasMaxLength(100);
            entity.Property(sp => sp.SupervisorEmail).HasMaxLength(256);
            entity.Property(sp => sp.ProjectTechnologies).HasMaxLength(500);
            entity.Property(sp => sp.ProjectGitHubUrl).HasMaxLength(500);
            entity.Property(sp => sp.ProjectDemoUrl).HasMaxLength(500);
            entity.Property(sp => sp.ProjectDocumentationUrl).HasMaxLength(500);
            entity.Property(sp => sp.AcademicAwards).HasMaxLength(2000);
            entity.Property(sp => sp.RelevantCourses).HasMaxLength(2000);
            entity.Property(sp => sp.AcademicProjects).HasMaxLength(2000);
            entity.Property(sp => sp.Publications).HasMaxLength(2000);
            entity.Property(sp => sp.CareerInterests).HasMaxLength(500);
            entity.Property(sp => sp.PreferredJobTypes).HasMaxLength(200);
            entity.Property(sp => sp.PreferredLocations).HasMaxLength(500);
            entity.Property(sp => sp.ExpectedSalary).HasMaxLength(100);
            entity.Property(sp => sp.Availability).HasMaxLength(50);
            entity.Property(sp => sp.FundingNeeded).HasPrecision(18, 2);
            entity.Property(sp => sp.FundingPurpose).HasMaxLength(1000);
            
            entity.HasOne(sp => sp.User)
                .WithMany()
                .HasForeignKey(sp => sp.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(sp => sp.UserId).IsUnique();
            entity.HasIndex(sp => sp.ProjectCategory);
            entity.HasIndex(sp => sp.ProjectStatus);
        });

        // ============================================
        // CONFIGURE CompanyProfile
        // ============================================
        builder.Entity<CompanyProfile>(entity =>
        {
            entity.HasKey(cp => cp.Id);
            entity.Property(cp => cp.RegistrationNumber).HasMaxLength(100);
            entity.Property(cp => cp.EmployeeCount).HasMaxLength(50);
            entity.Property(cp => cp.CompanyType).HasMaxLength(50);
            entity.Property(cp => cp.Headquarters).HasMaxLength(200);
            entity.Property(cp => cp.OfficeLocations).HasMaxLength(2000);
            entity.Property(cp => cp.CultureDescription).HasMaxLength(2000);
            entity.Property(cp => cp.Benefits).HasMaxLength(2000);
            entity.Property(cp => cp.TechStack).HasMaxLength(1000);
            entity.Property(cp => cp.PreferredSkills).HasMaxLength(500);
            entity.Property(cp => cp.PreferredUniversities).HasMaxLength(500);
            entity.Property(cp => cp.MinimumGPA).HasPrecision(3, 2);
            entity.Property(cp => cp.InternshipDurations).HasMaxLength(200);
            entity.Property(cp => cp.Rating).HasPrecision(3, 2);
            entity.Property(cp => cp.VerificationDocuments).HasMaxLength(2000);
            
            entity.HasOne(cp => cp.User)
                .WithMany()
                .HasForeignKey(cp => cp.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(cp => cp.UserId).IsUnique();
            entity.HasIndex(cp => cp.IsHiring);
            entity.HasIndex(cp => cp.IsVerified);
        });

        // ============================================
        // CONFIGURE MentorProfile
        // ============================================
        builder.Entity<MentorProfile>(entity =>
        {
            entity.HasKey(mp => mp.Id);
            entity.Property(mp => mp.ExpertiseAreas).HasMaxLength(500);
            entity.Property(mp => mp.MentoringStyle).HasMaxLength(100);
            entity.Property(mp => mp.PreferredCommunication).HasMaxLength(200);
            entity.Property(mp => mp.AvailableTimeSlots).HasMaxLength(2000);
            entity.Property(mp => mp.Timezone).HasMaxLength(50);
            entity.Property(mp => mp.HourlyRate).HasPrecision(10, 2);
            entity.Property(mp => mp.Currency).HasMaxLength(10).HasDefaultValue("USD");
            entity.Property(mp => mp.TotalHours).HasPrecision(10, 2);
            entity.Property(mp => mp.AverageRating).HasPrecision(3, 2);
            entity.Property(mp => mp.Education).HasMaxLength(2000);
            entity.Property(mp => mp.WorkExperience).HasMaxLength(2000);
            entity.Property(mp => mp.Achievements).HasMaxLength(2000);
            entity.Property(mp => mp.FeaturedProjects).HasMaxLength(2000);
            entity.Property(mp => mp.VerificationDocuments).HasMaxLength(2000);
            
            entity.HasOne(mp => mp.User)
                .WithMany()
                .HasForeignKey(mp => mp.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(mp => mp.UserId).IsUnique();
            entity.HasIndex(mp => mp.IsAcceptingMentees);
            entity.HasIndex(mp => mp.IsVerified);
        });

        // ============================================
        // CONFIGURE Project
        // ============================================
        builder.Entity<Project>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Title).IsRequired().HasMaxLength(200);
            entity.HasOne(p => p.Owner)
                .WithMany(u => u.Projects)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure TaskItem
        builder.Entity<TaskItem>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Name).IsRequired().HasMaxLength(200);
            entity.Property(t => t.Description).HasMaxLength(2000);
            entity.Property(t => t.Notes).HasMaxLength(2000);
            entity.Property(t => t.Tags).HasMaxLength(500);
            entity.Property(t => t.Priority).HasMaxLength(20).HasDefaultValue("Medium");
            entity.Property(t => t.Person).HasMaxLength(100);
            entity.Property(t => t.Status).HasMaxLength(20).HasDefaultValue("Pending");
            entity.Property(t => t.Category).HasMaxLength(50).HasDefaultValue("Other");
            entity.Property(t => t.UserId).IsRequired();
            
            entity.HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);
            
            entity.HasIndex(t => t.UserId);
            entity.HasIndex(t => t.Status);
            entity.HasIndex(t => t.Priority);
        });

        // Configure Subtask
        builder.Entity<Subtask>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name).IsRequired().HasMaxLength(200);
            entity.HasOne(s => s.TaskItem)
                .WithMany(t => t.Subtasks)
                .HasForeignKey(s => s.TaskItemId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Resume
        builder.Entity<Resume>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.FileName).IsRequired();
            entity.Property(r => r.FilePath).IsRequired();
            entity.HasOne(r => r.User)
                .WithMany(u => u.Resumes)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure JobPosting
        builder.Entity<JobPosting>(entity =>
        {
            entity.HasKey(j => j.Id);
            entity.Property(j => j.Title).IsRequired().HasMaxLength(200);
            entity.Property(j => j.Description).IsRequired();
            entity.HasOne(j => j.PostedBy)
                .WithMany(u => u.JobPostings)
                .HasForeignKey(j => j.PostedById)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Match
        builder.Entity<Match>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.HasOne(m => m.Resume)
                .WithMany(r => r.Matches)
                .HasForeignKey(m => m.ResumeId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(m => m.JobPosting)
                .WithMany(j => j.Matches)
                .HasForeignKey(m => m.JobPostingId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure RefreshToken
        builder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(rt => rt.Id);
            entity.Property(rt => rt.Token).IsRequired();
            entity.HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Conversation
        builder.Entity<Conversation>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasOne(c => c.User1)
                .WithMany(u => u.ConversationsAsUser1)
                .HasForeignKey(c => c.User1Id)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(c => c.User2)
                .WithMany(u => u.ConversationsAsUser2)
                .HasForeignKey(c => c.User2Id)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure ChatMessage
        builder.Entity<ChatMessage>(entity =>
        {
            entity.HasKey(cm => cm.Id);
            entity.Property(cm => cm.Content).IsRequired();
            entity.HasOne(cm => cm.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(cm => cm.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Mentor
        builder.Entity<Mentor>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.FullName).IsRequired().HasMaxLength(100);
            entity.Property(m => m.Email).IsRequired().HasMaxLength(256);
            entity.Property(m => m.JobTitle).HasMaxLength(100);
            entity.Property(m => m.Specialization).HasMaxLength(100);
            entity.Property(m => m.Bio).HasMaxLength(1000);
            entity.Property(m => m.Skills).HasMaxLength(500);
            entity.Property(m => m.WhatsApp).HasMaxLength(20);
            entity.HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure TeamMember
        builder.Entity<TeamMember>(entity =>
        {
            entity.HasKey(tm => tm.Id);
            entity.Property(tm => tm.FullName).IsRequired().HasMaxLength(100);
            entity.Property(tm => tm.Email).IsRequired().HasMaxLength(256);
            entity.Property(tm => tm.Role).IsRequired().HasMaxLength(100);
            entity.Property(tm => tm.University).HasMaxLength(200);
            entity.Property(tm => tm.Skills).HasMaxLength(500);
            entity.Property(tm => tm.Availability).HasMaxLength(50);
            entity.Property(tm => tm.Rating).HasPrecision(3, 2);
            entity.HasOne(tm => tm.Project)
                .WithMany(p => p.TeamMembers)
                .HasForeignKey(tm => tm.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(tm => tm.User)
                .WithMany()
                .HasForeignKey(tm => tm.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure TeamMemberTask
        builder.Entity<TeamMemberTask>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Title).IsRequired().HasMaxLength(200);
            entity.Property(t => t.Description).HasMaxLength(1000);
            entity.Property(t => t.Status).HasMaxLength(20).HasDefaultValue("Pending");
            entity.Property(t => t.Priority).HasMaxLength(20).HasDefaultValue("Medium");
            entity.HasOne(t => t.TeamMember)
                .WithMany(tm => tm.Tasks)
                .HasForeignKey(t => t.TeamMemberId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Favorite
        builder.Entity<Favorite>(entity =>
        {
            entity.HasKey(f => f.Id);
            entity.HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(f => f.Material)
                .WithMany(m => m.Favorites)
                .HasForeignKey(f => f.MaterialId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(f => new { f.UserId, f.MaterialId }).IsUnique();
        });

        // Configure RecentlyViewed
        builder.Entity<RecentlyViewed>(entity =>
        {
            entity.HasKey(rv => rv.Id);
            entity.HasOne(rv => rv.User)
                .WithMany(u => u.RecentlyViewed)
                .HasForeignKey(rv => rv.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(rv => rv.Material)
                .WithMany(m => m.RecentlyViewed)
                .HasForeignKey(rv => rv.MaterialId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(rv => rv.Timestamp);
        });

        // Configure MaterialField
        builder.Entity<MaterialField>(entity =>
        {
            entity.HasKey(f => f.Id);
            entity.Property(f => f.Name).IsRequired().HasMaxLength(100);
            entity.HasIndex(f => f.Name).IsUnique();
        });

        // Configure Sponsor
        builder.Entity<Sponsor>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name).IsRequired().HasMaxLength(200);
            entity.Property(s => s.Field).HasMaxLength(100);
            entity.Property(s => s.SupportedProject).HasMaxLength(500);
            entity.HasIndex(s => s.Field);
        });

        // Configure SponsorApplication
        builder.Entity<SponsorApplication>(entity =>
        {
            entity.HasKey(sa => sa.Id);
            entity.Property(sa => sa.FullName).IsRequired().HasMaxLength(100);
            entity.Property(sa => sa.Email).IsRequired().HasMaxLength(256);
            entity.Property(sa => sa.CompanyName).IsRequired().HasMaxLength(200);
            entity.Property(sa => sa.SponsorshipType).IsRequired().HasMaxLength(50);
            entity.Property(sa => sa.Message).HasMaxLength(1000);
            entity.Property(sa => sa.Status).HasMaxLength(20).HasDefaultValue("Pending");
            entity.HasOne(sa => sa.User)
                .WithMany()
                .HasForeignKey(sa => sa.UserId)
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasIndex(sa => sa.Status);
            entity.HasIndex(sa => sa.Email);
        });

        // Configure SponsorProfile
        builder.Entity<SponsorProfile>(entity =>
        {
            entity.HasKey(sp => sp.Id);
            entity.Property(sp => sp.CompanyName).IsRequired().HasMaxLength(200);
            entity.Property(sp => sp.CompanyDescription).HasMaxLength(2000);
            entity.Property(sp => sp.Industry).HasMaxLength(100);
            entity.Property(sp => sp.ContactPersonName).IsRequired().HasMaxLength(100);
            entity.Property(sp => sp.ContactEmail).IsRequired().HasMaxLength(256);
            entity.Property(sp => sp.ContactPhone).HasMaxLength(20);
            entity.Property(sp => sp.Address).HasMaxLength(500);
            entity.Property(sp => sp.FieldsOfInterest).HasMaxLength(500);
            entity.Property(sp => sp.TotalBudget).HasPrecision(18, 2);
            entity.Property(sp => sp.RemainingBudget).HasPrecision(18, 2);
            entity.HasOne(sp => sp.User)
                .WithMany()
                .HasForeignKey(sp => sp.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(sp => sp.UserId).IsUnique();
            entity.HasIndex(sp => sp.Industry);
        });

        // Configure SponsoredProject
        builder.Entity<SponsoredProject>(entity =>
        {
            entity.HasKey(sp => sp.Id);
            entity.Property(sp => sp.ProjectTitle).IsRequired().HasMaxLength(200);
            entity.Property(sp => sp.ProjectDescription).HasMaxLength(2000);
            entity.Property(sp => sp.StudentName).HasMaxLength(100);
            entity.Property(sp => sp.TeamName).HasMaxLength(100);
            entity.Property(sp => sp.Category).HasMaxLength(100);
            entity.Property(sp => sp.Status).HasMaxLength(20).HasDefaultValue("Pending");
            entity.Property(sp => sp.RejectionReason).HasMaxLength(1000);
            entity.Property(sp => sp.FundingAmount).HasPrecision(18, 2);
            entity.Property(sp => sp.FundingDelivered).HasPrecision(18, 2);
            entity.Property(sp => sp.CurrentMilestone).HasMaxLength(200);
            entity.HasOne(sp => sp.SponsorProfile)
                .WithMany(s => s.SponsoredProjects)
                .HasForeignKey(sp => sp.SponsorProfileId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(sp => sp.Project)
                .WithMany()
                .HasForeignKey(sp => sp.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(sp => sp.StudentUser)
                .WithMany()
                .HasForeignKey(sp => sp.StudentUserId)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasIndex(sp => sp.Status);
        });

        // Configure SponsorFunding
        builder.Entity<SponsorFunding>(entity =>
        {
            entity.HasKey(sf => sf.Id);
            entity.Property(sf => sf.Amount).HasPrecision(18, 2).IsRequired();
            entity.Property(sf => sf.TransactionType).HasMaxLength(50).HasDefaultValue("Payment");
            entity.Property(sf => sf.Status).HasMaxLength(20).HasDefaultValue("Pending");
            entity.Property(sf => sf.TransactionReference).HasMaxLength(100);
            entity.Property(sf => sf.Notes).HasMaxLength(500);
            entity.HasOne(sf => sf.SponsorProfile)
                .WithMany(s => s.Fundings)
                .HasForeignKey(sf => sf.SponsorProfileId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(sf => sf.SponsoredProject)
                .WithMany(sp => sp.Fundings)
                .HasForeignKey(sf => sf.SponsoredProjectId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        // Configure SponsorMessage
        builder.Entity<SponsorMessage>(entity =>
        {
            entity.HasKey(sm => sm.Id);
            entity.Property(sm => sm.Content).IsRequired().HasMaxLength(5000);
            entity.Property(sm => sm.SenderId).IsRequired();
            entity.Property(sm => sm.ReceiverId).IsRequired();
            entity.HasOne(sm => sm.Sender)
                .WithMany()
                .HasForeignKey(sm => sm.SenderId)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(sm => sm.Receiver)
                .WithMany()
                .HasForeignKey(sm => sm.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(sm => sm.SponsorProfile)
                .WithMany(s => s.Messages)
                .HasForeignKey(sm => sm.SponsorProfileId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(sm => sm.SponsoredProject)
                .WithMany(sp => sp.Messages)
                .HasForeignKey(sm => sm.SponsoredProjectId)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasIndex(sm => sm.SentAt);
        });

        // ============================================
        // CONFIGURE MentorshipRelation
        // ============================================
        builder.Entity<MentorshipRelation>(entity =>
        {
            entity.HasKey(mr => mr.Id);
            
            // Relationship properties
            entity.Property(mr => mr.Status).HasMaxLength(20).HasDefaultValue("Pending");
            entity.Property(mr => mr.StatusReason).HasMaxLength(500);
            
            // Request details
            entity.Property(mr => mr.RequestMessage).HasMaxLength(2000);
            entity.Property(mr => mr.MenteeGoals).HasMaxLength(2000);
            entity.Property(mr => mr.PreferredCommunication).HasMaxLength(100);
            entity.Property(mr => mr.ExpectedDuration).HasMaxLength(50);
            
            // Mentor response
            entity.Property(mr => mr.MentorResponse).HasMaxLength(2000);
            entity.Property(mr => mr.GuidancePlan).HasMaxLength(2000);
            
            // Progress
            entity.Property(mr => mr.TotalHours).HasPrecision(10, 2);
            entity.Property(mr => mr.ProgressNotes).HasMaxLength(5000);
            
            // Feedback
            entity.Property(mr => mr.MenteeFeedback).HasMaxLength(2000);
            entity.Property(mr => mr.MentorFeedback).HasMaxLength(2000);
            
            // Relationships to ApplicationUser
            entity.HasOne(mr => mr.Mentor)
                .WithMany(u => u.MentorshipRelationsAsMentor)
                .HasForeignKey(mr => mr.MentorId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(mr => mr.Mentee)
                .WithMany(u => u.MentorshipRelationsAsMentee)
                .HasForeignKey(mr => mr.MenteeId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Indexes for performance
            entity.HasIndex(mr => mr.MentorId);
            entity.HasIndex(mr => mr.MenteeId);
            entity.HasIndex(mr => mr.Status);
            entity.HasIndex(mr => mr.RequestedAt);
            entity.HasIndex(mr => new { mr.MentorId, mr.MenteeId }).IsUnique();
        });

        // ============================================
        // CONFIGURE MentoringSession
        // ============================================
        builder.Entity<MentoringSession>(entity =>
        {
            entity.HasKey(ms => ms.Id);
            
            // Session details
            entity.Property(ms => ms.Topic).IsRequired().HasMaxLength(200);
            entity.Property(ms => ms.Description).HasMaxLength(2000);
            entity.Property(ms => ms.Timezone).HasMaxLength(50);
            
            // Session type
            entity.Property(ms => ms.SessionType).HasMaxLength(20).HasDefaultValue("Online");
            entity.Property(ms => ms.MeetingLink).HasMaxLength(500);
            entity.Property(ms => ms.MeetingPlatform).HasMaxLength(50);
            entity.Property(ms => ms.MeetingId).HasMaxLength(100);
            entity.Property(ms => ms.MeetingPassword).HasMaxLength(100);
            entity.Property(ms => ms.Location).HasMaxLength(200);
            entity.Property(ms => ms.Address).HasMaxLength(500);
            
            // Status
            entity.Property(ms => ms.Status).HasMaxLength(20).HasDefaultValue("Scheduled");
            entity.Property(ms => ms.StatusReason).HasMaxLength(500);
            entity.Property(ms => ms.CancelledBy).HasMaxLength(20);
            
            // Notes & Content
            entity.Property(ms => ms.AgendaNotes).HasMaxLength(2000);
            entity.Property(ms => ms.MentorNotes).HasMaxLength(5000);
            entity.Property(ms => ms.MenteeNotes).HasMaxLength(5000);
            entity.Property(ms => ms.ActionItems).HasMaxLength(2000);
            entity.Property(ms => ms.SharedResources).HasMaxLength(5000);
            entity.Property(ms => ms.RecordingUrl).HasMaxLength(500);
            
            // Feedback
            entity.Property(ms => ms.MenteeFeedback).HasMaxLength(2000);
            entity.Property(ms => ms.MentorFeedback).HasMaxLength(2000);
            
            // Recurrence
            entity.Property(ms => ms.RecurrencePattern).HasMaxLength(20);
            
            // Payment
            entity.Property(ms => ms.SessionFee).HasPrecision(10, 2);
            entity.Property(ms => ms.Currency).HasMaxLength(10);
            entity.Property(ms => ms.PaymentStatus).HasMaxLength(20);
            
            // Relationships
            entity.HasOne(ms => ms.Mentor)
                .WithMany(u => u.MentoringSessionsAsMentor)
                .HasForeignKey(ms => ms.MentorId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(ms => ms.Mentee)
                .WithMany(u => u.MentoringSessionsAsMentee)
                .HasForeignKey(ms => ms.MenteeId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(ms => ms.MentorshipRelation)
                .WithMany(mr => mr.Sessions)
                .HasForeignKey(ms => ms.MentorshipRelationId)
                .OnDelete(DeleteBehavior.SetNull);
            
            // Indexes for performance
            entity.HasIndex(ms => ms.MentorId);
            entity.HasIndex(ms => ms.MenteeId);
            entity.HasIndex(ms => ms.Status);
            entity.HasIndex(ms => ms.ScheduledDate);
            entity.HasIndex(ms => ms.SessionType);
            entity.HasIndex(ms => new { ms.MentorId, ms.ScheduledDate });
        });

        // ============================================
        // CONFIGURE MentorReview
        // ============================================
        builder.Entity<MentorReview>(entity =>
        {
            entity.HasKey(mr => mr.Id);
            
            // Review content
            entity.Property(mr => mr.Title).HasMaxLength(200);
            entity.Property(mr => mr.Comment).HasMaxLength(5000);
            entity.Property(mr => mr.Pros).HasMaxLength(2000);
            entity.Property(mr => mr.Cons).HasMaxLength(2000);
            
            // Context
            entity.Property(mr => mr.MentorshipType).HasMaxLength(50);
            entity.Property(mr => mr.MentorshipDuration).HasMaxLength(50);
            
            // Moderation
            entity.Property(mr => mr.FlagReason).HasMaxLength(500);
            entity.Property(mr => mr.ModerationNotes).HasMaxLength(1000);
            
            // Mentor response
            entity.Property(mr => mr.MentorResponse).HasMaxLength(2000);
            
            // Relationships
            entity.HasOne(mr => mr.Mentor)
                .WithMany(u => u.ReviewsReceived)
                .HasForeignKey(mr => mr.MentorId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(mr => mr.Reviewer)
                .WithMany(u => u.ReviewsWritten)
                .HasForeignKey(mr => mr.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(mr => mr.MentorshipRelation)
                .WithMany()
                .HasForeignKey(mr => mr.MentorshipRelationId)
                .OnDelete(DeleteBehavior.SetNull);
                
            entity.HasOne(mr => mr.MentoringSession)
                .WithMany()
                .HasForeignKey(mr => mr.MentoringSessionId)
                .OnDelete(DeleteBehavior.SetNull);
            
            // Indexes
            entity.HasIndex(mr => mr.MentorId);
            entity.HasIndex(mr => mr.ReviewerId);
            entity.HasIndex(mr => mr.Rating);
            entity.HasIndex(mr => mr.IsApproved);
            entity.HasIndex(mr => mr.CreatedAt);
            entity.HasIndex(mr => new { mr.MentorId, mr.ReviewerId }).IsUnique();
        });

        // ============================================
        // CONFIGURE Internship
        // ============================================
        builder.Entity<Internship>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Title).IsRequired().HasMaxLength(200);
            entity.Property(i => i.Description).HasMaxLength(3000);
            entity.Property(i => i.Requirements).HasMaxLength(500);
            entity.Property(i => i.Skills).HasMaxLength(500);
            entity.Property(i => i.Location).HasMaxLength(200);
            entity.Property(i => i.Duration).HasMaxLength(100);
            entity.Property(i => i.CompanyName).HasMaxLength(200);
            entity.Property(i => i.Status).HasMaxLength(50).HasDefaultValue("Active");
            entity.Property(i => i.Stipend).HasPrecision(18, 2);
            
            entity.HasOne(i => i.PostedBy)
                .WithMany()
                .HasForeignKey(i => i.PostedById)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasIndex(i => i.Status);
            entity.HasIndex(i => i.PostedById);
            entity.HasIndex(i => i.CreatedAt);
        });

        // ============================================
        // CONFIGURE InternshipApplication
        // ============================================
        builder.Entity<InternshipApplication>(entity =>
        {
            entity.HasKey(ia => ia.Id);
            entity.Property(ia => ia.CoverLetter).HasMaxLength(2000);
            entity.Property(ia => ia.Status).HasMaxLength(50).HasDefaultValue("Pending");
            entity.Property(ia => ia.Notes).HasMaxLength(1000);
            
            entity.HasOne(ia => ia.Internship)
                .WithMany(i => i.Applications)
                .HasForeignKey(ia => ia.InternshipId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(ia => ia.Applicant)
                .WithMany()
                .HasForeignKey(ia => ia.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(ia => ia.Resume)
                .WithMany()
                .HasForeignKey(ia => ia.ResumeId)
                .OnDelete(DeleteBehavior.NoAction);
            
            entity.HasIndex(ia => ia.Status);
            entity.HasIndex(ia => new { ia.InternshipId, ia.ApplicantId }).IsUnique();
        });

        // ============================================
        // CONFIGURE JobApplication
        // ============================================
        builder.Entity<JobApplication>(entity =>
        {
            entity.HasKey(ja => ja.Id);
            entity.Property(ja => ja.CoverLetter).HasMaxLength(2000);
            entity.Property(ja => ja.Status).HasMaxLength(50).HasDefaultValue("Pending");
            entity.Property(ja => ja.Notes).HasMaxLength(1000);
            
            entity.HasOne(ja => ja.JobPosting)
                .WithMany()
                .HasForeignKey(ja => ja.JobPostingId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(ja => ja.Applicant)
                .WithMany()
                .HasForeignKey(ja => ja.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(ja => ja.Resume)
                .WithMany()
                .HasForeignKey(ja => ja.ResumeId)
                .OnDelete(DeleteBehavior.NoAction);
            
            entity.HasIndex(ja => ja.Status);
            entity.HasIndex(ja => new { ja.JobPostingId, ja.ApplicantId }).IsUnique();
        });

        // ============================================
        // CONFIGURE Notification
        // ============================================
        builder.Entity<Notification>(entity =>
        {
            entity.HasKey(n => n.Id);
            entity.Property(n => n.Type).IsRequired().HasMaxLength(100);
            entity.Property(n => n.Title).IsRequired().HasMaxLength(200);
            entity.Property(n => n.Message).HasMaxLength(500);
            entity.Property(n => n.Link).HasMaxLength(500);
            entity.Property(n => n.RelatedEntityType).HasMaxLength(50);
            
            entity.HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasIndex(n => n.UserId);
            entity.HasIndex(n => n.IsRead);
            entity.HasIndex(n => n.CreatedAt);
        });

        // ============================================
        // CONFIGURE PasswordResetToken
        // ============================================
        builder.Entity<PasswordResetToken>(entity =>
        {
            entity.HasKey(prt => prt.Id);
            entity.Property(prt => prt.Token).IsRequired().HasMaxLength(500);
            
            entity.HasOne(prt => prt.User)
                .WithMany()
                .HasForeignKey(prt => prt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasIndex(prt => prt.Token);
            entity.HasIndex(prt => prt.ExpiresAt);
        });

        // ============================================
        // CONFIGURE MentorApplication
        // ============================================
        builder.Entity<MentorApplication>(entity =>
        {
            entity.HasKey(ma => ma.Id);
            entity.Property(ma => ma.FullName).IsRequired().HasMaxLength(100);
            entity.Property(ma => ma.Email).IsRequired().HasMaxLength(256);
            entity.Property(ma => ma.PhoneNumber).HasMaxLength(20);
            entity.Property(ma => ma.Specialization).IsRequired().HasMaxLength(100);
            entity.Property(ma => ma.LinkedInUrl).HasMaxLength(500);
            entity.Property(ma => ma.Bio).HasMaxLength(2000);
            entity.Property(ma => ma.CurrentPosition).HasMaxLength(500);
            entity.Property(ma => ma.Company).HasMaxLength(500);
            entity.Property(ma => ma.Status).HasMaxLength(50).HasDefaultValue("Pending");
            entity.Property(ma => ma.FinalScore).HasPrecision(5, 2);
            
            entity.HasOne(ma => ma.User)
                .WithMany()
                .HasForeignKey(ma => ma.UserId)
                .OnDelete(DeleteBehavior.SetNull);
            
            entity.HasIndex(ma => ma.Email);
            entity.HasIndex(ma => ma.Status);
            entity.HasIndex(ma => ma.CreatedAt);
        });

        // ============================================
        // CONFIGURE MentorTest
        // ============================================
        builder.Entity<MentorTest>(entity =>
        {
            entity.HasKey(mt => mt.Id);
            entity.Property(mt => mt.Token).IsRequired().HasMaxLength(100);
            entity.Property(mt => mt.Status).HasMaxLength(50).HasDefaultValue("Pending");
            entity.Property(mt => mt.Score).HasPrecision(5, 2);
            entity.Property(mt => mt.QuestionIds).HasMaxLength(1000);
            entity.Property(mt => mt.SubmittedAnswers).HasMaxLength(5000);
            
            entity.HasOne(mt => mt.Application)
                .WithMany(ma => ma.Tests)
                .HasForeignKey(mt => mt.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasIndex(mt => mt.Token).IsUnique();
            entity.HasIndex(mt => mt.Status);
            entity.HasIndex(mt => mt.ExpiresAt);
        });

        // ============================================
        // CONFIGURE TestQuestion
        // ============================================
        builder.Entity<TestQuestion>(entity =>
        {
            entity.HasKey(tq => tq.Id);
            entity.Property(tq => tq.Category).IsRequired().HasMaxLength(100);
            entity.Property(tq => tq.QuestionText).IsRequired().HasMaxLength(1000);
            entity.Property(tq => tq.OptionA).IsRequired().HasMaxLength(500);
            entity.Property(tq => tq.OptionB).IsRequired().HasMaxLength(500);
            entity.Property(tq => tq.OptionC).IsRequired().HasMaxLength(500);
            entity.Property(tq => tq.OptionD).IsRequired().HasMaxLength(500);
            entity.Property(tq => tq.CorrectAnswer).IsRequired().HasMaxLength(1);
            entity.Property(tq => tq.Difficulty).HasMaxLength(20).HasDefaultValue("Medium");
            entity.Property(tq => tq.Explanation).HasMaxLength(1000);
            
            entity.HasIndex(tq => tq.Category);
            entity.HasIndex(tq => tq.Difficulty);
            entity.HasIndex(tq => tq.IsActive);
        });

        // ============================================
        // CONFIGURE Enhanced Material (updated)
        // ============================================
        builder.Entity<Material>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Title).IsRequired().HasMaxLength(200);
            entity.Property(m => m.Description).HasMaxLength(2000);
            entity.Property(m => m.Type).IsRequired().HasMaxLength(50);
            entity.Property(m => m.Field).HasMaxLength(100);
            entity.Property(m => m.FileType).HasMaxLength(20);
            entity.Property(m => m.Author).HasMaxLength(200);
            entity.Property(m => m.Link).HasMaxLength(1000);
            entity.Property(m => m.FilePath).HasMaxLength(500);
            entity.Property(m => m.ThumbnailUrl).HasMaxLength(500);
            entity.Property(m => m.Tags).HasMaxLength(500);
            entity.Property(m => m.Status).HasMaxLength(20).HasDefaultValue("pending");
            entity.Property(m => m.DifficultyLevel).HasMaxLength(20);
            entity.Property(m => m.ReviewNotes).HasMaxLength(1000);
            entity.Property(m => m.AverageRating).HasPrecision(3, 2);
            
            entity.HasOne(m => m.SubmittedBy)
                .WithMany()
                .HasForeignKey(m => m.SubmittedById)
                .OnDelete(DeleteBehavior.NoAction);
            
            entity.HasOne(m => m.ReviewedBy)
                .WithMany()
                .HasForeignKey(m => m.ReviewedById)
                .OnDelete(DeleteBehavior.NoAction);
            
            entity.HasIndex(m => m.Type);
            entity.HasIndex(m => m.Field);
            entity.HasIndex(m => m.Status);
            entity.HasIndex(m => m.IsFeatured);
            entity.HasIndex(m => m.CreatedAt);
        });

        // ============================================
        // CONFIGURE MaterialRating
        // ============================================
        builder.Entity<MaterialRating>(entity =>
        {
            entity.HasKey(mr => mr.Id);
            entity.Property(mr => mr.ReviewText).HasMaxLength(1000);
            
            entity.HasOne(mr => mr.Material)
                .WithMany(m => m.Ratings)
                .HasForeignKey(mr => mr.MaterialId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(mr => mr.User)
                .WithMany()
                .HasForeignKey(mr => mr.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            
            entity.HasIndex(mr => new { mr.MaterialId, mr.UserId }).IsUnique();
            entity.HasIndex(mr => mr.Rating);
            entity.HasIndex(mr => mr.CreatedAt);
        });

        // ============================================
        // CONFIGURE MaterialProgress
        // ============================================
        builder.Entity<MaterialProgress>(entity =>
        {
            entity.HasKey(mp => mp.Id);
            entity.Property(mp => mp.Status).HasMaxLength(20).HasDefaultValue("not_started");
            entity.Property(mp => mp.LastPosition).HasMaxLength(100);
            
            entity.HasOne(mp => mp.Material)
                .WithMany(m => m.ProgressRecords)
                .HasForeignKey(mp => mp.MaterialId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(mp => mp.User)
                .WithMany()
                .HasForeignKey(mp => mp.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            
            entity.HasIndex(mp => new { mp.MaterialId, mp.UserId }).IsUnique();
            entity.HasIndex(mp => mp.Status);
            entity.HasIndex(mp => mp.IsBookmarked);
            entity.HasIndex(mp => mp.LastAccessedAt);
        });

        // ============================================
        // CONFIGURE MaterialNote
        // ============================================
        builder.Entity<MaterialNote>(entity =>
        {
            entity.HasKey(mn => mn.Id);
            entity.Property(mn => mn.Content).IsRequired().HasMaxLength(5000);
            entity.Property(mn => mn.Position).HasMaxLength(100);
            entity.Property(mn => mn.Color).HasMaxLength(20);
            
            entity.HasOne(mn => mn.Material)
                .WithMany(m => m.Notes)
                .HasForeignKey(mn => mn.MaterialId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(mn => mn.User)
                .WithMany()
                .HasForeignKey(mn => mn.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            
            entity.HasIndex(mn => new { mn.MaterialId, mn.UserId });
            entity.HasIndex(mn => mn.CreatedAt);
        });

        // ============================================
        // CONFIGURE MaterialCollection
        // ============================================
        builder.Entity<MaterialCollection>(entity =>
        {
            entity.HasKey(mc => mc.Id);
            entity.Property(mc => mc.Title).IsRequired().HasMaxLength(200);
            entity.Property(mc => mc.Description).HasMaxLength(1000);
            entity.Property(mc => mc.CoverImageUrl).HasMaxLength(500);
            entity.Property(mc => mc.Type).HasMaxLength(20).HasDefaultValue("personal");
            entity.Property(mc => mc.Field).HasMaxLength(100);
            entity.Property(mc => mc.DifficultyLevel).HasMaxLength(20);
            
            entity.HasOne(mc => mc.Owner)
                .WithMany()
                .HasForeignKey(mc => mc.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasIndex(mc => mc.OwnerId);
            entity.HasIndex(mc => mc.Type);
            entity.HasIndex(mc => mc.IsPublic);
            entity.HasIndex(mc => mc.IsSystemPath);
        });

        // ============================================
        // CONFIGURE CollectionMaterial
        // ============================================
        builder.Entity<CollectionMaterial>(entity =>
        {
            entity.HasKey(cm => cm.Id);
            
            entity.HasOne(cm => cm.Collection)
                .WithMany(c => c.Materials)
                .HasForeignKey(cm => cm.CollectionId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(cm => cm.Material)
                .WithMany(m => m.CollectionMaterials)
                .HasForeignKey(cm => cm.MaterialId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasIndex(cm => new { cm.CollectionId, cm.MaterialId }).IsUnique();
            entity.HasIndex(cm => cm.Order);
        });

        // ============================================
        // CONFIGURE CollectionFollower
        // ============================================
        builder.Entity<CollectionFollower>(entity =>
        {
            entity.HasKey(cf => cf.Id);
            
            entity.HasOne(cf => cf.Collection)
                .WithMany(c => c.Followers)
                .HasForeignKey(cf => cf.CollectionId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(cf => cf.User)
                .WithMany()
                .HasForeignKey(cf => cf.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            
            entity.HasIndex(cf => new { cf.CollectionId, cf.UserId }).IsUnique();
        });

        // ============================================
        // CONFIGURE EmailVerificationToken
        // ============================================
        builder.Entity<EmailVerificationToken>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Token).IsRequired();
            
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasIndex(e => e.Token);
            entity.HasIndex(e => e.ExpiresAt);
        });

        // ============================================
        // CONFIGURE OtpVerification
        // ============================================
        builder.Entity<OtpVerification>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OtpCode).IsRequired().HasMaxLength(6);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasIndex(e => e.Email);
            entity.HasIndex(e => e.ExpiresAt);
            entity.HasIndex(e => new { e.Email, e.OtpCode });
        });

        // ============================================
        // CONFIGURE TaskAttachment
        // ============================================
        builder.Entity<TaskAttachment>(entity =>
        {
            entity.HasKey(ta => ta.Id);
            entity.Property(ta => ta.FileName).IsRequired();
            entity.Property(ta => ta.OriginalFileName).IsRequired();
            entity.Property(ta => ta.ContentType).IsRequired();
            entity.Property(ta => ta.StoragePath).IsRequired();
            
            entity.HasOne(ta => ta.Task)
                .WithMany()
                .HasForeignKey(ta => ta.TaskId)
                .OnDelete(DeleteBehavior.NoAction);
            
            entity.HasOne(ta => ta.UploadedBy)
                .WithMany()
                .HasForeignKey(ta => ta.UploadedById)
                .OnDelete(DeleteBehavior.NoAction);
            
            entity.HasIndex(ta => ta.TaskId);
        });

        // ============================================
        // CONFIGURE PushSubscription
        // ============================================
        builder.Entity<PushSubscription>(entity =>
        {
            entity.HasKey(ps => ps.Id);
            entity.Property(ps => ps.Endpoint).IsRequired();
            entity.Property(ps => ps.P256dh).IsRequired();
            entity.Property(ps => ps.Auth).IsRequired();
            
            entity.HasOne(ps => ps.User)
                .WithMany()
                .HasForeignKey(ps => ps.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasIndex(ps => ps.UserId);
            entity.HasIndex(ps => ps.Endpoint);
        });

        // ============================================
        // CONFIGURE Payment
        // ============================================
        builder.Entity<Payment>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.PaymentIntentId).IsRequired();
            entity.Property(p => p.Amount).HasPrecision(18, 2);
            entity.Property(p => p.Currency).IsRequired();
            entity.Property(p => p.Status).IsRequired();
            
            entity.HasOne(p => p.Payer)
                .WithMany()
                .HasForeignKey(p => p.PayerId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(p => p.Sponsorship)
                .WithMany()
                .HasForeignKey(p => p.SponsorshipId)
                .OnDelete(DeleteBehavior.NoAction);
            
            entity.HasOne(p => p.Project)
                .WithMany()
                .HasForeignKey(p => p.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);
            
            entity.HasIndex(p => p.PaymentIntentId);
            entity.HasIndex(p => p.Status);
            entity.HasIndex(p => p.PayerId);
        });

        // ============================================
        // CONFIGURE TeamRequest
        // ============================================
        builder.Entity<TeamRequest>(entity =>
        {
            entity.HasKey(tr => tr.Id);
            entity.Property(tr => tr.ProjectName).IsRequired().HasMaxLength(200);
            entity.Property(tr => tr.Description).HasMaxLength(2000);
            entity.Property(tr => tr.Category).HasMaxLength(50);
            entity.Property(tr => tr.SkillsNeeded).HasMaxLength(500);
            entity.Property(tr => tr.Status).HasMaxLength(20).HasDefaultValue("Active");
            entity.Property(tr => tr.ContactEmail).HasMaxLength(256);
            
            entity.HasOne(tr => tr.Owner)
                .WithMany()
                .HasForeignKey(tr => tr.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasIndex(tr => tr.OwnerId);
            entity.HasIndex(tr => tr.Status);
            entity.HasIndex(tr => tr.Category);
            entity.HasIndex(tr => tr.CreatedAt);
        });

        // ============================================
        // CONFIGURE JoinRequest
        // ============================================
        builder.Entity<JoinRequest>(entity =>
        {
            entity.HasKey(jr => jr.Id);
            entity.Property(jr => jr.ApplicantName).IsRequired().HasMaxLength(100);
            entity.Property(jr => jr.ApplicantEmail).IsRequired().HasMaxLength(256);
            entity.Property(jr => jr.ApplicantSkills).HasMaxLength(500);
            entity.Property(jr => jr.Message).HasMaxLength(2000);
            entity.Property(jr => jr.Status).HasMaxLength(20).HasDefaultValue("Pending");
            
            entity.HasOne(jr => jr.TeamRequest)
                .WithMany(tr => tr.JoinRequests)
                .HasForeignKey(jr => jr.TeamRequestId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(jr => jr.Applicant)
                .WithMany()
                .HasForeignKey(jr => jr.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasIndex(jr => jr.TeamRequestId);
            entity.HasIndex(jr => jr.ApplicantId);
            entity.HasIndex(jr => jr.Status);
            entity.HasIndex(jr => new { jr.TeamRequestId, jr.ApplicantId }).IsUnique();
        });
    }
}
