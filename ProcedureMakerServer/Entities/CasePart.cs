using EFCoreBase.Entities;
using ProcedureMakerServer.Entities.BaseEntities;
using Reinforced.Typings.Attributes;

namespace ProcedureMakerServer.Entities;

[TsClass]
public class CasePart : CourtMemberBase
{
    public Guid CaseId { get; set; }
    public virtual Case Case { get; set; } = new Case();

    
}
