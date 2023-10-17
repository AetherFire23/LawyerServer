using ProcedureMakerServer.Authentication;
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
    public CasesContextRepository(
        IUserRepository userRepository,
        ICasePartRepository casePartRepository,
        ICaseRepository caseRepository,
        IClientRepository clientRepository,
        ILawyerRepository lawyerRepository)
    {
        _userRepository = userRepository;
        _casePartRepository = casePartRepository;
        _caseRepository = caseRepository;
        _clientRepository = clientRepository;
        _lawyerRepository = lawyerRepository;
    }

    public async Task<CasesContext> MapCasesContext(Guid lawyerId)
    {
        //User user = await _userRepository.GetUserById(userId);

        var lawyer = await _lawyerRepository.GetEntityById(lawyerId);
        if (!lawyer.Cases.Any()) return new CasesContext();

        List<CaseDto> cases = new List<CaseDto>();
        foreach (var c in lawyer.Cases)
        {
            var caseDto = await _caseRepository.MapCaseDto(c.Id);
            cases.Add(caseDto);
        }

        CasesContext context = new CasesContext()
        {
            Cases = cases,
        };

        return context;
    }

    public async Task ModifyContextFromCaseDto(CaseDto caseDto)
    {
        // jai tu vrm besoin de modifier le lawye vu que cest tout le temps le meme, en tout cas
        await _lawyerRepository.ModifyLawyer(caseDto.ManagerLawyer);
        await _clientRepository.ModifyClient(caseDto.Client);

        foreach (var participant in caseDto.Participants)
        {
            await _casePartRepository.ModifyCasePart(participant);
        }

        var lcase = _caseRepository.ModifyCaseFromDto(caseDto);

        // save lawyer
        // save Client
        // save CaseParts
    }
}
