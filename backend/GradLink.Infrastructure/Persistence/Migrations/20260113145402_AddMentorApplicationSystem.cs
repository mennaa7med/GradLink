using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GradLink.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMentorApplicationSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MentorApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Specialization = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    YearsOfExperience = table.Column<int>(type: "int", nullable: false),
                    LinkedInUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CurrentPosition = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Company = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RetryAllowedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TestAttempts = table.Column<int>(type: "int", nullable: false),
                    FinalScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MentorApplications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TestQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    OptionA = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OptionB = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OptionC = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OptionD = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Difficulty = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Medium"),
                    Explanation = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestQuestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MentorTests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TimeLimitMinutes = table.Column<int>(type: "int", nullable: false),
                    TotalQuestions = table.Column<int>(type: "int", nullable: false),
                    CorrectAnswers = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    QuestionIds = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SubmittedAnswers = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MentorTests_MentorApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "MentorApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MentorApplications_CreatedAt",
                table: "MentorApplications",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_MentorApplications_Email",
                table: "MentorApplications",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_MentorApplications_Status",
                table: "MentorApplications",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_MentorApplications_UserId",
                table: "MentorApplications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorTests_ApplicationId",
                table: "MentorTests",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorTests_ExpiresAt",
                table: "MentorTests",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_MentorTests_Status",
                table: "MentorTests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_MentorTests_Token",
                table: "MentorTests",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestQuestions_Category",
                table: "TestQuestions",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_TestQuestions_Difficulty",
                table: "TestQuestions",
                column: "Difficulty");

            migrationBuilder.CreateIndex(
                name: "IX_TestQuestions_IsActive",
                table: "TestQuestions",
                column: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MentorTests");

            migrationBuilder.DropTable(
                name: "TestQuestions");

            migrationBuilder.DropTable(
                name: "MentorApplications");
        }
    }
}
