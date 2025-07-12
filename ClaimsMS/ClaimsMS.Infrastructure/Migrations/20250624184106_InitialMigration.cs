using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClaimsMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    ClaimId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimAuctionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimDescription = table.Column<string>(type: "text", nullable: false),
                    ClaimReason = table.Column<string>(type: "text", nullable: false),
                    StatusClaim = table.Column<string>(type: "text", nullable: false),
                    ClaimEvidence = table.Column<string>(type: "text", nullable: true),
                    ResolutionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.ClaimId);
                });

            migrationBuilder.CreateTable(
                name: "Resolutions",
                columns: table => new
                {
                    ResolutionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResolutionDescription = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resolutions", x => x.ResolutionId);
                    table.ForeignKey(
                        name: "FK_Resolutions_Claims_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "Claims",
                        principalColumn: "ClaimId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Claims_ResolutionId",
                table: "Claims",
                column: "ResolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_ClaimId",
                table: "Resolutions",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_ResolutionId",
                table: "Resolutions",
                column: "ResolutionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_Resolutions_ResolutionId",
                table: "Claims",
                column: "ResolutionId",
                principalTable: "Resolutions",
                principalColumn: "ResolutionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_Resolutions_ResolutionId",
                table: "Claims");

            migrationBuilder.DropTable(
                name: "Resolutions");

            migrationBuilder.DropTable(
                name: "Claims");
        }
    }
}
