using AutoMapper;
using EFCoreBase.Repositories;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Interfaces;
using ProcedureMakerServer.Repository.ProcedureRepo;

namespace ProcedureMakerServer.Repository;

public class LawyerRepository : ProcedureCrudBase<Lawyer>, ILawyerRepository
{
    public LawyerRepository(ProcedureContext context, IMapper mapper) : base(context, mapper)
    {

    }

    public async Task ModifyLawyer(Lawyer lawyer)
    {
        var entity = await GetEntityById(lawyer.Id);
        // Do i really need a profile - yep
        // proceduralement creer les profiles?
        _mapper.Map(lawyer, entity);
        // _mapper.Map<Lawyer>(entity);
        // can also already pass the existing object
        await Context.SaveChangesAsync();
    }
}
