using EFCoreBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcedureMakerServer.Entities.BaseEntities;

namespace ProcedureMakerServer.Entities;

public class Client : CourtMemberBase
{
    public Guid LawyerId { get; set; }
    public Lawyer Lawyer { get; set; } 
    public ICollection<Case> Cases { get; set; } = new List<Case>();
    // client can have many open-closed cases over time

}
public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasMany(c => c.Cases)
            .WithOne(p => p.Client)
            .HasForeignKey(p => p.ClientId);

   
    }
}
