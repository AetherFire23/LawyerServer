﻿using AutoMapper;
using EFCoreBase.Entities;

namespace ProcedureMakerServer.AutoMapper;

public class CaseProfile : Profile
{
    public CaseProfile()
    {
        _ = CreateMap<Case, Case>();

    }
}
