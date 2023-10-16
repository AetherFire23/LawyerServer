using EFCoreBase.Entities;
using EFCoreBase.Interfaces;

namespace ProcedureMakerServer.Interfaces;

public interface ICaseRepository : IProcedureRepositoryBase<EFCoreBase.Entities.LawyerFile>
{
}