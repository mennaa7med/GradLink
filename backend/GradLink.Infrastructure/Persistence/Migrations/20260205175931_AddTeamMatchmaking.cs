using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GradLink.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTeamMatchmaking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeamRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SkillsNeeded = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MembersNeeded = table.Column<int>(type: "int", nullable: false),
                    CurrentMembers = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Active"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamRequests_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JoinRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamRequestId = table.Column<int>(type: "int", nullable: false),
                    ApplicantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicantName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ApplicantEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ApplicantSkills = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pending"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoinRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JoinRequests_AspNetUsers_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JoinRequests_TeamRequests_TeamRequestId",
                        column: x => x.TeamRequestId,
                        principalTable: "TeamRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JoinRequests_ApplicantId",
                table: "JoinRequests",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinRequests_Status",
                table: "JoinRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_JoinRequests_TeamRequestId",
                table: "JoinRequests",
                column: "TeamRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinRequests_TeamRequestId_ApplicantId",
                table: "JoinRequests",
                columns: new[] { "TeamRequestId", "ApplicantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeamRequests_Category",
                table: "TeamRequests",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_TeamRequests_CreatedAt",
                table: "TeamRequests",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TeamRequests_OwnerId",
                table: "TeamRequests",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamRequests_Status",
                table: "TeamRequests",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JoinRequests");

            migrationBuilder.DropTable(
                name: "TeamRequests");
        }
    }
}
