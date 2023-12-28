﻿using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.Models;

namespace ProcedureMakerServer.Interfaces;

public interface ICaseContextService
{
    Task<GetCaseResponse> CreateNewCase(CaseCreationInfo creationInfo);
    Task<CaseContextDto> GetCaseContext(Guid lawyerId);
    Task UpdateCasePrimitiveProps(CaseDto caseDto);
}