using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ProcedureMakerServer.Billing.InvoiceDtos;
using ProcedureMakerServer.Billing.StatementDtos;
using ProcedureMakerServer.Billing.StatementEntities;
using ProcedureMakerServer.Repository.ProcedureRepo;

namespace ProcedureMakerServer.Billing.Services;

public partial class AccountStatementRepository : ProcedureRepositoryContextBase
{
    public AccountStatementRepository(IMapper mapper, ProcedureContext context) : base(mapper, context)
    {

    }

    public async Task<AccountStatementDto> ConstructAccountStatementDtoByCaseId(Guid caseId)
    {
        var accountStatement = await GetAccountStatementByCaseId(caseId);
        IEnumerable<InvoiceDto> invoicesDto = await MapInvoicesDto(accountStatement.Id);
        AccountStatementDto accountStatementDto = new()
        {
            Invoices = invoicesDto.ToList(),
            Id = accountStatement.Id,
        };

        return accountStatementDto;
    }

    public async Task<AccountStatementDto> ConstructAccountStatementDtoById(Guid accountStatementId)
    {
        var accountStatement = await GetAccountStatementById(accountStatementId);
        IEnumerable<InvoiceDto> invoicesDto = await MapInvoicesDto(accountStatement.Id);
        AccountStatementDto accountStatementDto = new()
        {
            Invoices = invoicesDto.ToList(),
            Id = accountStatement.Id,
        };

        return accountStatementDto;
    }

    public async Task<IEnumerable<InvoiceDto>> MapInvoicesDto(Guid accountStatementId)
    {
        var invoices = await Context.Invoices.Where(x => x.AccountStatementId == accountStatementId).ToListAsync();

        var invoicesDto = new List<InvoiceDto>();
        foreach (Invoice invoice in invoices)
        {
            InvoiceDto invoiceDto = await MapInvoiceDto(invoice);
            invoicesDto.Add(invoiceDto);
        }
        return invoicesDto;
    }

    private async Task<InvoiceDto> MapInvoiceDto(Invoice invoice)
    {
        var activitiesDto = await MapActivitiesDto(invoice.Id);
        var payments = await MapPaymentsDto(invoice.Id);
        var invoiceDto = new InvoiceDto()
        {
            Activities = activitiesDto.ToList(),
            InvoiceStatus = invoice.InvoiceStatus,
            Payments = payments.ToList(),
            Id = invoice.Id,
        };
        return invoiceDto;
    }

    public async Task<IEnumerable<ActivityEntryDto>> MapActivitiesDto(Guid invoiceId)
    {
        var activities = await Context.Activities.Where(x => x.InvoiceId == invoiceId).ToListAsync();
        var activitiesDto = activities.Select(x => x.ToDto());
        return activitiesDto;
    }

    public async Task<IEnumerable<PaymentDto>> MapPaymentsDto(Guid invoiceId)
    {
        var payments = await Context.Payments.Where(x => x.InvoiceId == invoiceId).ToListAsync();
        var paymentsDto = payments.Select(x => x.ToDto());
        return paymentsDto;
    }

    private async Task<IIncludableQueryable<AccountStatement, List<Invoice>>> CreateAccountStatementIncludes()
    {
        IIncludableQueryable<AccountStatement, List<Invoice>> accountStatementIncludes = Context.AccountStatements
                    .Include(x => x.Case)
                        .ThenInclude(x => x.Client)
                    .Include(x => x.Lawyer)
                        .ThenInclude(x => x.Clients)
                    .Include(x => x.Lawyer)
                        .ThenInclude(x => x.Cases)
                    .Include(x => x.Lawyer)
                    .Include(x => x.Invoices)
                        .ThenInclude(x => x.Activities)
                    .Include(x => x.Invoices)
                        .ThenInclude(x => x.Payments)
                    .Include(x => x.Invoices);

        return accountStatementIncludes;
    }
    private async Task<AccountStatement> GetAccountStatementByCaseId(Guid caseId)
    {
        IIncludableQueryable<AccountStatement, List<Invoice>> accountStatementIncludes = await CreateAccountStatementIncludes();

        AccountStatement accountStatement1 = await accountStatementIncludes.FirstAsync(x => x.CaseId == caseId);
        return accountStatement1;
    }

    private async Task<AccountStatement> GetAccountStatementById(Guid accountId)
    {
        IIncludableQueryable<AccountStatement, List<Invoice>> accountStatementIncludes = await CreateAccountStatementIncludes();

        AccountStatement accountStatement1 = await accountStatementIncludes.FirstAsync(x => x.Id == accountId);
        return accountStatement1;
    }
}
