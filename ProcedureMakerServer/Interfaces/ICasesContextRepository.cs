using ProcedureMakerServer.Dtos;

namespace ProcedureMakerServer.Interfaces;

public interface ICasesContextRepository
{
    Task<CaseContextDto> MapCasesContext(Guid userId);
}