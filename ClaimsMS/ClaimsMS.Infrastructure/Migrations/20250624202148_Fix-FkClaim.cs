using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClaimsMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixFkClaim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_Resolutions_ResolutionId",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Resolutions_ClaimId",
                table: "Resolutions");

            migrationBuilder.DropIndex(
                name: "IX_Resolutions_ResolutionId",
                table: "Resolutions");

            migrationBuilder.DropIndex(
                name: "IX_Claims_ResolutionId",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "ResolutionId",
                table: "Claims");

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_ClaimId",
                table: "Resolutions",
                column: "ClaimId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Resolutions_ClaimId",
                table: "Resolutions");

            migrationBuilder.AddColumn<Guid>(
                name: "ResolutionId",
                table: "Claims",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_ClaimId",
                table: "Resolutions",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_ResolutionId",
                table: "Resolutions",
                column: "ResolutionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Claims_ResolutionId",
                table: "Claims",
                column: "ResolutionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_Resolutions_ResolutionId",
                table: "Claims",
                column: "ResolutionId",
                principalTable: "Resolutions",
                principalColumn: "ResolutionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
