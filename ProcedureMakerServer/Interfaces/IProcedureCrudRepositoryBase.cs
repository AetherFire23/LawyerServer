//namespace ProcedureMakerServer.Interfaces;

using EFCoreBase.Entities;
using EFCoreBase.Interfaces;
using ProcedureMakerServer;

public interface IProcedureCrudRepositoryBase<T> : ICrudRepositoryBase<ProcedureContext, T>
	where T : EntityBase
{
}
