using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GradLink.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddJobsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    Requirements = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    Responsibilities = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    JobType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WorkMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExperienceLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MinExperience = table.Column<int>(type: "int", nullable: true),
                    MaxExperience = table.Column<int>(type: "int", nullable: true),
                    SalaryMin = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    SalaryMax = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    SalaryCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "USD"),
                    SalaryPeriod = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Monthly"),
                    ShowSalary = table.Column<bool>(type: "bit", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Skills = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PreferredSkills = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    EducationLevel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FieldOfStudy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Benefits = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Draft"),
                    PostedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FilledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ViewsCount = table.Column<int>(type: "int", nullable: false),
                    ApplicationsCount = table.Column<int>(type: "int", nullable: false),
                    Vacancies = table.Column<int>(type: "int", nullable: false),
                    AllowExternalApplications = table.Column<bool>(type: "bit", nullable: false),
                    ExternalApplicationUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ApplicationEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: false),
                    CompanyProfileId = table.Column<int>(type: "int", nullable: false)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jobs");
        }
    }
}
