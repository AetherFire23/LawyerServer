using AutoMapper;
using EFCoreBase.Entities;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Interfaces;
using ProcedureMakerServer.Repository.ProcedureRepo;

namespace ProcedureMakerServer.Repository;

public class CasePartRepository : ProcedureCrudBase<CaseParticipant>, ICasePartRepository
{
    public CasePartRepository(ProcedureContext context, IMapper mapper) : base(context, mapper)
    {

    }

    public override async Task<CaseParticipant> GetEntityById(Guid id)
    {
        CaseParticipant entity = await base.Set.Include(x => x.Case)
            .FirstAsync(x => x.Id == id);
        return entity;
    }

    // This shouldnt! work
    public async Task CreateOrModifyCasePart(Case cCase, CaseParticipant casePart)
    {
        CaseParticipant? caseEntity = await base.Set.Include(x => x.Case)
            .FirstOrDefaultAsync(x => x.Id == casePart.Id);

        if (caseEntity is null)
        {
            casePart.Case = cCase;
            _ = this.Set.Add(casePart);
            _ = await base.Context.SaveChangesAsync();
        }
        await ModifyCasePart(casePart);
        _ = await base.Context.SaveChangesAsync();
    }

    public async Task ModifyCasePart(CaseParticipant casePart)
    {
        _ = await GetEntityById(casePart.Id);

        //Mapper.Map(casePart, queriedCasePart);
    }
    public async Task<List<CaseParticipant>> GetParticipantsForCase(Guid caseId)
    {
        List<CaseParticipant> cases = await Set.Where(x => x.CaseId == caseId).ToListAsync();
        return cases;
    }
}
