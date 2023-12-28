using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcedureMakerServer.Billing.StatementEntities;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Enums;

namespace EFCoreBase.Entities;


public class Case : EntityBase
{
    public Guid ManagerLawyerId { get; set; }
    public virtual Lawyer? ManagerLawyer { get; set; }

    public Guid ClientId { get; set; }
    public virtual Client Client { get; set; }

    public virtual AccountStatement? AccountStatement { get; set; }
    public ICollection<CaseParticipant> CaseParticipants { get; set; } = new List<CaseParticipant>();
    public string DistrictName { get; set; } = string.Empty;
    public string CourtAffairNumber { get; set; } = string.Empty;
    public string CaseNumber { get; set; } = string.Empty; // cases can have different filenumbers if it comes again many times 
    public ChamberNames ChamberName { get; set; }
    public int CourtNumber { get; set; }
}

public class CaseConfiguration : IEntityTypeConfiguration<Case>
{
    public void Configure(EntityTypeBuilder<Case> builder)
    {
        _ = builder.HasMany(p => p.CaseParticipants)
            .WithOne(p => p.Case)
            .HasForeignKey(k => k.CaseId);
    }
}
