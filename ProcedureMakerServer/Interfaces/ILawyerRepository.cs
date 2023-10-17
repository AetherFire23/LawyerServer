using ProcedureMakerServer.Entities;

namespace ProcedureMakerServer.Interfaces;

public interface ILawyerRepository : IProcedureCrudRepositoryBase<Lawyer>
{
    Task ModifyLawyer(Lawyer lawyer);
}
