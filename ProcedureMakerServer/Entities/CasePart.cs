using EFCoreBase.Entities;
using ProcedureMakerServer.Entities.BaseEntities;

namespace ProcedureMakerServer.Entities;

public class CasePart : CourtMemberBase
{
    public Guid CaseId { get; set; }
    public virtual Case Case { get; set; } = new Case();
}
