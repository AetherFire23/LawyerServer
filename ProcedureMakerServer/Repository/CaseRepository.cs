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

    public CaseRepository(
        ProcedureContext context,
        IMapper mapper,
        ILawyerRepository lawyerRepository,
        ICasePartRepository casePartRepository,
        IClientRepository clientRepository) : base(context, mapper)
    {
        _lawyerRepository = lawyerRepository;
        _casePartRepository = casePartRepository;
        _clientRepository = clientRepository;
    }

    public override async Task<Case> GetEntityById(Guid id)
    {
        var lcase = await base.Set
            .Include(p => p.Client)
            .Include(c => c.Participants)
            .Include(c => c.ManagerLawyer)
            .FirstAsync(c => c.Id == id);

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
        var retrieved = await GetEntityById(caseDto.Id);
        Mapper.Map(caseDto, retrieved);

        await Context.SaveChangesAsync();

    }

    public async Task<CaseDto> MapCaseDto(Guid caseId)
    {
        var user = await GetEntityById(caseId);

        var dto = Mapper.Map<CaseDto>(user); // check if caseparticipant gets mapped even if Icollection

        return dto;
    }
}
