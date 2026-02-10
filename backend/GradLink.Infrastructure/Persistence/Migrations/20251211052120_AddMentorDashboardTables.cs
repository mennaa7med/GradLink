using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GradLink.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMentorDashboardTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InternshipApplications");

            migrationBuilder.DropTable(
                name: "JobApplications");

            migrationBuilder.DropTable(
                name: "Internships");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_CompanyProfiles_Industry",
                table: "CompanyProfiles");

            migrationBuilder.DropIndex(
                name: "IX_CompanyProfiles_IsActive",
                table: "CompanyProfiles");

            migrationBuilder.DropColumn(
                name: "ActiveInternships",
                table: "CompanyProfiles");

            migrationBuilder.DropColumn(
                name: "City",
                table: "CompanyProfiles");

            migrationBuilder.DropColumn(
                name: "ContactPersonName",
                table: "CompanyProfiles");

            migrationBuilder.DropColumn(
                name: "FacebookUrl",
                table: "CompanyProfiles");

            migrationBuilder.DropColumn(
                name: "LinkedInUrl",
                table: "CompanyProfiles");

            migrationBuilder.RenameColumn(
                name: "TwitterUrl",
                table: "CompanyProfiles",
                newName: "PreferredUniversities");

            migrationBuilder.RenameColumn(
                name: "TotalJobsPosted",
                table: "CompanyProfiles",
                newName: "TrustScore");

            migrationBuilder.RenameColumn(
                name: "TotalInternshipsPosted",
                table: "CompanyProfiles",
                newName: "ReviewsCount");

            migrationBuilder.RenameColumn(
                name: "Logo",
                table: "CompanyProfiles",
                newName: "PreferredSkills");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "CompanyProfiles",
                newName: "OffersProjectJobs");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "CompanyProfiles",
                newName: "RegistrationNumber");

            migrationBuilder.RenameColumn(
                name: "ActiveJobs",
                table: "CompanyProfiles",
                newName: "OpenPositionsCount");

            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "CompanyProfiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Industry",
                table: "CompanyProfiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactPhone",
                table: "CompanyProfiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactEmail",
                table: "CompanyProfiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "CompanySize",
                table: "CompanyProfiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyName",
                table: "CompanyProfiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyDescription",
                table: "CompanyProfiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "CompanyProfiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AverageResponseTime",
                table: "CompanyProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Benefits",
                table: "CompanyProfiles",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyType",
                table: "CompanyProfiles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CultureDescription",
                table: "CompanyProfiles",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeCount",
                table: "CompanyProfiles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Headquarters",
                table: "CompanyProfiles",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HiringStatus",
                table: "CompanyProfiles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "InternshipDurations",
                table: "CompanyProfiles",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHiring",
                table: "CompanyProfiles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "CompanyProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MinimumGPA",
                table: "CompanyProfiles",
                type: "decimal(3,2)",
                precision: 3,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OffersInternships",
                table: "CompanyProfiles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OfficeLocations",
                table: "CompanyProfiles",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Rating",
                table: "CompanyProfiles",
                type: "decimal(3,2)",
                precision: 3,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TechStack",
                table: "CompanyProfiles",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerificationDocuments",
                table: "CompanyProfiles",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Major",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Company",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "AspNetUsers",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicYear",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountStatus",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Active");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationsCount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Certifications",
                table: "AspNetUsers",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyDescription",
                table: "AspNetUsers",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyLogo",
                table: "AspNetUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanySize",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyWebsite",
                table: "AspNetUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompletedProjectsCount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverImage",
                table: "AspNetUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeactivationReason",
                table: "AspNetUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExperienceYears",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacebookUrl",
                table: "AspNetUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Faculty",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GPA",
                table: "AspNetUsers",
                type: "decimal(3,2)",
                precision: 3,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GitHubUrl",
                table: "AspNetUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Industry",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOnline",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPremium",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsProfilePublic",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JobsPostedCount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Languages",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastActivityAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastLoginDevice",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastLoginIp",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedInUrl",
                table: "AspNetUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NotificationSettings",
                table: "AspNetUsers",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlanType",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Free");

            migrationBuilder.AddColumn<string>(
                name: "Preferences",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PremiumEndDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PremiumStartDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfileCompletionPercentage",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProfileViewsCount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProjectsCount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Student");

            migrationBuilder.AddColumn<string>(
                name: "Skills",
                table: "AspNetUsers",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Specialization",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitterUrl",
                table: "AspNetUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "University",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebsiteUrl",
                table: "AspNetUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhatsApp",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MentorProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExpertiseAreas = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MentoringStyle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MaxMentees = table.Column<int>(type: "int", nullable: true),
                    CurrentMenteesCount = table.Column<int>(type: "int", nullable: false),
                    IsAcceptingMentees = table.Column<bool>(type: "bit", nullable: false),
                    PreferredCommunication = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AvailableTimeSlots = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Timezone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SessionDurationMinutes = table.Column<int>(type: "int", nullable: false),
                    IsFree = table.Column<bool>(type: "bit", nullable: false),
                    HourlyRate = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, defaultValue: "USD"),
                    TotalSessions = table.Column<int>(type: "int", nullable: false),
                    TotalHours = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    TotalStudentsMentored = table.Column<int>(type: "int", nullable: false),
                    AverageRating = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: true),
                    ReviewsCount = table.Column<int>(type: "int", nullable: false),
                    ResponseRate = table.Column<int>(type: "int", nullable: false),
                    Education = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    WorkExperience = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Achievements = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    FeaturedProjects = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerificationDocuments = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MentorProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MentorshipRelations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MentorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MenteeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pending"),
                    StatusReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RequestMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    MenteeGoals = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PreferredCommunication = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExpectedDuration = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MentorResponse = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    GuidancePlan = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ProgressPercentage = table.Column<int>(type: "int", nullable: false),
                    SessionsCompleted = table.Column<int>(type: "int", nullable: false),
                    TotalHours = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    ProgressNotes = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RespondedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastActivityAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HasMenteeFeedback = table.Column<bool>(type: "bit", nullable: false),
                    MenteeRating = table.Column<int>(type: "int", nullable: true),
                    MenteeFeedback = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    HasMentorFeedback = table.Column<bool>(type: "bit", nullable: false),
                    MentorFeedback = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorshipRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MentorshipRelations_AspNetUsers_MenteeId",
                        column: x => x.MenteeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MentorshipRelations_AspNetUsers_MentorId",
                        column: x => x.MentorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ProjectDescription = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ProjectCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProjectStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Idea"),
                    TeamName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TeamSize = table.Column<int>(type: "int", nullable: true),
                    SupervisorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SupervisorEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ProjectStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProjectEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProjectTechnologies = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProjectGitHubUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProjectDemoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProjectDocumentationUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AcademicAwards = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    RelevantCourses = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    AcademicProjects = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Publications = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CareerInterests = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PreferredJobTypes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PreferredLocations = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ExpectedSalary = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Availability = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsOpenToRelocation = table.Column<bool>(type: "bit", nullable: false),
                    IsOpenToRemote = table.Column<bool>(type: "bit", nullable: false),
                    IsLookingForMentor = table.Column<bool>(type: "bit", nullable: false),
                    IsLookingForSponsor = table.Column<bool>(type: "bit", nullable: false),
                    FundingNeeded = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    FundingPurpose = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MentoringSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MentorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MenteeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MentorshipRelationId = table.Column<int>(type: "int", nullable: true),
                    Topic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    Timezone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SessionType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Online"),
                    MeetingLink = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MeetingPlatform = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MeetingId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MeetingPassword = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Scheduled"),
                    StatusReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CancelledBy = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ActualStartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualEndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualDurationMinutes = table.Column<int>(type: "int", nullable: true),
                    AgendaNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    MentorNotes = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    MenteeNotes = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    ActionItems = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    SharedResources = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    RecordingUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HasMenteeRating = table.Column<bool>(type: "bit", nullable: false),
                    MenteeRating = table.Column<int>(type: "int", nullable: true),
                    MenteeFeedback = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    HasMentorRating = table.Column<bool>(type: "bit", nullable: false),
                    MentorRating = table.Column<int>(type: "int", nullable: true),
                    MentorFeedback = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    WasHelpful = table.Column<bool>(type: "bit", nullable: true),
                    WouldRecommend = table.Column<bool>(type: "bit", nullable: true),
                    ReminderSent = table.Column<bool>(type: "bit", nullable: false),
                    ReminderSentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReminderMinutesBefore = table.Column<int>(type: "int", nullable: false),
                    MentorNotified = table.Column<bool>(type: "bit", nullable: false),
                    MenteeNotified = table.Column<bool>(type: "bit", nullable: false),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false),
                    RecurrencePattern = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ParentSessionId = table.Column<int>(type: "int", nullable: true),
                    RecurrenceEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SessionNumber = table.Column<int>(type: "int", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    SessionFee = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentoringSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MentoringSessions_AspNetUsers_MenteeId",
                        column: x => x.MenteeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MentoringSessions_AspNetUsers_MentorId",
                        column: x => x.MentorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MentoringSessions_MentorshipRelations_MentorshipRelationId",
                        column: x => x.MentorshipRelationId,
                        principalTable: "MentorshipRelations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "MentorReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MentorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReviewerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MentorshipRelationId = table.Column<int>(type: "int", nullable: true),
                    MentoringSessionId = table.Column<int>(type: "int", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    KnowledgeRating = table.Column<int>(type: "int", nullable: true),
                    CommunicationRating = table.Column<int>(type: "int", nullable: true),
                    AvailabilityRating = table.Column<int>(type: "int", nullable: true),
                    HelpfulnessRating = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    Pros = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Cons = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    WouldRecommend = table.Column<bool>(type: "bit", nullable: false),
                    MentorshipType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MentorshipDuration = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SessionsCount = table.Column<int>(type: "int", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsFlagged = table.Column<bool>(type: "bit", nullable: false),
                    FlagReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsHidden = table.Column<bool>(type: "bit", nullable: false),
                    ModerationNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ShowReviewerName = table.Column<bool>(type: "bit", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    HelpfulCount = table.Column<int>(type: "int", nullable: false),
                    NotHelpfulCount = table.Column<int>(type: "int", nullable: false),
                    HasMentorResponse = table.Column<bool>(type: "bit", nullable: false),
                    MentorResponse = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    MentorRespondedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MentorReviews_AspNetUsers_MentorId",
                        column: x => x.MentorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MentorReviews_AspNetUsers_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MentorReviews_MentoringSessions_MentoringSessionId",
                        column: x => x.MentoringSessionId,
                        principalTable: "MentoringSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MentorReviews_MentorshipRelations_MentorshipRelationId",
                        column: x => x.MentorshipRelationId,
                        principalTable: "MentorshipRelations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfiles_IsHiring",
                table: "CompanyProfiles",
                column: "IsHiring");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AccountStatus",
                table: "AspNetUsers",
                column: "AccountStatus");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompanyName",
                table: "AspNetUsers",
                column: "CompanyName");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CreatedAt",
                table: "AspNetUsers",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Role",
                table: "AspNetUsers",
                column: "Role");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_University",
                table: "AspNetUsers",
                column: "University");

            migrationBuilder.CreateIndex(
                name: "IX_MentoringSessions_MenteeId",
                table: "MentoringSessions",
                column: "MenteeId");

            migrationBuilder.CreateIndex(
                name: "IX_MentoringSessions_MentorId",
                table: "MentoringSessions",
                column: "MentorId");

            migrationBuilder.CreateIndex(
                name: "IX_MentoringSessions_MentorId_ScheduledDate",
                table: "MentoringSessions",
                columns: new[] { "MentorId", "ScheduledDate" });

            migrationBuilder.CreateIndex(
                name: "IX_MentoringSessions_MentorshipRelationId",
                table: "MentoringSessions",
                column: "MentorshipRelationId");

            migrationBuilder.CreateIndex(
                name: "IX_MentoringSessions_ScheduledDate",
                table: "MentoringSessions",
                column: "ScheduledDate");

            migrationBuilder.CreateIndex(
                name: "IX_MentoringSessions_SessionType",
                table: "MentoringSessions",
                column: "SessionType");

            migrationBuilder.CreateIndex(
                name: "IX_MentoringSessions_Status",
                table: "MentoringSessions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_MentorProfiles_IsAcceptingMentees",
                table: "MentorProfiles",
                column: "IsAcceptingMentees");

            migrationBuilder.CreateIndex(
                name: "IX_MentorProfiles_IsVerified",
                table: "MentorProfiles",
                column: "IsVerified");

            migrationBuilder.CreateIndex(
                name: "IX_MentorProfiles_UserId",
                table: "MentorProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MentorReviews_CreatedAt",
                table: "MentorReviews",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_MentorReviews_IsApproved",
                table: "MentorReviews",
                column: "IsApproved");

            migrationBuilder.CreateIndex(
                name: "IX_MentorReviews_MentorId",
                table: "MentorReviews",
                column: "MentorId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorReviews_MentorId_ReviewerId",
                table: "MentorReviews",
                columns: new[] { "MentorId", "ReviewerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MentorReviews_MentoringSessionId",
                table: "MentorReviews",
                column: "MentoringSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorReviews_MentorshipRelationId",
                table: "MentorReviews",
                column: "MentorshipRelationId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorReviews_Rating",
                table: "MentorReviews",
                column: "Rating");

            migrationBuilder.CreateIndex(
                name: "IX_MentorReviews_ReviewerId",
                table: "MentorReviews",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorshipRelations_MenteeId",
                table: "MentorshipRelations",
                column: "MenteeId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorshipRelations_MentorId",
                table: "MentorshipRelations",
                column: "MentorId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorshipRelations_MentorId_MenteeId",
                table: "MentorshipRelations",
                columns: new[] { "MentorId", "MenteeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MentorshipRelations_RequestedAt",
                table: "MentorshipRelations",
                column: "RequestedAt");

            migrationBuilder.CreateIndex(
                name: "IX_MentorshipRelations_Status",
                table: "MentorshipRelations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_StudentProfiles_ProjectCategory",
                table: "StudentProfiles",
                column: "ProjectCategory");

            migrationBuilder.CreateIndex(
                name: "IX_StudentProfiles_ProjectStatus",
                table: "StudentProfiles",
                column: "ProjectStatus");

            migrationBuilder.CreateIndex(
                name: "IX_StudentProfiles_UserId",
                table: "StudentProfiles",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MentorProfiles");

            migrationBuilder.DropTable(
                name: "MentorReviews");

            migrationBuilder.DropTable(
                name: "StudentProfiles");

            migrationBuilder.DropTable(
                name: "MentoringSessions");

            migrationBuilder.DropTable(
                name: "MentorshipRelations");

            migrationBuilder.DropIndex(
                name: "IX_CompanyProfiles_IsHiring",
                table: "CompanyProfiles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AccountStatus",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CompanyName",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CreatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Role",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_University",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AverageResponseTime",
                table: "CompanyProfiles");

            migrationBuilder.DropColumn(
                name: "Benefits",
                table: "CompanyProfiles");

            migrationBuilder.DropColumn(
                name: "CompanyType",
                table: "CompanyProfiles");

            migrationBuilder.DropColumn(
                name: "CultureDescription",
                table: "CompanyProfiles");

            migrationBuilder.DropColumn(
                name: "EmployeeCount",
                table: "CompanyProfiles");

            migrationBuilder.DropColumn(
                name: "Headquarters",
                table: "CompanyProfiles");

            migrationBuilder.DropColumn(
                name: "HiringStatus",
                table: "CompanyProfiles");

            migrationBuilder.DropColumn(
                name: "InternshipDurations",
                table: "CompanyProfiles");

            migrationBuilder.DropColumn(
                name: "IsHiring",
                table: "CompanyProfiles");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "CompanyProfiles");

            migrationBuilder.DropColumn(
                name: "MinimumGPA",
                table: "CompanyProfiles");

            migrationBuilder.DropColumn(
                name: "OffersInternships",
                table: "CompanyProfiles");

            migrationBuilder.DropColumn(
                name: "OfficeLocations",
                table: "CompanyProfiles");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "CompanyProfiles");

            migrationBuilder.DropColumn(
                name: "TechStack",
                table: "CompanyProfiles");

            migrationBuilder.DropColumn(
                name: "VerificationDocuments",
                table: "CompanyProfiles");

            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AccountStatus",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ApplicationsCount",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Certifications",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompanyDescription",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompanyLogo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompanySize",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompanyWebsite",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompletedProjectsCount",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CoverImage",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DeactivationReason",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ExperienceYears",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FacebookUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Faculty",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GPA",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GitHubUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Industry",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsOnline",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsPremium",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsProfilePublic",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "JobsPostedCount",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Languages",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastActivityAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastLoginDevice",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastLoginIp",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LinkedInUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NotificationSettings",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PlanType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Preferences",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PremiumEndDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PremiumStartDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfileCompletionPercentage",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfileViewsCount",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProjectsCount",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Skills",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Specialization",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TwitterUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "University",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WebsiteUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WhatsApp",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "TrustScore",
                table: "CompanyProfiles",
                newName: "TotalJobsPosted");

            migrationBuilder.RenameColumn(
                name: "ReviewsCount",
                table: "CompanyProfiles",
                newName: "TotalInternshipsPosted");

            migrationBuilder.RenameColumn(
                name: "RegistrationNumber",
                table: "CompanyProfiles",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "PreferredUniversities",
                table: "CompanyProfiles",
                newName: "TwitterUrl");

            migrationBuilder.RenameColumn(
                name: "PreferredSkills",
                table: "CompanyProfiles",
                newName: "Logo");

            migrationBuilder.RenameColumn(
                name: "OpenPositionsCount",
                table: "CompanyProfiles",
                newName: "ActiveJobs");

            migrationBuilder.RenameColumn(
                name: "OffersProjectJobs",
                table: "CompanyProfiles",
                newName: "IsActive");

            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "CompanyProfiles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Industry",
                table: "CompanyProfiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactPhone",
                table: "CompanyProfiles",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactEmail",
                table: "CompanyProfiles",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanySize",
                table: "CompanyProfiles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyName",
                table: "CompanyProfiles",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyDescription",
                table: "CompanyProfiles",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "CompanyProfiles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActiveInternships",
                table: "CompanyProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "CompanyProfiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPersonName",
                table: "CompanyProfiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FacebookUrl",
                table: "CompanyProfiles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedInUrl",
                table: "CompanyProfiles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Major",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Company",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Internships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyProfileId = table.Column<int>(type: "int", nullable: false),
                    AcademicYear = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AllowExternalApplications = table.Column<bool>(type: "bit", nullable: false),
                    ApplicationDeadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApplicationEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ApplicationInstructions = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ApplicationsCount = table.Column<int>(type: "int", nullable: false),
                    AvailableSlots = table.Column<int>(type: "int", nullable: false),
                    Benefits = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    DurationMonths = table.Column<int>(type: "int", nullable: true),
                    DurationWeeks = table.Column<int>(type: "int", nullable: true),
                    EducationLevel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExternalApplicationUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FieldOfStudy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FilledSlots = table.Column<int>(type: "int", nullable: false),
                    HasFullTimeOpportunity = table.Column<bool>(type: "bit", nullable: false),
                    HasMentorship = table.Column<bool>(type: "bit", nullable: false),
                    HoursPerWeek = table.Column<int>(type: "int", nullable: true),
                    InternshipType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    IsStartDateFlexible = table.Column<bool>(type: "bit", nullable: false),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: false),
                    LanguageRequirements = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LearningOutcomes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MaxDurationWeeks = table.Column<int>(type: "int", nullable: true),
                    MinDurationWeeks = table.Column<int>(type: "int", nullable: true),
                    MinGPA = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: true),
                    PostedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PreferredSkills = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ProvidesCertificate = table.Column<bool>(type: "bit", nullable: false),
                    Requirements = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    ResponseTime = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Responsibilities = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    Schedule = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SelectionProcess = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ShowStipend = table.Column<bool>(type: "bit", nullable: false),
                    Skills = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Draft"),
                    StipendAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    StipendCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "USD"),
                    StipendMax = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    StipendPeriod = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Monthly"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TotalSlots = table.Column<int>(type: "int", nullable: false),
                    TrainingProvided = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ViewsCount = table.Column<int>(type: "int", nullable: false),
                    WorkMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Internships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Internships_CompanyProfiles_CompanyProfileId",
                        column: x => x.CompanyProfileId,
                        principalTable: "CompanyProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyProfileId = table.Column<int>(type: "int", nullable: false),
                    AllowExternalApplications = table.Column<bool>(type: "bit", nullable: false),
                    ApplicationEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ApplicationsCount = table.Column<int>(type: "int", nullable: false),
                    Benefits = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    EducationLevel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExperienceLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExternalApplicationUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FieldOfStudy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FilledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: false),
                    JobType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MaxExperience = table.Column<int>(type: "int", nullable: true),
                    MinExperience = table.Column<int>(type: "int", nullable: true),
                    PostedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PreferredSkills = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Requirements = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    Responsibilities = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    SalaryCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "USD"),
                    SalaryMax = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    SalaryMin = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    SalaryPeriod = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Monthly"),
                    ShowSalary = table.Column<bool>(type: "bit", nullable: false),
                    Skills = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Draft"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Vacancies = table.Column<int>(type: "int", nullable: false),
                    ViewsCount = table.Column<int>(type: "int", nullable: false),
                    WorkMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_CompanyProfiles_CompanyProfileId",
                        column: x => x.CompanyProfileId,
                        principalTable: "CompanyProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InternshipApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicantUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InternshipId = table.Column<int>(type: "int", nullable: false),
                    AIAnalysis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AIScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    AcademicScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    AcademicYear = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AdditionalDocuments = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    AnalyzedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApplicantEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ApplicantName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ApplicantPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AppliedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Availability = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CompanyNotes = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    CompanyRating = table.Column<int>(type: "int", nullable: true),
                    CoverLetter = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    ExpectedGraduation = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtractedSkills = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GPA = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: true),
                    GitHubUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HasBeenContacted = table.Column<bool>(type: "bit", nullable: false),
                    InterviewLocation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InterviewNotes = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    InterviewScheduledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InterviewScore = table.Column<int>(type: "int", nullable: true),
                    InterviewType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsFavorite = table.Column<bool>(type: "bit", nullable: false),
                    LastContactedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LinkedInUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Major = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MatchPercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    MessagesCount = table.Column<int>(type: "int", nullable: false),
                    OfferAcceptedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OfferDeadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OfferExtendedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OfferedStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OfferedStipend = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    PortfolioUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PreferredStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReferredBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RelevantExperience = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ResumeFileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ResumeUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShortlistedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SkillsScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    Source = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    StatusUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    University = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WhyInterested = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    WithdrawalReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternshipApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InternshipApplications_AspNetUsers_ApplicantUserId",
                        column: x => x.ApplicantUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InternshipApplications_Internships_InternshipId",
                        column: x => x.InternshipId,
                        principalTable: "Internships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicantUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    AIAnalysis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AIScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    AnalyzedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApplicantEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ApplicantName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ApplicantPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AppliedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyNotes = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    CompanyRating = table.Column<int>(type: "int", nullable: true),
                    CoverLetter = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    EducationScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    ExperienceScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    ExtractedExperience = table.Column<int>(type: "int", nullable: true),
                    ExtractedSkills = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GitHubUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HasBeenContacted = table.Column<bool>(type: "bit", nullable: false),
                    InterviewLocation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InterviewNotes = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    InterviewScheduledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InterviewScore = table.Column<int>(type: "int", nullable: true),
                    InterviewType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsFavorite = table.Column<bool>(type: "bit", nullable: false),
                    LastContactedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LinkedInUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MatchPercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    MessagesCount = table.Column<int>(type: "int", nullable: false),
                    PortfolioUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReferredBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ResumeFileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ResumeText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResumeUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShortlistedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SkillsScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    Source = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    StatusUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobApplications_AspNetUsers_ApplicantUserId",
                        column: x => x.ApplicantUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobApplications_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfiles_Industry",
                table: "CompanyProfiles",
                column: "Industry");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfiles_IsActive",
                table: "CompanyProfiles",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipApplications_AIScore",
                table: "InternshipApplications",
                column: "AIScore");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipApplications_ApplicantUserId",
                table: "InternshipApplications",
                column: "ApplicantUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipApplications_AppliedAt",
                table: "InternshipApplications",
                column: "AppliedAt");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipApplications_InternshipId_ApplicantUserId",
                table: "InternshipApplications",
                columns: new[] { "InternshipId", "ApplicantUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternshipApplications_IsFavorite",
                table: "InternshipApplications",
                column: "IsFavorite");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipApplications_Major",
                table: "InternshipApplications",
                column: "Major");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipApplications_MatchPercentage",
                table: "InternshipApplications",
                column: "MatchPercentage");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipApplications_Status",
                table: "InternshipApplications",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipApplications_University",
                table: "InternshipApplications",
                column: "University");

            migrationBuilder.CreateIndex(
                name: "IX_Internships_ApplicationDeadline",
                table: "Internships",
                column: "ApplicationDeadline");

            migrationBuilder.CreateIndex(
                name: "IX_Internships_Category",
                table: "Internships",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Internships_City",
                table: "Internships",
                column: "City");

            migrationBuilder.CreateIndex(
                name: "IX_Internships_CompanyProfileId",
                table: "Internships",
                column: "CompanyProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Internships_Country",
                table: "Internships",
                column: "Country");

            migrationBuilder.CreateIndex(
                name: "IX_Internships_InternshipType",
                table: "Internships",
                column: "InternshipType");

            migrationBuilder.CreateIndex(
                name: "IX_Internships_IsFeatured",
                table: "Internships",
                column: "IsFeatured");

            migrationBuilder.CreateIndex(
                name: "IX_Internships_IsPaid",
                table: "Internships",
                column: "IsPaid");

            migrationBuilder.CreateIndex(
                name: "IX_Internships_IsUrgent",
                table: "Internships",
                column: "IsUrgent");

            migrationBuilder.CreateIndex(
                name: "IX_Internships_PostedAt",
                table: "Internships",
                column: "PostedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Internships_Status",
                table: "Internships",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Internships_WorkMode",
                table: "Internships",
                column: "WorkMode");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_AIScore",
                table: "JobApplications",
                column: "AIScore");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_ApplicantUserId",
                table: "JobApplications",
                column: "ApplicantUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_AppliedAt",
                table: "JobApplications",
                column: "AppliedAt");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_IsFavorite",
                table: "JobApplications",
                column: "IsFavorite");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_JobId_ApplicantUserId",
                table: "JobApplications",
                columns: new[] { "JobId", "ApplicantUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_MatchPercentage",
                table: "JobApplications",
                column: "MatchPercentage");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_Status",
                table: "JobApplications",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_Category",
                table: "Jobs",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_City",
                table: "Jobs",
                column: "City");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_CompanyProfileId",
                table: "Jobs",
                column: "CompanyProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_Country",
                table: "Jobs",
                column: "Country");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_ExpiresAt",
                table: "Jobs",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_IsFeatured",
                table: "Jobs",
                column: "IsFeatured");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_IsUrgent",
                table: "Jobs",
                column: "IsUrgent");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_JobType",
                table: "Jobs",
                column: "JobType");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_PostedAt",
                table: "Jobs",
                column: "PostedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_Status",
                table: "Jobs",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_WorkMode",
                table: "Jobs",
                column: "WorkMode");
        }
    }
}
