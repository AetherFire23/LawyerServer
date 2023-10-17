using EFCoreBase.Entities;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Authentication;
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

    public ProcedureContext(DbContextOptions<ProcedureContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder) // denis says fluent api is for advanced shit
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
