using EFCoreBase.Entities;
using ProcedureMakerServer.Entities;

namespace ProcedureMakerServer.Interfaces;

public interface ICasePartRepository : IProcedureCrudRepositoryBase<CaseParticipant>
{
    Task<List<CaseParticipant>> GetParticipantsForCase(Guid caseId);
    Task ModifyCasePart(CaseParticipant casePart);
    Task CreateOrModifyCasePart(Case cCase, CaseParticipant casePart);
}