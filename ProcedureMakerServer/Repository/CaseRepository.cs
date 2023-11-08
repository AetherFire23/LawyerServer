using AutoMapper;
using EFCoreBase.Entities;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Authentication.Interfaces;
using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.Interfaces;
using ProcedureMakerServer.Repository.ProcedureRepo;

namespace ProcedureMakerServer.Repository;

public class CaseRepository : ProcedureCrudBase<Case>, ICaseRepository
{
    private readonly IUserRepository _userRepository;
    private readonly ILawyerRepository _lawyerRepository;
    private readonly ICasePartRepository _casePartRepository;
    private readonly IClientRepository _clientRepository;

    private ProcedureContext c2;
    public CaseRepository(ProcedureContext context, IMapper mapper,
        IUserRepository userRepository,
        ILawyerRepository lawyerRepository,
        ICasePartRepository casePartRepository,
        IClientRepository clientRepository) : base(context, mapper)
    {
        _userRepository = userRepository;
        _lawyerRepository = lawyerRepository;
        _casePartRepository = casePartRepository;
        _clientRepository = clientRepository;
        c2 = context;
    }

    public override async Task<Case> GetEntityById(Guid id)
    {
        var lcase = await c2.Cases
            //.Include(p => p.Client)
            .Include(c => c.Participants)
            //.Include(c => c.ManagerLawyer)
            .FirstOrDefaultAsync(c => c.Id == id);

        return lcase;
    }

    public async Task ModifyCase(Case lcase)
    {
        var retrieved = await GetEntityById(lcase.Id);
        Mapper.Map(lcase, retrieved);

        await Context.SaveChangesAsync();
    }

    public async Task ModifyCaseFromDto(CaseDto caseDto)
    {
        Case retrieved = await GetEntityById(caseDto.Id);

        retrieved.CourtNumber = caseDto.CourtNumber;
        retrieved.DistrictName = caseDto.DistrictName;
        retrieved.CourtAffairNumber = caseDto.CourtAffairNumber;
        retrieved.CaseNumber = caseDto.CaseNumber;
        retrieved.CourtType = caseDto.CourtType;
        retrieved.CourtNumber = caseDto.CourtNumber;

        await Context.SaveChangesAsync();
    }

    public async Task<CaseDto> MapCaseDto(Guid caseId)
    {
        Case lcase = await GetEntityById(caseId);


        CaseDto caseDto = new CaseDto()
        {
            Id = lcase.Id,
            Client = lcase.Client,
            ManagerLawyer = lcase.ManagerLawyer,
            Participants = lcase.Participants.ToList(),
            CaseNumber = lcase.CaseNumber,
            CourtAffairNumber = lcase.CourtAffairNumber,
            CourtNumber = lcase.CourtNumber,
            CourtType = lcase.CourtType,
            DistrictName = lcase.DistrictName,

        };


        return caseDto;
    }
}
