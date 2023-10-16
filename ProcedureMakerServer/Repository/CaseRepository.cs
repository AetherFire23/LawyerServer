using AutoMapper;
using EFCoreBase.Entities;
using ProcedureMakerServer.Interfaces;
using ProcedureMakerServer.Repository.ProcedureRepo;

namespace ProcedureMakerServer.Repository;

public class CaseRepository : ProcedureCrudBase<EFCoreBase.Entities.LawyerFile>, ICaseRepository
{
    public CaseRepository(ProcedureContext context, IMapper mapper) : base(context, mapper)
    {

    }
}
