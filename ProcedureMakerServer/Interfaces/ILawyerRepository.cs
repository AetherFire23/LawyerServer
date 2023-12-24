using ProcedureMakerServer.Entities;

namespace ProcedureMakerServer.Interfaces;

public interface ILawyerRepository : IProcedureCrudRepositoryBase<Lawyer>
{
    Task<Lawyer> GetLawyerFromUserId(Guid userId);
    Task UpdateLawyer(Lawyer lawyer);
}
