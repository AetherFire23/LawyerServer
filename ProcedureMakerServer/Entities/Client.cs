using EFCoreBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcedureMakerServer.Entities.BaseEntities;
using Reinforced.Typings.Attributes;

namespace ProcedureMakerServer.Entities;

[TsClass]
public class Client : CourtMemberBase
{
    public Guid LawyerId { get; set; }
    public Lawyer Lawyer { get; set; }

    public ICollection<Case> Cases { get; set; } = new List<Case>();

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
