using EFCoreBase.Entities;
using ProcedureMakerServer.Entities;

namespace ProcedureMakerServer.Interfaces;

public interface ICasePartRepository : IProcedureCrudRepositoryBase<CasePart>
{
    Task CreateOrModifyCasePart(Case cCase, CasePart casePart);
    Task<List<CasePart>> GetParticipantsForCase(Guid caseId);
    Task ModifyCasePart(CasePart casePart);
}