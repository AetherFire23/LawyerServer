using AutoMapper;
using EFCoreBase.Entities;
using Microsoft.Extensions.Options;
using ProcedureMakerServer.Dtos;

namespace ProcedureMakerServer.AutoMapper;

public class CaseProfile : Profile
{
    public CaseProfile()
    {
        CreateMap<Case, Case>();
            
    }
}
