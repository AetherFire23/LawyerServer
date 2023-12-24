using AutoMapper;
using DocumentFormat.OpenXml.EMMA;
using EFCoreBase.Entities;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Authentication.Interfaces;
using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Interfaces;
using ProcedureMakerServer.Repository.ProcedureRepo;

namespace ProcedureMakerServer.Repository;

public class CaseRepository : ProcedureCrudBase<Case>
{
    private readonly IUserRepository _userRepository;
    private readonly ILawyerRepository _lawyerRepository;
    private readonly CasePartRepository _casePartRepository;
    private readonly ClientRepository _clientRepository;

    private ProcedureContext c2;
    public CaseRepository(ProcedureContext context, IMapper mapper,
        IUserRepository userRepository,
        ILawyerRepository lawyerRepository,
        CasePartRepository casePartRepository,
        ClientRepository clientRepository) : base(context, mapper)
    {
        _userRepository = userRepository;
        _lawyerRepository = lawyerRepository;
        _casePartRepository = casePartRepository;
        _clientRepository = clientRepository;
        c2 = context;
    }

    public async Task<CaseDto> MapCaseDto(Guid caseId)
    {
        Case lcase = await GetEntityById(caseId);
        var participants = await GetCaseParticipantsDto(caseId);

        CaseDto caseDto = new CaseDto
        {
            Id = lcase.Id,
            Client = lcase.Client,
            ManagerLawyer = lcase.ManagerLawyer,
            Participants = participants,
            CaseNumber = lcase.CaseNumber,
            CourtAffairNumber = lcase.CourtAffairNumber,
            CourtNumber = lcase.CourtNumber,
            ChamberName = lcase.ChamberName,
            DistrictName = lcase.DistrictName,
        };
        return caseDto;
    }

    public override async Task<Case> GetEntityById(Guid id)
    {
        Case lcase = await c2.Cases
            .Include(c => c.CaseParticipants)
            .FirstAsync(c => c.Id == id);

        return lcase;
    }

    public async Task UpdateCase(CaseDto caseDto)
    {
        Case retrieved = await GetEntityById(caseDto.Id);

        retrieved.CourtNumber = caseDto.CourtNumber;
        retrieved.DistrictName = caseDto.DistrictName;
        retrieved.CourtAffairNumber = caseDto.CourtAffairNumber;
        retrieved.CaseNumber = caseDto.CaseNumber;
        retrieved.ChamberName = caseDto.ChamberName;
        retrieved.CourtNumber = caseDto.CourtNumber;

        await Context.SaveChangesAsync();
    }

    public async Task<List<string>> GetEmailsToNotify(Guid caseId)
    {
        var notifiableInCase = await Context.CaseParticipants
            .Where(c => c.MustNotify)
            .Where(c => c.NotificationEmail != "")
            .Select(c => c.NotificationEmail)
            .ToListAsync();

        return notifiableInCase;
    }

    private async Task<List<CaseParticipantDto>> GetCaseParticipantsDto(Guid caseId)
    {
        var lcase = await GetEntityById(caseId);
        var participants = lcase.CaseParticipants.ToList()
            .Select(x => x.ToDto())
            .ToList();

        return participants;
    }
}
