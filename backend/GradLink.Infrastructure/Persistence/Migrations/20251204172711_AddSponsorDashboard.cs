using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GradLink.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSponsorDashboard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SponsorProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CompanyDescription = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Industry = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContactPersonName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ContactPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FieldsOfInterest = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TotalBudget = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    RemainingBudget = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    TotalProjectsSponsored = table.Column<int>(type: "int", nullable: false),
                    ActiveProjects = table.Column<int>(type: "int", nullable: false),
                    ApprovedProjects = table.Column<int>(type: "int", nullable: false),
                    CompletedProjects = table.Column<int>(type: "int", nullable: false),
                    PendingRequests = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SponsorProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SponsorProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SponsoredProjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ProjectDescription = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    StudentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TeamName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pending"),
                    RejectionReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FundingAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    FundingDelivered = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ProgressPercentage = table.Column<int>(type: "int", nullable: false),
                    CurrentMilestone = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Milestones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Documents = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SponsorProfileId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    StudentUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SponsoredProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SponsoredProjects_AspNetUsers_StudentUserId",
                        column: x => x.StudentUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SponsoredProjects_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SponsoredProjects_SponsorProfiles_SponsorProfileId",
                        column: x => x.SponsorProfileId,
                        principalTable: "SponsorProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SponsorFundings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Payment"),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pending"),
                    TransactionReference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SponsorProfileId = table.Column<int>(type: "int", nullable: false),
                    SponsoredProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SponsorFundings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SponsorFundings_SponsorProfiles_SponsorProfileId",
                        column: x => x.SponsorProfileId,
                        principalTable: "SponsorProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SponsorFundings_SponsoredProjects_SponsoredProjectId",
                        column: x => x.SponsoredProjectId,
                        principalTable: "SponsoredProjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SponsorMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    Attachments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReceiverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsSponsorSender = table.Column<bool>(type: "bit", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SponsorProfileId = table.Column<int>(type: "int", nullable: false),
                    SponsoredProjectId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SponsorMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SponsorMessages_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SponsorMessages_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SponsorMessages_SponsorProfiles_SponsorProfileId",
                        column: x => x.SponsorProfileId,
                        principalTable: "SponsorProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SponsorMessages_SponsoredProjects_SponsoredProjectId",
                        column: x => x.SponsoredProjectId,
                        principalTable: "SponsoredProjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SponsoredProjects_ProjectId",
                table: "SponsoredProjects",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SponsoredProjects_SponsorProfileId",
                table: "SponsoredProjects",
                column: "SponsorProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_SponsoredProjects_Status",
                table: "SponsoredProjects",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_SponsoredProjects_StudentUserId",
                table: "SponsoredProjects",
                column: "StudentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SponsorFundings_SponsoredProjectId",
                table: "SponsorFundings",
                column: "SponsoredProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SponsorFundings_SponsorProfileId",
                table: "SponsorFundings",
                column: "SponsorProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_SponsorMessages_ReceiverId",
                table: "SponsorMessages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_SponsorMessages_SenderId",
                table: "SponsorMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_SponsorMessages_SentAt",
                table: "SponsorMessages",
                column: "SentAt");

            migrationBuilder.CreateIndex(
                name: "IX_SponsorMessages_SponsoredProjectId",
                table: "SponsorMessages",
                column: "SponsoredProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SponsorMessages_SponsorProfileId",
                table: "SponsorMessages",
                column: "SponsorProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_SponsorProfiles_Industry",
                table: "SponsorProfiles",
                column: "Industry");

            migrationBuilder.CreateIndex(
                name: "IX_SponsorProfiles_UserId",
                table: "SponsorProfiles",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SponsorFundings");

            migrationBuilder.DropTable(
                name: "SponsorMessages");

            migrationBuilder.DropTable(
                name: "SponsoredProjects");

            migrationBuilder.DropTable(
                name: "SponsorProfiles");
        }
    }
}
