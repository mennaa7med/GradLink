using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GradLink.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddInternshipsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Internships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    Requirements = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    Responsibilities = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    LearningOutcomes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    InternshipType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WorkMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DurationWeeks = table.Column<int>(type: "int", nullable: true),
                    DurationMonths = table.Column<int>(type: "int", nullable: true),
                    MinDurationWeeks = table.Column<int>(type: "int", nullable: true),
                    MaxDurationWeeks = table.Column<int>(type: "int", nullable: true),
                    HoursPerWeek = table.Column<int>(type: "int", nullable: true),
                    Schedule = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsStartDateFlexible = table.Column<bool>(type: "bit", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    StipendAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    StipendMax = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    StipendCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "USD"),
                    StipendPeriod = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Monthly"),
                    ShowStipend = table.Column<bool>(type: "bit", nullable: false),
                    Benefits = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ProvidesCertificate = table.Column<bool>(type: "bit", nullable: false),
                    HasFullTimeOpportunity = table.Column<bool>(type: "bit", nullable: false),
                    TrainingProvided = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    HasMentorship = table.Column<bool>(type: "bit", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Skills = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PreferredSkills = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    EducationLevel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FieldOfStudy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AcademicYear = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MinGPA = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: true),
                    LanguageRequirements = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TotalSlots = table.Column<int>(type: "int", nullable: false),
                    AvailableSlots = table.Column<int>(type: "int", nullable: false),
                    FilledSlots = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Draft"),
                    PostedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApplicationDeadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ViewsCount = table.Column<int>(type: "int", nullable: false),
                    ApplicationsCount = table.Column<int>(type: "int", nullable: false),
                    AllowExternalApplications = table.Column<bool>(type: "bit", nullable: false),
                    ExternalApplicationUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ApplicationEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ApplicationInstructions = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    SelectionProcess = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ResponseTime = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: false),
                    CompanyProfileId = table.Column<int>(type: "int", nullable: false)
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
                name: "InternshipApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InternshipId = table.Column<int>(type: "int", nullable: false),
                    ApplicantUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicantName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ApplicantEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ApplicantPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    University = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Major = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AcademicYear = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExpectedGraduation = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GPA = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: true),
                    ResumeUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ResumeFileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CoverLetter = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    PortfolioUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LinkedInUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GitHubUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AdditionalDocuments = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    WhyInterested = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    RelevantExperience = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Availability = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PreferredStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    SubStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    WithdrawalReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AIScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    MatchPercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    SkillsScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    AcademicScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    AIAnalysis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtractedSkills = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnalyzedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompanyRating = table.Column<int>(type: "int", nullable: true),
                    CompanyNotes = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsFavorite = table.Column<bool>(type: "bit", nullable: false),
                    InterviewScheduledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InterviewType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InterviewLocation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InterviewNotes = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    InterviewScore = table.Column<int>(type: "int", nullable: true),
                    OfferedStipend = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    OfferedStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OfferDeadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OfferExtendedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OfferAcceptedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AppliedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShortlistedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HasBeenContacted = table.Column<bool>(type: "bit", nullable: false),
                    LastContactedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MessagesCount = table.Column<int>(type: "int", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReferredBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InternshipApplications");

            migrationBuilder.DropTable(
                name: "Internships");
        }
    }
}
