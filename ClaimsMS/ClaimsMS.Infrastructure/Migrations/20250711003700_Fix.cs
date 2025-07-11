using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClaimsMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClaimDeliveries_Resolutions_ResolutionId",
                table: "ClaimDeliveries");

            migrationBuilder.DropIndex(
                name: "IX_ClaimDeliveries_ResolutionId",
                table: "ClaimDeliveries");

            migrationBuilder.DropColumn(
                name: "ResolutionId",
                table: "ClaimDeliveries");

            migrationBuilder.AddColumn<Guid>(
                name: "ClaimDeliveryId",
                table: "Resolutions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_ClaimDeliveryId",
                table: "Resolutions",
                column: "ClaimDeliveryId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Resolutions_ClaimDeliveries_ClaimDeliveryId",
                table: "Resolutions",
                column: "ClaimDeliveryId",
                principalTable: "ClaimDeliveries",
                principalColumn: "ClaimId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resolutions_ClaimDeliveries_ClaimDeliveryId",
                table: "Resolutions");

            migrationBuilder.DropIndex(
                name: "IX_Resolutions_ClaimDeliveryId",
                table: "Resolutions");

            migrationBuilder.DropColumn(
                name: "ClaimDeliveryId",
                table: "Resolutions");

            migrationBuilder.AddColumn<Guid>(
                name: "ResolutionId",
                table: "ClaimDeliveries",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ClaimDeliveries_ResolutionId",
                table: "ClaimDeliveries",
                column: "ResolutionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimDeliveries_Resolutions_ResolutionId",
                table: "ClaimDeliveries",
                column: "ResolutionId",
                principalTable: "Resolutions",
                principalColumn: "ResolutionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
