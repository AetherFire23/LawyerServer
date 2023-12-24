using EFCoreBase.Entities;
using EFCoreBase.Utils;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Interfaces;

namespace ProcedureMakerServer.Repository;

public class CasesContextRepository
{
    private readonly CaseRepository _caseRepository;
    private readonly ILawyerRepository _lawyerRepository;
    private readonly ProcedureContext _context;
    public CasesContextRepository(
        CaseRepository caseRepository,
        ILawyerRepository lawyerRepository,
        ProcedureContext context)
    {
        _caseRepository = caseRepository;
        _lawyerRepository = lawyerRepository;
        _context = context;
    }

    public async Task<CasesContext> MapCasesContext(Guid lawyerId)
    {
        var lawyer = await _lawyerRepository.GetEntityById(lawyerId);
        var cases = await MapCasesDtos(lawyer);

        var context = new CasesContext
        {
            Cases = cases,
            Lawyer = lawyer,
        };

        return context;
    }

    private async Task<List<CaseDto>> MapCasesDtos(Lawyer lawyer)
    {
        var cases = new List<CaseDto>();
        foreach (Case c in lawyer.Cases)
        {
            cases.Add(await _caseRepository.MapCaseDto(c.Id));
        }

        return cases;
    }

    public async Task AddCaseParticipant(Guid caseId, CaseParticipantDto updatedCaseParticipantDto)
    {
        var lcase = await _context.Cases.FirstAsync(c=> c.Id == caseId);

        var caseParticipant = new CaseParticipant()
        {
            Id = updatedCaseParticipantDto.GenerateIdIfNull(),
            Case = lcase,
        };

        updatedCaseParticipantDto.CopyFromCourtMember(caseParticipant);

        _context.CaseParticipants.Add(caseParticipant);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateCaseParticipant(CaseParticipantDto updatedCaseParticipant)
    {
        //var caseParticipant = await _context.CaseParticipants.FirstByIdAsync(updatedCaseParticipant.Id);
        var caseParticipant = await _context.CaseParticipants.FirstAsync(x=> x.Id == updatedCaseParticipant.Id);

        caseParticipant.CopyFromCourtMember(caseParticipant);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveCaseParticipant(Guid caseParticipantId)
    {
        var caseParticipant = await _context.CaseParticipants.FirstByIdAsync(caseParticipantId);

        _context.CaseParticipants.Remove(caseParticipant);
        await _context.SaveChangesAsync();
    }
}
