using EFCoreBase.Interfaces;
using EFCoreBase.Repositories;
using ProcedureMakerServer.Entities;

namespace ProcedureMakerServer.Interfaces;

public interface ILawyerRepository : IProcedureRepositoryBase<Lawyer>
{
    Task ModifyLawyer(Lawyer lawyer);
}
