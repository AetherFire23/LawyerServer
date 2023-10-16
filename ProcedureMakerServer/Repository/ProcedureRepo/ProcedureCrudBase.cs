using AutoMapper;
using EFCoreBase.Entities;
using EFCoreBase.Repositories;

namespace ProcedureMakerServer.Repository.ProcedureRepo;

public abstract class ProcedureCrudBase<T> : CrudRepositoryBase<ProcedureContext, T>
    where T : EntityBase
{
    protected readonly IMapper _mapper;
    public ProcedureCrudBase(ProcedureContext context, IMapper mapper) : base(context)
    {
        _mapper = mapper;
    }
}
