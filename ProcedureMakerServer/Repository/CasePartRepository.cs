using AutoMapper;
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

    public async Task ModifyCasePart(CasePart casePart)
    {
        var queriedCasePart = await GetEntityById(casePart.Id);

        Mapper.Map(casePart, queriedCasePart);
    }
}
