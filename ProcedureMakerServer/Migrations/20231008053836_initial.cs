using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProcedureMakerServer.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lawyers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    PostalCode = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    MobilePhoneNumber = table.Column<string>(type: "text", nullable: false),
                    WorkPhoneNumber = table.Column<string>(type: "text", nullable: false),
                    HomePhoneNumber = table.Column<string>(type: "text", nullable: false),
                    HasJuridicalAid = table.Column<bool>(type: "boolean", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SocialSecurityNumber = table.Column<string>(type: "text", nullable: false),
                    CourtRole = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lawyers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserBase",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBase", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LawyerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourtNumber = table.Column<string>(type: "text", nullable: false),
                    FileNumber = table.Column<string>(type: "text", nullable: false),
                    CourtType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_Lawyers_LawyerId",
                        column: x => x.LawyerId,
                        principalTable: "Lawyers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleBase",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    UserBaseId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleBase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleBase_UserBase_UserBaseId",
                        column: x => x.UserBaseId,
                        principalTable: "UserBase",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_RoleBase_RoleId",
                        column: x => x.RoleId,
                        principalTable: "RoleBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_UserBase_UserId",
                        column: x => x.UserId,
                        principalTable: "UserBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Lawyers",
                columns: new[] { "Id", "Address", "City", "Country", "CourtRole", "DateOfBirth", "Email", "FirstName", "HasJuridicalAid", "HomePhoneNumber", "LastName", "MobilePhoneNumber", "PostalCode", "SocialSecurityNumber", "WorkPhoneNumber" },
                values: new object[,]
                {
                    { new Guid("3284ac9d-81b2-4fc0-82fb-47855df39814"), "", "", "", 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "grangerf@master.com", "john", false, "", "granger", "", "", "", "" },
                    { new Guid("c931cc96-4e87-49f6-8037-111041bcc774"), "", "", "", 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "richerf@master.com", "fred", false, "", "richer", "", "", "", "" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_LawyerId",
                table: "Files",
                column: "LawyerId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleBase_UserBaseId",
                table: "RoleBase",
                column: "UserBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Lawyers");

            migrationBuilder.DropTable(
                name: "RoleBase");

            migrationBuilder.DropTable(
                name: "UserBase");
        }
    }
}
