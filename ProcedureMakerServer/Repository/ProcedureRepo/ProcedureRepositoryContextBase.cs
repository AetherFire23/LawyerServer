using AutoMapper;
using EFCoreBase.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ProcedureMakerServer.Repository.ProcedureRepo;

public class ProcedureRepositoryContextBase : RepositoryBase<ProcedureContext>
{
    protected readonly IMapper _mapper;
    public ProcedureRepositoryContextBase(IMapper mapper, ProcedureContext context) : base(context)
    {
        _mapper = mapper;
    }
}
