using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClaimsMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ClaimDelivery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClaimDeliveries",
                columns: table => new
                {
                    ClaimId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimDeliveryId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_ClaimDeliveries", x => x.ClaimId);
                    table.ForeignKey(
                        name: "FK_ClaimDeliveries_Resolutions_ResolutionId",
                        column: x => x.ResolutionId,
                        principalTable: "Resolutions",
                        principalColumn: "ResolutionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClaimDeliveries_ResolutionId",
                table: "ClaimDeliveries",
                column: "ResolutionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClaimDeliveries");
        }
    }
}
