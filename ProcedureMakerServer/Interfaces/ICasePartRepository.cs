using EFCoreBase.Entities;
using ProcedureMakerServer.Entities;

namespace ProcedureMakerServer.Interfaces;

public interface ICasePartRepository : IProcedureCrudRepositoryBase<CasePart>
{
    Task<List<CasePart>> GetParticipantsForCase(Guid caseId);
    Task ModifyCasePart(CasePart casePart);
    Task CreateOrModifyCasePart(Case cCase, CasePart casePart);
}