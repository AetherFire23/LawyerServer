using EFCoreBase.Entities;
using ProcedureMakerServer.Billing.StatementEntities;
using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Exceptions.HttpResponseExceptions;
using ProcedureMakerServer.Interfaces;
using ProcedureMakerServer.Models;
using ProcedureMakerServer.Trusts;

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

    public async Task<GetCaseResponse> CreateNewCase(CaseCreationInfo creationInfo)
    {
        var (lawyerId, caseNumber, clientFirstName, clientLastName) = creationInfo;

        await _procedureContext.SaveChangesAsync();

        var lawyer = await _lawyerRepository.GetEntityById(lawyerId);

        Client client = new Client()
        {
            FirstName = clientFirstName,
            LastName = clientLastName,
            Lawyer = lawyer
        };

        await _procedureContext.Clients.AddAsync(client);
        await _procedureContext.SaveChangesAsync();

        Case cCase = new Case()
        {
            CaseNumber = caseNumber,
            ManagerLawyer = lawyer,
            Client = client,
        };

        await _procedureContext.Cases.AddAsync(cCase);
        await _procedureContext.SaveChangesAsync();

        var accountStatement = new AccountStatement()
        {
            Case = cCase,
            Lawyer = lawyer,
        };
        await _procedureContext.AccountStatements.AddAsync(accountStatement);
        await _procedureContext.SaveChangesAsync();

        var trust = new Trust()
        {
            Client = client,
        };

        await _procedureContext.Trusts.AddAsync(trust);
        await _procedureContext.SaveChangesAsync();


        return new GetCaseResponse() { CreatedId = cCase.Id };
    }

    public async Task<CasesContext> GetCaseContext(Guid lawyerId)
    {
        if (lawyerId.Equals(Guid.Empty)) throw new ArgumentInvalidException("LawyerId was empty");

        CasesContext caseContext = await _casesContextRepository.MapCasesContext(lawyerId);
        return caseContext;
    }

    public async Task SaveCaseDto(CaseDto caseDto)
    {
        await _casesContextRepository.ModifyCaseDto(caseDto);
    }
}
