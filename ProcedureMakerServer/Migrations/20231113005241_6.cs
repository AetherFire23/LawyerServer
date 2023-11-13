using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProcedureMakerServer.Migrations
{
    /// <inheritdoc />
    public partial class _6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_BillingElements_PersonalizedBillingElementId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_PersonalizedBillingElementId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "BillingElements");

            migrationBuilder.DropColumn(
                name: "PersonalizedBillingElementId",
                table: "Activities");

            migrationBuilder.AddColumn<int>(
                name: "InvoiceStatuses",
                table: "Invoices",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceStatuses",
                table: "Invoices");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "BillingElements",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "PersonalizedBillingElementId",
                table: "Activities",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Activities_PersonalizedBillingElementId",
                table: "Activities",
                column: "PersonalizedBillingElementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_BillingElements_PersonalizedBillingElementId",
                table: "Activities",
                column: "PersonalizedBillingElementId",
                principalTable: "BillingElements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
