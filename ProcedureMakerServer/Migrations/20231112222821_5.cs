using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProcedureMakerServer.Migrations
{
    /// <inheritdoc />
    public partial class _5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_AccountStatements_AccountStatementId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Cases_CaseId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_CaseId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "CaseId",
                table: "Invoices");

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountStatementId",
                table: "Invoices",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_AccountStatements_AccountStatementId",
                table: "Invoices",
                column: "AccountStatementId",
                principalTable: "AccountStatements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_AccountStatements_AccountStatementId",
                table: "Invoices");

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountStatementId",
                table: "Invoices",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "CaseId",
                table: "Invoices",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CaseId",
                table: "Invoices",
                column: "CaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_AccountStatements_AccountStatementId",
                table: "Invoices",
                column: "AccountStatementId",
                principalTable: "AccountStatements",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Cases_CaseId",
                table: "Invoices",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
