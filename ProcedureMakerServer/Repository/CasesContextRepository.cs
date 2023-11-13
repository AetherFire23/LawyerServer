using EFCoreBase.Entities;
using ProcedureMakerServer.Authentication.Interfaces;
using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.Interfaces;

namespace ProcedureMakerServer.Repository;

public class CasesContextRepository : ICasesContextRepository
{
    private readonly IUserRepository _userRepository;
    private readonly ICasePartRepository _casePartRepository;
    private readonly ICaseRepository _caseRepository;
    private readonly IClientRepository _clientRepository;
    private readonly ILawyerRepository _lawyerRepository;
    private readonly ProcedureContext _context;
    public CasesContextRepository(
        IUserRepository userRepository,
        ICasePartRepository casePartRepository,
        ICaseRepository caseRepository,
        IClientRepository clientRepository,
        ILawyerRepository lawyerRepository,
        ProcedureContext context)
    {
        _userRepository = userRepository;
        _casePartRepository = casePartRepository;
        _caseRepository = caseRepository;
        _clientRepository = clientRepository;
        _lawyerRepository = lawyerRepository;
        _context = context;
    }

    public async Task<CasesContext> MapCasesContext(Guid lawyerId)
    {
        //User user = await _userRepository.GetUserById(userId);

        var lawyer = await _lawyerRepository.GetEntityById(lawyerId);
        //if (!lawyer.Cases.Any()) return new CasesContext();


        List<CaseDto> cases = new List<CaseDto>();
        foreach (var c in lawyer.Cases)
        {
            var caseDto = await _caseRepository.MapCaseDto(c.Id);
            cases.Add(caseDto);
        }

        CasesContext context = new CasesContext()
        {
            Cases = cases,
            Lawyer = lawyer,
        };

        return context;
    }

    public async Task ModifyCaseDto(CaseDto caseDto)
    {
        Case cCase = await _caseRepository.GetEntityById(caseDto.Id);
        cCase.CourtAffairNumber = caseDto.CourtAffairNumber;
        await _context.SaveChangesAsync();

        await _clientRepository.ModifyClient(caseDto.Client);



        // can either create or update the case part (can create a new client in the form :)
        foreach (var participant in caseDto.Participants)
        {

            await _casePartRepository.CreateOrModifyCasePart(cCase, participant);
            //await _casePartRepository.ModifyCasePart(participant);
        }
    }
}
