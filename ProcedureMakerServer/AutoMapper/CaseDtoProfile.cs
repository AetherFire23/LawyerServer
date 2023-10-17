using AutoMapper;
using ProcedureMakerServer.Dtos;

namespace ProcedureMakerServer.AutoMapper;

public class CaseDtoProfile : Profile
{
    public CaseDtoProfile()
    {
        CreateMap<CaseDto, CaseDto>();
    }

}
