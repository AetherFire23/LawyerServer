using EFCoreBase.Entities;
using ProcedureMakerServer.Entities.BaseEntities;

namespace ProcedureMakerServer.Entities;


public class CaseParticipant : CourtMemberBase
{
    public Guid CaseId { get; set; }
    public virtual Case Case { get; set; } = new Case();

    public CaseParticipantDto ToDto()
    {
        var caseParticipant = new CaseParticipantDto();

        caseParticipant.Id = this.Id;
        caseParticipant.CopyFromCourtMember(this);

        return caseParticipant;
    }
}
