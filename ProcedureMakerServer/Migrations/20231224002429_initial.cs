using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProcedureMakerServer.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HashedPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountStatements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountStatements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BillingElementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BillingElements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ManagerLawyerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsHourlyRate = table.Column<bool>(type: "bit", nullable: false),
                    IsDisburse = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingElements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountStatementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceStatus = table.Column<int>(type: "int", nullable: false),
                    DefaultBillingElementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_AccountStatements_AccountStatementId",
                        column: x => x.AccountStatementId,
                        principalTable: "AccountStatements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_BillingElements_DefaultBillingElementId",
                        column: x => x.DefaultBillingElementId,
                        principalTable: "BillingElements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lawyers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourtLockerNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaseHourlyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DefaultHourlyRateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DefaultHourlyElementId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobilePhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomePhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasJuridicalAid = table.Column<bool>(type: "bit", nullable: false),
                    PostalCase = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SocialSecurityNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotificationEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourtRole = table.Column<int>(type: "int", nullable: false),
                    MustNotify = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lawyers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lawyers_BillingElements_DefaultHourlyElementId",
                        column: x => x.DefaultHourlyElementId,
                        principalTable: "BillingElements",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Lawyers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoicePayments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountPaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPaymentComingFromTrust = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoicePayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoicePayments_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LawyerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobilePhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomePhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasJuridicalAid = table.Column<bool>(type: "bit", nullable: false),
                    PostalCase = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SocialSecurityNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotificationEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourtRole = table.Column<int>(type: "int", nullable: false),
                    MustNotify = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_Lawyers_LawyerId",
                        column: x => x.LawyerId,
                        principalTable: "Lawyers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ManagerLawyerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DistrictName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourtAffairNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChamberName = table.Column<int>(type: "int", nullable: false),
                    CourtNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cases_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cases_Lawyers_ManagerLawyerId",
                        column: x => x.ManagerLawyerId,
                        principalTable: "Lawyers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TrustClientCards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrustClientCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrustClientCards_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseParticipants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobilePhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomePhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasJuridicalAid = table.Column<bool>(type: "bit", nullable: false),
                    PostalCase = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SocialSecurityNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotificationEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourtRole = table.Column<int>(type: "int", nullable: false),
                    MustNotify = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseParticipants_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrustPayments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrustId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrustPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrustPayments_TrustClientCards_TrustId",
                        column: x => x.TrustId,
                        principalTable: "TrustClientCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountStatements_CaseId",
                table: "AccountStatements",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_BillingElementId",
                table: "Activities",
                column: "BillingElementId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_InvoiceId",
                table: "Activities",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_BillingElements_ManagerLawyerId",
                table: "BillingElements",
                column: "ManagerLawyerId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseParticipants_CaseId",
                table: "CaseParticipants",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_ClientId",
                table: "Cases",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_ManagerLawyerId",
                table: "Cases",
                column: "ManagerLawyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_LawyerId",
                table: "Clients",
                column: "LawyerId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoicePayments_InvoiceId",
                table: "InvoicePayments",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_AccountStatementId",
                table: "Invoices",
                column: "AccountStatementId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_DefaultBillingElementId",
                table: "Invoices",
                column: "DefaultBillingElementId");

            migrationBuilder.CreateIndex(
                name: "IX_Lawyers_DefaultHourlyElementId",
                table: "Lawyers",
                column: "DefaultHourlyElementId");

            migrationBuilder.CreateIndex(
                name: "IX_Lawyers_UserId",
                table: "Lawyers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrustClientCards_ClientId",
                table: "TrustClientCards",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrustPayments_TrustId",
                table: "TrustPayments",
                column: "TrustId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountStatements_Cases_CaseId",
                table: "AccountStatements",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_BillingElements_BillingElementId",
                table: "Activities",
                column: "BillingElementId",
                principalTable: "BillingElements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Invoices_InvoiceId",
                table: "Activities",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BillingElements_Lawyers_ManagerLawyerId",
                table: "BillingElements",
                column: "ManagerLawyerId",
                principalTable: "Lawyers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lawyers_BillingElements_DefaultHourlyElementId",
                table: "Lawyers");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "CaseParticipants");

            migrationBuilder.DropTable(
                name: "InvoicePayments");

            migrationBuilder.DropTable(
                name: "TrustPayments");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "TrustClientCards");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "AccountStatements");

            migrationBuilder.DropTable(
                name: "Cases");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "BillingElements");

            migrationBuilder.DropTable(
                name: "Lawyers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
