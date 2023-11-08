using AutoMapper;
using EFCoreBase.Entities;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Interfaces;
using ProcedureMakerServer.Repository.ProcedureRepo;

namespace ProcedureMakerServer.Repository;

public class CasePartRepository : ProcedureCrudBase<CasePart>, ICasePartRepository
{
    public CasePartRepository(ProcedureContext context, IMapper mapper) : base(context, mapper)
    {

    }

    public override async Task<CasePart> GetEntityById(Guid id)
    {
        var entity = await base.Set.Include(x => x.Case)
            .FirstAsync(x => x.Id == id);
        return entity;
    }

    public async Task CreateOrModifyCasePart(Case cCase, CasePart casePart)
    {
        var c = await base.Set.Include(x => x.Case)
            .FirstOrDefaultAsync(x => x.Id == casePart.Id);

        if (c is null)
        {
            casePart.Case = cCase;
            this.Set.Add(casePart);
        }
        else
        {
            await ModifyCasePart(casePart);
        }
        await base.Context.SaveChangesAsync();
    }

    public async Task ModifyCasePart(CasePart casePart)
    {
        var queriedCasePart = await GetEntityById(casePart.Id);

        //Mapper.Map(casePart, queriedCasePart);
    }
    public async Task<List<CasePart>> GetParticipantsForCase(Guid caseId)
    {
        var cases = await Set.Where(x => x.CaseId == caseId).ToListAsync();
        return cases;
    }
}
