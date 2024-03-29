﻿using EFCoreBase.Entities;
using EFCoreBase.Utils;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Billing.StatementEntities;
using ProcedureMakerServer.Exceptions.HttpResponseExceptions;
using ProcedureMakerServer.Models;
using ProcedureMakerServer.Repository;
using ProcedureMakerServer.Trusts;
using ProcedureShared.Dtos;

namespace ProcedureMakerServer.Services;

public class CaseContextService
{
    private readonly ProcedureContext _context;
    private readonly CaseContextRepository _casesContextRepository;
    private readonly LawyerRepository _lawyerRepository;
    public CaseContextService(ProcedureContext procedureContext,
                              CaseContextRepository casesContextRepository,
                              LawyerRepository lawyerRepository)
    {
        _context = procedureContext;
        _casesContextRepository = casesContextRepository;
        _lawyerRepository = lawyerRepository;
    }

    public async Task<GetCaseResponse> CreateNewCase(Guid lawyerId, Guid clientId)
    {
        await _context.SaveChangesAsync();

        var lawyer = await _lawyerRepository.GetEntityById(lawyerId);
        var client = await _context.Clients.FirstByIdAsync(clientId);

        var cCase = new Case()
        {
            ManagerLawyer = lawyer,
            Client = client,
        };

        await _context.Cases.AddAsync(cCase);
        await _context.SaveChangesAsync();

        var accountStatement = new AccountStatement()
        {
            Case = cCase,
        };

        await _context.AccountStatements.AddAsync(accountStatement);
        await _context.SaveChangesAsync();

        cCase.AccountStatement = accountStatement; // required one-to=one
        await _context.SaveChangesAsync(); // see if one to one breaks

        TrustClientCard trust = new TrustClientCard()
        {
            Client = client,
        };

        await _context.TrustClientCards.AddAsync(trust);
        await _context.SaveChangesAsync();



        var caseResponse = new GetCaseResponse() { CreatedId = cCase.Id };
        return caseResponse;
    }

    public async Task<CaseContextDto> GetCaseContext(Guid lawyerId)
    {
        if (lawyerId.Equals(Guid.Empty)) throw new ArgumentInvalidException("LawyerId was empty");

        var caseContext = await _casesContextRepository.MapCasesContext(lawyerId);
        return caseContext;
    }

    public async Task UpdateCasePrimitiveProps(CaseDto caseDto)
    {
        var ccase = await _context.Cases.FirstByIdAsync(caseDto.Id);

        ccase.CourtNumber = caseDto.CourtNumber;
        ccase.CourtAffairNumber = caseDto.CourtAffairNumber;
        ccase.CaseNumber = caseDto.CaseNumber;
        ccase.ChamberName = caseDto.ChamberName;
        ccase.DistrictName = caseDto.DistrictName;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteCase(Guid caseId)
    {
        var ccase = await _context.Cases
            .Include(x => x.CaseParticipants)
            .FirstAsync(x => x.Id == caseId);

        // delete participants
        foreach (var participant in ccase.CaseParticipants)
        {
            _context.CaseParticipants.Remove(participant);
        }

        _context.Cases.Remove(ccase);
        await _context.SaveChangesAsync();
    }
}
