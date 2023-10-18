using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Interfaces;
using ProcedureMakerServer.Repository.ProcedureRepo;

namespace ProcedureMakerServer.Repository;

public class LawyerRepository : ProcedureCrudBase<Lawyer>, ILawyerRepository
{
    public LawyerRepository(ProcedureContext context, IMapper mapper) : base(context, mapper)
    {

    }

    public override async Task<Lawyer> GetEntityById(Guid id)
    {
        var lawyer = await base.Set
            .Include(l => l.Cases)
            .Include(l => l.Clients)
            .FirstOrDefaultAsync(x => x.Id == id);

        return lawyer;
    }


    // no real update atm
    public async Task ModifyLawyer(Lawyer lawyer)
    {
        var entity = await GetEntityById(lawyer.Id);

        
        // Do i really need a profile - yep
        // proceduralement creer les profiles?
        //Mapper.Map(lawyer, entity); // new goes into the old
        // _mapper.Map<Lawyer>(entity);
        // can also already pass the existing object
        await Context.SaveChangesAsync();
    }
}
