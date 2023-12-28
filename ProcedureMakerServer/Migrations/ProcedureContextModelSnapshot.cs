﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProcedureMakerServer;

#nullable disable

namespace ProcedureMakerServer.Migrations
{
    [DbContext(typeof(ProcedureContext))]
    partial class ProcedureContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EFCoreBase.Entities.Case", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CaseNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ChamberName")
                        .HasColumnType("int");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CourtAffairNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CourtNumber")
                        .HasColumnType("int");

                    b.Property<string>("DistrictName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ManagerLawyerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("ManagerLawyerId");

                    b.ToTable("Cases");
                });

            modelBuilder.Entity("ProcedureMakerServer.Authentication.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("RoleType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("ProcedureMakerServer.Authentication.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ProcedureMakerServer.Authentication.UserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("ProcedureMakerServer.Billing.StatementEntities.AccountStatement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CaseId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CaseId")
                        .IsUnique();

                    b.ToTable("AccountStatements");
                });

            modelBuilder.Entity("ProcedureMakerServer.Billing.StatementEntities.Activity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("CostInDollars")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("InvoiceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDisburse")
                        .HasColumnType("bit");

                    b.Property<bool>("IsTaxable")
                        .HasColumnType("bit");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("InvoiceId");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("ProcedureMakerServer.Billing.StatementEntities.BillingElement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ActivityName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("IsDisburse")
                        .HasColumnType("bit");

                    b.Property<bool>("IsHourlyRate")
                        .HasColumnType("bit");

                    b.Property<bool>("IsInvoiceSpecific")
                        .HasColumnType("bit");

                    b.Property<Guid>("ManagerLawyerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SpecificInvoiceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ManagerLawyerId");

                    b.ToTable("BillingElements");
                });

            modelBuilder.Entity("ProcedureMakerServer.Billing.StatementEntities.Invoice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountStatementId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DefaultBillingElementId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("InvoiceStatus")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountStatementId");

                    b.HasIndex("DefaultBillingElementId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("ProcedureMakerServer.Billing.StatementEntities.InvoicePayment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("AmountPaid")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("AmountPaidDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("InvoiceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsPaymentComingFromTrust")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("InvoiceId");

                    b.ToTable("InvoicePayments");
                });

            modelBuilder.Entity("ProcedureMakerServer.Entities.CaseParticipant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CaseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CourtRole")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fax")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<bool>("HasJuridicalAid")
                        .HasColumnType("bit");

                    b.Property<string>("HomePhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MobilePhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("MustNotify")
                        .HasColumnType("bit");

                    b.Property<string>("NotificationEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCase")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SocialSecurityNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkPhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CaseId");

                    b.ToTable("CaseParticipants");
                });

            modelBuilder.Entity("ProcedureMakerServer.Entities.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CourtRole")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fax")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<bool>("HasJuridicalAid")
                        .HasColumnType("bit");

                    b.Property<string>("HomePhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("LawyerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("MobilePhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("MustNotify")
                        .HasColumnType("bit");

                    b.Property<string>("NotificationEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCase")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SocialSecurityNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkPhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LawyerId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("ProcedureMakerServer.Entities.Lawyer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CourtLockerNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CourtRole")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("DefaultHourlyElementId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DefaultHourlyRateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fax")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<bool>("HasJuridicalAid")
                        .HasColumnType("bit");

                    b.Property<string>("HomePhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MobilePhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("MustNotify")
                        .HasColumnType("bit");

                    b.Property<string>("NotificationEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCase")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SocialSecurityNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("WorkPhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DefaultHourlyElementId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Lawyers");
                });

            modelBuilder.Entity("ProcedureMakerServer.Trusts.TrustClientCard", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ClientId")
                        .IsUnique();

                    b.ToTable("TrustClientCards");
                });

            modelBuilder.Entity("ProcedureMakerServer.Trusts.TrustPayment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("TrustId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TrustId");

                    b.ToTable("TrustPayments");
                });

            modelBuilder.Entity("EFCoreBase.Entities.Case", b =>
                {
                    b.HasOne("ProcedureMakerServer.Entities.Client", "Client")
                        .WithMany("Cases")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProcedureMakerServer.Entities.Lawyer", "ManagerLawyer")
                        .WithMany("Cases")
                        .HasForeignKey("ManagerLawyerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("ManagerLawyer");
                });

            modelBuilder.Entity("ProcedureMakerServer.Authentication.UserRole", b =>
                {
                    b.HasOne("ProcedureMakerServer.Authentication.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProcedureMakerServer.Authentication.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProcedureMakerServer.Billing.StatementEntities.AccountStatement", b =>
                {
                    b.HasOne("EFCoreBase.Entities.Case", "Case")
                        .WithOne("AccountStatement")
                        .HasForeignKey("ProcedureMakerServer.Billing.StatementEntities.AccountStatement", "CaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Case");
                });

            modelBuilder.Entity("ProcedureMakerServer.Billing.StatementEntities.Activity", b =>
                {
                    b.HasOne("ProcedureMakerServer.Billing.StatementEntities.Invoice", "Invoice")
                        .WithMany("Activities")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Invoice");
                });

            modelBuilder.Entity("ProcedureMakerServer.Billing.StatementEntities.BillingElement", b =>
                {
                    b.HasOne("ProcedureMakerServer.Entities.Lawyer", "ManagerLawyer")
                        .WithMany("BillingElements")
                        .HasForeignKey("ManagerLawyerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("ManagerLawyer");
                });

            modelBuilder.Entity("ProcedureMakerServer.Billing.StatementEntities.Invoice", b =>
                {
                    b.HasOne("ProcedureMakerServer.Billing.StatementEntities.AccountStatement", "AccountStatement")
                        .WithMany("Invoices")
                        .HasForeignKey("AccountStatementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProcedureMakerServer.Billing.StatementEntities.BillingElement", "DefaultBillingElement")
                        .WithMany()
                        .HasForeignKey("DefaultBillingElementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountStatement");

                    b.Navigation("DefaultBillingElement");
                });

            modelBuilder.Entity("ProcedureMakerServer.Billing.StatementEntities.InvoicePayment", b =>
                {
                    b.HasOne("ProcedureMakerServer.Billing.StatementEntities.Invoice", "Invoice")
                        .WithMany("Payments")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Invoice");
                });

            modelBuilder.Entity("ProcedureMakerServer.Entities.CaseParticipant", b =>
                {
                    b.HasOne("EFCoreBase.Entities.Case", "Case")
                        .WithMany("CaseParticipants")
                        .HasForeignKey("CaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Case");
                });

            modelBuilder.Entity("ProcedureMakerServer.Entities.Client", b =>
                {
                    b.HasOne("ProcedureMakerServer.Entities.Lawyer", "Lawyer")
                        .WithMany("Clients")
                        .HasForeignKey("LawyerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lawyer");
                });

            modelBuilder.Entity("ProcedureMakerServer.Entities.Lawyer", b =>
                {
                    b.HasOne("ProcedureMakerServer.Billing.StatementEntities.BillingElement", "DefaultHourlyElement")
                        .WithMany()
                        .HasForeignKey("DefaultHourlyElementId");

                    b.HasOne("ProcedureMakerServer.Authentication.User", "User")
                        .WithOne()
                        .HasForeignKey("ProcedureMakerServer.Entities.Lawyer", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DefaultHourlyElement");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProcedureMakerServer.Trusts.TrustClientCard", b =>
                {
                    b.HasOne("ProcedureMakerServer.Entities.Client", "Client")
                        .WithOne("TrustClientCard")
                        .HasForeignKey("ProcedureMakerServer.Trusts.TrustClientCard", "ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("ProcedureMakerServer.Trusts.TrustPayment", b =>
                {
                    b.HasOne("ProcedureMakerServer.Trusts.TrustClientCard", "Trust")
                        .WithMany("Payments")
                        .HasForeignKey("TrustId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trust");
                });

            modelBuilder.Entity("EFCoreBase.Entities.Case", b =>
                {
                    b.Navigation("AccountStatement");

                    b.Navigation("CaseParticipants");
                });

            modelBuilder.Entity("ProcedureMakerServer.Authentication.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("ProcedureMakerServer.Authentication.User", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("ProcedureMakerServer.Billing.StatementEntities.AccountStatement", b =>
                {
                    b.Navigation("Invoices");
                });

            modelBuilder.Entity("ProcedureMakerServer.Billing.StatementEntities.Invoice", b =>
                {
                    b.Navigation("Activities");

                    b.Navigation("Payments");
                });

            modelBuilder.Entity("ProcedureMakerServer.Entities.Client", b =>
                {
                    b.Navigation("Cases");

                    b.Navigation("TrustClientCard");
                });

            modelBuilder.Entity("ProcedureMakerServer.Entities.Lawyer", b =>
                {
                    b.Navigation("BillingElements");

                    b.Navigation("Cases");

                    b.Navigation("Clients");
                });

            modelBuilder.Entity("ProcedureMakerServer.Trusts.TrustClientCard", b =>
                {
                    b.Navigation("Payments");
                });
#pragma warning restore 612, 618
        }
    }
}
