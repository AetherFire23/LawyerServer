using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.Models;

namespace ProcedureMakerServer.Interfaces;

public interface ICaseContextService
{
    Task<GetCaseResponse> CreateNewCase(CaseCreationInfo creationInfo);
    Task<CasesContext> GetCaseContext(Guid lawyerId);
    Task SaveCaseDto(CaseDto caseDto);
}