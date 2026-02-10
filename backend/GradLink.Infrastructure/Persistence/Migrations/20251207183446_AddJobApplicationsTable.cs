using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GradLink.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddJobApplicationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    ApplicantUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicantName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ApplicantEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ApplicantPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ResumeUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ResumeFileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ResumeText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoverLetter = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    PortfolioUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LinkedInUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GitHubUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    SubStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AIScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    MatchPercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    SkillsScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    ExperienceScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    EducationScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    AIAnalysis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtractedSkills = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtractedExperience = table.Column<int>(type: "int", nullable: true),
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
                    AppliedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShortlistedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReferredBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HasBeenContacted = table.Column<bool>(type: "bit", nullable: false),
                    LastContactedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MessagesCount = table.Column<int>(type: "int", nullable: false)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobApplications");
        }
    }
}
