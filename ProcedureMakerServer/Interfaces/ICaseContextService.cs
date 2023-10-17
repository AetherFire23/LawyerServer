using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.Models;

namespace ProcedureMakerServer.Interfaces;

public interface ICaseContextService
{
    Task CreateNewCase(CaseCreationInfo creationInfo);
    Task<CasesContext> GetCase(Guid lawyerId);
    Task SaveContextDto(CasesContext caseContext);
}