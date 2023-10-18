using EFCoreBase.Entities;
using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Interfaces;
using ProcedureMakerServer.Models;

namespace ProcedureMakerServer.Services;

public class CaseContextService : ICaseContextService
{
    private readonly ProcedureContext _procedureContext;
    private readonly ICasesContextRepository _casesContextRepository;
    private readonly ILawyerRepository _lawyerRepository;
    public CaseContextService(ProcedureContext procedureContext, ICasesContextRepository casesContextRepository, ILawyerRepository lawyerRepository)
    {
        _procedureContext = procedureContext;
        _casesContextRepository = casesContextRepository;
        _lawyerRepository = lawyerRepository;
    }

    public async Task CreateNewCase(CaseCreationInfo creationInfo)
    {
        var (lawyerId, caseNumber, clientFirstName, clientLastName) = creationInfo;
        await _procedureContext.SaveChangesAsync();

        // initialize some default values but leave the rest mostly empty

        var lawyer = await _lawyerRepository.GetEntityById(lawyerId);

        Client client = new Client()
        {
            FirstName = clientFirstName,
            LastName = clientLastName,
            Lawyer = lawyer
        };

        await _procedureContext.Clients.AddAsync(client);
        await _procedureContext.SaveChangesAsync();


        Case c = new Case()
        {
            CaseNumber = caseNumber,
            ManagerLawyer = lawyer,
            Client = client
        };
        

        await _procedureContext.Cases.AddAsync(c);
        await _procedureContext.SaveChangesAsync();

    }

    public async Task<CasesContext> GetCase(Guid lawyerId)
    {
        var caseContext = await _casesContextRepository.MapCasesContext(lawyerId);
        return caseContext;
    }

    public async Task SaveContextDto(CasesContext caseContext)
    {
        foreach (CaseDto caseDto in caseContext.Cases)
        {
            await _casesContextRepository.ModifyContextFromCaseDto(caseDto);
        }
    }
}
