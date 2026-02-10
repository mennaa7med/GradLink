using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GradLink.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEnhancedMaterials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "DownloadLink",
                table: "Materials");

            migrationBuilder.RenameColumn(
                name: "Downloads",
                table: "Materials",
                newName: "RatingCount");

            migrationBuilder.AlterColumn<string>(
                name: "Link",
                table: "Materials",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Field",
                table: "Materials",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Materials",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Author",
                table: "Materials",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AverageRating",
                table: "Materials",
                type: "float(3)",
                precision: 3,
                scale: 2,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "DifficultyLevel",
                table: "Materials",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DownloadCount",
                table: "Materials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "Materials",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EstimatedTimeMinutes",
                table: "Materials",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Materials",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Materials",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ReviewNotes",
                table: "Materials",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReviewedAt",
                table: "Materials",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewedById",
                table: "Materials",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Materials",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "pending");

            migrationBuilder.AddColumn<string>(
                name: "SubmittedById",
                table: "Materials",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Materials",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Materials",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MaterialCollections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CoverImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "personal"),
                    Field = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DifficultyLevel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EstimatedTimeMinutes = table.Column<int>(type: "int", nullable: true),
                    IsSystemPath = table.Column<bool>(type: "bit", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialCollections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialCollections_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    Position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Color = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialNotes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialNotes_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "not_started"),
                    ProgressPercent = table.Column<int>(type: "int", nullable: false),
                    TimeSpentMinutes = table.Column<int>(type: "int", nullable: false),
                    LastPosition = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsBookmarked = table.Column<bool>(type: "bit", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastAccessedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialProgresses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialProgresses_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    ReviewText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    HelpfulCount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialRatings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialRatings_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollectionFollowers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CollectionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProgressPercent = table.Column<int>(type: "int", nullable: false),
                    FollowedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionFollowers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollectionFollowers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CollectionFollowers_MaterialCollections_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "MaterialCollections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollectionMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CollectionId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsPrerequisite = table.Column<bool>(type: "bit", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollectionMaterials_MaterialCollections_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "MaterialCollections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollectionMaterials_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Materials_CreatedAt",
                table: "Materials",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_IsFeatured",
                table: "Materials",
                column: "IsFeatured");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_ReviewedById",
                table: "Materials",
                column: "ReviewedById");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_Status",
                table: "Materials",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_SubmittedById",
                table: "Materials",
                column: "SubmittedById");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionFollowers_CollectionId_UserId",
                table: "CollectionFollowers",
                columns: new[] { "CollectionId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CollectionFollowers_UserId",
                table: "CollectionFollowers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionMaterials_CollectionId_MaterialId",
                table: "CollectionMaterials",
                columns: new[] { "CollectionId", "MaterialId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CollectionMaterials_MaterialId",
                table: "CollectionMaterials",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionMaterials_Order",
                table: "CollectionMaterials",
                column: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialCollections_IsPublic",
                table: "MaterialCollections",
                column: "IsPublic");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialCollections_IsSystemPath",
                table: "MaterialCollections",
                column: "IsSystemPath");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialCollections_OwnerId",
                table: "MaterialCollections",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialCollections_Type",
                table: "MaterialCollections",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialNotes_CreatedAt",
                table: "MaterialNotes",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialNotes_MaterialId_UserId",
                table: "MaterialNotes",
                columns: new[] { "MaterialId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialNotes_UserId",
                table: "MaterialNotes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialProgresses_IsBookmarked",
                table: "MaterialProgresses",
                column: "IsBookmarked");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialProgresses_LastAccessedAt",
                table: "MaterialProgresses",
                column: "LastAccessedAt");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialProgresses_MaterialId_UserId",
                table: "MaterialProgresses",
                columns: new[] { "MaterialId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaterialProgresses_Status",
                table: "MaterialProgresses",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialProgresses_UserId",
                table: "MaterialProgresses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialRatings_CreatedAt",
                table: "MaterialRatings",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialRatings_MaterialId_UserId",
                table: "MaterialRatings",
                columns: new[] { "MaterialId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaterialRatings_Rating",
                table: "MaterialRatings",
                column: "Rating");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialRatings_UserId",
                table: "MaterialRatings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_AspNetUsers_ReviewedById",
                table: "Materials",
                column: "ReviewedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_AspNetUsers_SubmittedById",
                table: "Materials",
                column: "SubmittedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_AspNetUsers_ReviewedById",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_AspNetUsers_SubmittedById",
                table: "Materials");

            migrationBuilder.DropTable(
                name: "CollectionFollowers");

            migrationBuilder.DropTable(
                name: "CollectionMaterials");

            migrationBuilder.DropTable(
                name: "MaterialNotes");

            migrationBuilder.DropTable(
                name: "MaterialProgresses");

            migrationBuilder.DropTable(
                name: "MaterialRatings");

            migrationBuilder.DropTable(
                name: "MaterialCollections");

            migrationBuilder.DropIndex(
                name: "IX_Materials_CreatedAt",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Materials_IsFeatured",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Materials_ReviewedById",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Materials_Status",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Materials_SubmittedById",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "DifficultyLevel",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "DownloadCount",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "EstimatedTimeMinutes",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "ReviewNotes",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "ReviewedAt",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "ReviewedById",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "SubmittedById",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Materials");

            migrationBuilder.RenameColumn(
                name: "RatingCount",
                table: "Materials",
                newName: "Downloads");

            migrationBuilder.AlterColumn<string>(
                name: "Link",
                table: "Materials",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Field",
                table: "Materials",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Materials",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "Author",
                table: "Materials",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "Materials",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DownloadLink",
                table: "Materials",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
