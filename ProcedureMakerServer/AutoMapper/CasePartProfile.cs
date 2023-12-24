using AutoMapper;
using ProcedureMakerServer.Entities;

namespace ProcedureMakerServer.AutoMapper;

public class CasePartProfile : Profile
{

    public CasePartProfile()
    {
        _ = CreateMap<CaseParticipant, CaseParticipant>();
    }
}
