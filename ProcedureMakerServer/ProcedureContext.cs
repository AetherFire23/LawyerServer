using EFCoreBase.Entities;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Authentication;
using ProcedureMakerServer.Billing;
using ProcedureMakerServer.Entities;
using System.Reflection;

namespace ProcedureMakerServer;

public class ProcedureContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Role> Roles { get; set; }

    public DbSet<Case> Cases { get; set; }
    public DbSet<Lawyer> Lawyers { get; set; }
    public DbSet<CasePart> CaseParts { get; set; }
    public DbSet<Client> Clients { get; set; }

    // billing

    public DbSet<AccountStatement> AccountStatements { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<BillingElement> BillingElements { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<LawyerBillingOptions> LawyerBillingOptions { get; set; }
  
    public DbSet<Payment> Payments { get; set; }


    public ProcedureContext(DbContextOptions<ProcedureContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) // denis says fluent api is for advanced shit
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
