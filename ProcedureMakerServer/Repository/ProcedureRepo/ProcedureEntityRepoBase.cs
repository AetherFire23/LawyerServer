using AutoMapper;
using EFCoreBase.Entities;
using EFCoreBase.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ProcedureMakerServer.Repository.ProcedureRepo;

public class ProcedureEntityRepoBase<T> : EntityRepositoryBase<ProcedureContext, T>
    where T : EntityBase
{
    protected readonly IMapper Mapper;

    public ProcedureEntityRepoBase(IMapper mapper, ProcedureContext context) : base(context)
    {
        Mapper = mapper;
    }
}
