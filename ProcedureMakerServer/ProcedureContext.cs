using EFCoreBase.Entities;
using JWTAuth.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcedureMakerServer.Authentication;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Initialization;
namespace ProcedureMakerServer;

public class ProcedureContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Role> Roles { get; set; }
    public ProcedureContext(DbContextOptions<ProcedureContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder) // denis says fluent api is for advanced shit
    {
        modelBuilder.Entity<Lawyer>()
            .HasData(Seeder.SampleLawyers);

        //modelBuilder.Entity<UserRole>()
        //    .HasOne(x => x.Role)
        //    .WithMany(x => x.UserRoles);

        //modelBuilder.Entity<UserRole>()
        //    .HasOne(x => x.User)
        //    .WithMany(x => x.UserRoles);

    }
}
