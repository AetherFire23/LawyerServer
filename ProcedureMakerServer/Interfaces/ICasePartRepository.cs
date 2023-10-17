using ProcedureMakerServer.Entities;

namespace ProcedureMakerServer.Interfaces;

public interface ICasePartRepository : IProcedureCrudRepositoryBase<CasePart>
{
    Task ModifyCasePart(CasePart casePart);
}