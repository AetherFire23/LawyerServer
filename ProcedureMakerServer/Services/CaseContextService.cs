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
    public CaseContextService(ProcedureContext procedureContext, ICasesContextRepository casesContextRepository)
    {
        _procedureContext = procedureContext;
        _casesContextRepository = casesContextRepository;
    }

    public async Task CreateNewCase(CaseCreationInfo creationInfo)
    {
        var (lawyerId, caseNumber, clientFirstName, clientLastName) = creationInfo;

        // initialize some default values but leave the rest mostly empty
        Client client = new Client()
        {
            FirstName = clientFirstName,
            LastName = clientLastName,
        };

        Case c = new Case()
        {
            ClientId = client.Id,
            CaseNumber = caseNumber,
            ManagerLawyerId = lawyerId,
        };


        await _procedureContext.Clients.AddAsync(client);
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
