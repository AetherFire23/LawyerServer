using AutoMapper;
using EFCoreBase.Entities;
using ProcedureMakerServer.Dtos;

namespace ProcedureMakerServer.AutoMapper;

public class CaseDtoProfile : Profile
{
    public CaseDtoProfile()
    {
        CreateMap<Case, CaseDto>();
    }

}
