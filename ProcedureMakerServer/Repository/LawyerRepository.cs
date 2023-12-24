using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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
        Lawyer? lawyer = await Set
            .Include(l => l.DefaultHourlyElement)
            .Include(l => l.Cases)
            .Include(l => l.User)
            .Include(l => l.Clients)
                .ThenInclude(c => c.TrustClientCard)
            .FirstOrDefaultAsync(l => l.Id == id);

        return lawyer;
    }

    public async Task<Lawyer> GetLawyerFromUserId(Guid userId)
    {
        Lawyer lawyer = await this.Set
            .Include(l => l.Cases)
            .FirstAsync(l => l.UserId == userId);
        return lawyer;
    }

    public async Task UpdateLawyer(Lawyer updatedLawyer)
    {
        Lawyer entity = await GetEntityById(updatedLawyer.Id);

        entity.CopyFromCourtMember(updatedLawyer);

        entity.CourtLockerNumber = updatedLawyer.CourtLockerNumber;
        entity.BaseHourlyRate = updatedLawyer.BaseHourlyRate;

        await Context.SaveChangesAsync();
    }
}
