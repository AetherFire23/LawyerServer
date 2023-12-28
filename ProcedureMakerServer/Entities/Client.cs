using EFCoreBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcedureMakerServer.Entities.BaseEntities;
using ProcedureMakerServer.Trusts;
namespace ProcedureMakerServer.Entities;

public class Client : CourtMemberBase
{
    public Guid LawyerId { get; set; } 
    public Lawyer Lawyer { get; set; } 

    // One-to-one relationship (? indicates it can be null)
    public TrustClientCard? TrustClientCard { get; set; } 
    public virtual ICollection<Case> Cases { get; set; } = new List<Case>(); 
}
public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        _ = builder.HasMany(c => c.Cases)
            .WithOne(p => p.Client)
            .HasForeignKey(p => p.ClientId);
    }
}
