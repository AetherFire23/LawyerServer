using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Interfaces;
using ProcedureMakerServer.Repository.ProcedureRepo;

namespace ProcedureMakerServer.Repository;

public class LawyerRepository : ProcedureCrudBase<Lawyer>, ILawyerRepository
{
    private IIncludableQueryable<Lawyer, List<Client>> SetWithIncludes => base.Set
            .Include(l => l.Cases)
            .Include(l => l.Clients);

    public LawyerRepository(ProcedureContext context, IMapper mapper) : base(context, mapper)
    {

    }

    public override async Task<Lawyer> GetEntityById(Guid id)
    {
        var lawyer = await SetWithIncludes
            .FirstOrDefaultAsync(l => l.Id == id);

        return lawyer;
    }

    public async Task<Lawyer> GetLawyerFromUserId(Guid userId)
    {
        Lawyer lawyer = await SetWithIncludes
            .FirstAsync(l => l.UserId == userId);
        return lawyer;
    }

    // no real update atm
    public async Task ModifyLawyer(Lawyer lawyer)
    {
        var entity = await GetEntityById(lawyer.Id);
        entity.CourtRole = lawyer.CourtRole;
        entity.Address = entity.Address;
        entity.City = lawyer.City;
        entity.Country = lawyer.Country;
        entity.LastName = lawyer.LastName;
        entity.FirstName = lawyer.FirstName;
        entity.DateOfBirth = lawyer.DateOfBirth;

        // Do i really need a profile - yep
        // proceduralement creer les profiles?
        //Mapper.Map(lawyer, entity); // new goes into the old
        // _mapper.Map<Lawyer>(entity);
        // can also already pass the existing object
        await Context.SaveChangesAsync();
    }
}
