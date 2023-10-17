using EFCoreBase.Entities;
using ProcedureMakerServer.Dtos;

namespace ProcedureMakerServer.Interfaces;

public interface ICaseRepository : IProcedureCrudRepositoryBase<Case>
{
    Task<CaseDto> MapCaseDto(Guid caseId);
    Task ModifyCase(Case lcase);
    Task ModifyCaseFromDto(CaseDto caseDto);
}