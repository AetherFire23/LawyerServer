using AutoMapper;
using ProcedureMakerServer.Entities;

namespace ProcedureMakerServer.AutoMapper;

public class CasePartProfile : Profile
{

    public CasePartProfile()
    {
        CreateMap<CasePart, CasePart>();
    }
}
