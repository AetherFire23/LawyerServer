using EFCoreBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcedureMakerServer.Authentication;
using ProcedureMakerServer.Entities.BaseEntities;

namespace ProcedureMakerServer.Entities;

public class Lawyer : CourtMemberBase
{
    public Guid UserId { get; set; }
    public User User { get; set; } = new User();


    public ICollection<Case> Cases { get; set; } = new List<Case>();

    public List<Client> Clients
        => Cases is null || !Cases.Any()
        ? new List<Client>()
        : Cases.Select(c => c.Client).ToList();
}


public class CaseConfiguration : IEntityTypeConfiguration<Lawyer>
{
    public void Configure(EntityTypeBuilder<Lawyer> builder)
    {
        builder.HasMany(c => c.Cases)
            .WithOne(p => p.ManagerLawyer)
            .HasForeignKey(p => p.ManagerLawyerId);

        builder.HasMany(c => c.Clients)
            .WithOne(l => l.Lawyer)
            .HasForeignKey(k => k.LawyerId);
    }
}
