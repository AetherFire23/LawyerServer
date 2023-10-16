//namespace ProcedureMakerServer.Interfaces;

using EFCoreBase.Entities;
using EFCoreBase.Interfaces;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer;

public interface IProcedureRepositoryBase<T> : ICrudRepositoryBase<ProcedureContext, T>
    where T : EntityBase
{
}
