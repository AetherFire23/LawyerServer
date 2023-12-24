using ProcedureMakerServer.Dtos;

namespace ProcedureMakerServer.Interfaces;

public interface ICasesContextRepository
{
    Task<CasesContext> MapCasesContext(Guid userId);
}