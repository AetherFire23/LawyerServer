using EFCoreBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcedureMakerServer.Authentication;
using ProcedureMakerServer.Billing.StatementEntities;
using ProcedureMakerServer.Entities.BaseEntities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProcedureMakerServer.Entities;



public class Lawyer : CourtMemberBase
{
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public string CourtLockerNumber { get; set; } = string.Empty;
    public decimal BaseHourlyRate { get; set; } = 100;

    // will this fucking break^
    public Guid DefaultHourlyRateId { get; set; }
    public BillingElement? DefaultHourlyElement { get; set; } // Principal

    public ICollection<BillingElement> BillingElement { get; set; } = new List<BillingElement>();
    public ICollection<Case> Cases { get; set; } = new List<Case>();
    public ICollection<Client> Clients { get; set; } = new List<Client>();


    [JsonIgnore]
    [NotMapped]
    public List<Client> ClientsFromCases
        => Cases is null || !Cases.Any()
        ? new List<Client>()
        : Cases.Select(c => c.Client).ToList();

}


public class CaseConfiguration : IEntityTypeConfiguration<Lawyer>
{
    public void Configure(EntityTypeBuilder<Lawyer> builder)
    {
        _ = builder
            .HasMany(c => c.Cases)
            .WithOne(p => p.ManagerLawyer)
            .HasForeignKey(p => p.ManagerLawyerId)
            .OnDelete(DeleteBehavior.NoAction);

        _ = builder
            .HasMany(c => c.Clients)
            .WithOne(l => l.Lawyer)
            .HasForeignKey(k => k.LawyerId);

        _ = builder
            .HasOne(p => p.User)
            .WithOne()
            .HasForeignKey<Lawyer>(x => x.UserId);


        builder
            .HasMany(x => x.BillingElement)
            .WithOne(x => x.ManagerLawyer)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
