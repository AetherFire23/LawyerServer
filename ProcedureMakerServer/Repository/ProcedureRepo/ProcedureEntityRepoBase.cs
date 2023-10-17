using AutoMapper;
using EFCoreBase.Entities;
using EFCoreBase.Repositories;

namespace ProcedureMakerServer.Repository.ProcedureRepo;

public class ProcedureEntityRepoBase<T> : EntityRepositoryBase<ProcedureContext, T>
    where T : EntityBase
{
    public readonly IMapper Mapper;

    public ProcedureEntityRepoBase(IMapper mapper, ProcedureContext context) : base(context)
    {
        Mapper = mapper;
    }
}
