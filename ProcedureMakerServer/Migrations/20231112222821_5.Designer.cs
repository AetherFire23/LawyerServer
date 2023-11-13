﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ProcedureMakerServer;

#nullable disable

namespace ProcedureMakerServer.Migrations
{
    [DbContext(typeof(ProcedureContext))]
    [Migration("20231112222821_5")]
    partial class _5
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EFCoreBase.Entities.Case", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CaseNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uuid");

                    b.Property<string>("CourtAffairNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CourtNumber")
                        .HasColumnType("integer");

                    b.Property<int>("CourtType")
                        .HasColumnType("integer");

                    b.Property<string>("DistrictName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ManagerLawyerId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("ManagerLawyerId");

                    b.ToTable("Cases");
                });

            modelBuilder.Entity("ProcedureMakerServer.Authentication.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("RoleType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("ProcedureMakerServer.Authentication.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ProcedureMakerServer.Authentication.UserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("ProcedureMakerServer.Billing.AccountStatement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CaseId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CaseId")
                        .IsUnique();

                    b.ToTable("AccountStatements");
                });

            modelBuilder.Entity("ProcedureMakerServer.Billing.Activity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BillingElementId")
                        .HasColumnType("uuid");

                    b.Property<bool>("HasPersonalizedBillingElement")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("InvoiceId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PersonalizedBillingElementId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("TimeWorking")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("BillingElementId");

                    b.HasIndex("InvoiceId");

                    b.HasIndex("PersonalizedBillingElementId");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("ProcedureMakerServer.Billing.BillingElement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ActivityName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsHourlyRate")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("LawyerBillingOptionsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("LawyerId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("LawyerBillingOptionsId");

                    b.HasIndex("LawyerId");

                    b.ToTable("BillingElements");

                    b.HasDiscriminator<string>("Discriminator").HasValue("BillingElement");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("ProcedureMakerServer.Billing.Invoice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountStatementId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AccountStatementId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("ProcedureMakerServer.Billing.LawyerBillingOptions", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("LawyerId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("TPS")
                        .HasColumnType("numeric");

                    b.Property<decimal>("TVQ")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("LawyerId")
                        .IsUnique();

                    b.ToTable("LawyerBillingOptions");
                });

            modelBuilder.Entity("ProcedureMakerServer.Billing.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("AmountPaid")
                        .HasColumnType("numeric");

                    b.Property<DateTime?>("AmountPaidDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("InvoiceId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("InvoiceId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("ProcedureMakerServer.Entities.CasePart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("CaseId")
                        .HasColumnType("uuid");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CourtRole")
                        .HasColumnType("integer");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Fax")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<bool>("HasJuridicalAid")
                        .HasColumnType("boolean");

                    b.Property<string>("HomePhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MobilePhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NotificationEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PostalCase")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SocialSecurityNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("WorkPhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CaseId");

                    b.ToTable("CaseParts");
                });

            modelBuilder.Entity("ProcedureMakerServer.Entities.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CourtRole")
                        .HasColumnType("integer");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Fax")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<bool>("HasJuridicalAid")
                        .HasColumnType("boolean");

                    b.Property<string>("HomePhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("LawyerId")
                        .HasColumnType("uuid");

                    b.Property<string>("MobilePhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NotificationEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PostalCase")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SocialSecurityNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("WorkPhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LawyerId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("ProcedureMakerServer.Entities.Lawyer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CourtLockerNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CourtRole")
                        .HasColumnType("integer");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Fax")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<bool>("HasJuridicalAid")
                        .HasColumnType("boolean");

                    b.Property<string>("HomePhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MobilePhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NotificationEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PostalCase")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SocialSecurityNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("WorkPhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Lawyers");
                });

            modelBuilder.Entity("ProcedureMakerServer.Billing.PersonalizedBillingElement", b =>
                {
                    b.HasBaseType("ProcedureMakerServer.Billing.BillingElement");

                    b.HasDiscriminator().HasValue("PersonalizedBillingElement");
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
                        .OnDelete(DeleteBehavior.Cascade)
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

            modelBuilder.Entity("ProcedureMakerServer.Billing.AccountStatement", b =>
                {
                    b.HasOne("EFCoreBase.Entities.Case", "Case")
                        .WithOne("AccountStatement")
                        .HasForeignKey("ProcedureMakerServer.Billing.AccountStatement", "CaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Case");
                });

            modelBuilder.Entity("ProcedureMakerServer.Billing.Activity", b =>
                {
                    b.HasOne("ProcedureMakerServer.Billing.BillingElement", "BillingElement")
                        .WithMany()
                        .HasForeignKey("BillingElementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProcedureMakerServer.Billing.Invoice", null)
                        .WithMany("Activities")
                        .HasForeignKey("InvoiceId");

                    b.HasOne("ProcedureMakerServer.Billing.PersonalizedBillingElement", "PersonalizedBillingElement")
                        .WithMany()
                        .HasForeignKey("PersonalizedBillingElementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BillingElement");

                    b.Navigation("PersonalizedBillingElement");
                });

            modelBuilder.Entity("ProcedureMakerServer.Billing.BillingElement", b =>
                {
                    b.HasOne("ProcedureMakerServer.Billing.LawyerBillingOptions", null)
                        .WithMany("BillingElements")
                        .HasForeignKey("LawyerBillingOptionsId");

                    b.HasOne("ProcedureMakerServer.Entities.Lawyer", "Lawyer")
                        .WithMany()
                        .HasForeignKey("LawyerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lawyer");
                });

            modelBuilder.Entity("ProcedureMakerServer.Billing.Invoice", b =>
                {
                    b.HasOne("ProcedureMakerServer.Billing.AccountStatement", "AccountStatement")
                        .WithMany("Invoices")
                        .HasForeignKey("AccountStatementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountStatement");
                });

            modelBuilder.Entity("ProcedureMakerServer.Billing.LawyerBillingOptions", b =>
                {
                    b.HasOne("ProcedureMakerServer.Entities.Lawyer", "Lawyer")
                        .WithOne("LawyerBillingOptions")
                        .HasForeignKey("ProcedureMakerServer.Billing.LawyerBillingOptions", "LawyerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lawyer");
                });

            modelBuilder.Entity("ProcedureMakerServer.Billing.Payment", b =>
                {
                    b.HasOne("ProcedureMakerServer.Billing.Invoice", "Invoice")
                        .WithMany("Payments")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Invoice");
                });

            modelBuilder.Entity("ProcedureMakerServer.Entities.CasePart", b =>
                {
                    b.HasOne("EFCoreBase.Entities.Case", "Case")
                        .WithMany("Participants")
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
                    b.HasOne("ProcedureMakerServer.Authentication.User", "User")
                        .WithOne()
                        .HasForeignKey("ProcedureMakerServer.Entities.Lawyer", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("EFCoreBase.Entities.Case", b =>
                {
                    b.Navigation("AccountStatement")
                        .IsRequired();

                    b.Navigation("Participants");
                });

            modelBuilder.Entity("ProcedureMakerServer.Authentication.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("ProcedureMakerServer.Authentication.User", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("ProcedureMakerServer.Billing.AccountStatement", b =>
                {
                    b.Navigation("Invoices");
                });

            modelBuilder.Entity("ProcedureMakerServer.Billing.Invoice", b =>
                {
                    b.Navigation("Activities");

                    b.Navigation("Payments");
                });

            modelBuilder.Entity("ProcedureMakerServer.Billing.LawyerBillingOptions", b =>
                {
                    b.Navigation("BillingElements");
                });

            modelBuilder.Entity("ProcedureMakerServer.Entities.Client", b =>
                {
                    b.Navigation("Cases");
                });

            modelBuilder.Entity("ProcedureMakerServer.Entities.Lawyer", b =>
                {
                    b.Navigation("Cases");

                    b.Navigation("Clients");

                    b.Navigation("LawyerBillingOptions")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
