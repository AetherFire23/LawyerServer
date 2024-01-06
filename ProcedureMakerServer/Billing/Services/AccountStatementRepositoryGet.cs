using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ProcedureMakerServer.Billing.InvoiceDtos;
using ProcedureMakerServer.Billing.StatementDtos;
using ProcedureMakerServer.Billing.StatementEntities;
using ProcedureMakerServer.Repository.ProcedureRepo;
using ProcedureShared.Dtos;

namespace ProcedureMakerServer.Billing.Services;

public partial class AccountStatementRepository : ProcedureRepositoryContextBase
{
	public AccountStatementRepository(IMapper mapper, ProcedureContext context) : base(mapper, context)
	{

	}

	public async Task<AccountStatementDto> ConstructAccountStatementDtoByCaseId(Guid caseId)
	{
		var accountStatement = await GetAccountStatementByCaseId(caseId);
		var invoicesDto = await MapInvoicesDto(accountStatement.Id);
		var accountStatementDto = new AccountStatementDto()
		{
			Invoices = invoicesDto.ToList(),
			Id = accountStatement.Id,
		};

		return accountStatementDto;
	}

	public async Task<AccountStatementDto> ConstructAccountStatementDtoById(Guid accountStatementId)
	{
		var accountStatement = await GetAccountStatementById(accountStatementId);
		var invoicesDto = await MapInvoicesDto(accountStatement.Id);
		var accountStatementDto = new AccountStatementDto()
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
			var invoiceDto = await MapInvoiceDto(invoice);
			invoicesDto.Add(invoiceDto);
		}
		return invoicesDto;
	}

	public async Task<AccountStatement> GetAccountStatementByCaseId(Guid caseId)
	{
		var accountStatementIncludes = await CreateAccountStatementIncludes();
		var accountStatement1 = await accountStatementIncludes.FirstAsync(x => x.CaseId == caseId);
		return accountStatement1;
	}

	private async Task<InvoiceDto> MapInvoiceDto(Invoice invoice)
	{
		var activitiesDto = await MapActivitiesDto(invoice.Id);
		var payments = await MapPaymentsDto(invoice.Id);
		var availableBillingElements = await MapAvailableBillingElementsForInvoice(invoice.Id);

		var invoiceDto = new InvoiceDto()
		{
			Activities = activitiesDto.ToList(),
			InvoiceStatus = invoice.InvoiceStatus,
			Payments = payments.ToList(),
			Id = invoice.Id,
			AvailableBillingElementsForInvoice = availableBillingElements,
			InvoiceNumber = invoice.InvoiceNumber,
		};

		return invoiceDto;
	}

	private async Task<List<BillingElementDto>> MapAvailableBillingElementsForInvoice(Guid invoiceId)
	{
		Guid lawyerId = await FindLawyerIdFromInvoiceId(invoiceId);

		var invoiceSpecificBillingElements = await Context.BillingElements
			.Where(x => x.IsInvoiceSpecific)
			.Where(x => x.SpecificInvoiceId == invoiceId)
			.ToListAsync();

		var globalBillingElements = await Context.BillingElements
			.Where(x => x.ManagerLawyerId == lawyerId)
			.Where(x => x.IsInvoiceSpecific == false)
			.ToListAsync();

		var invoiceBillingElements = invoiceSpecificBillingElements
			.Union(globalBillingElements)
			.Select(x => x.ToDto())
			.ToList();

		return invoiceBillingElements;
	}

	private async Task<IEnumerable<InvoicePaymentDto>> MapPaymentsDto(Guid invoiceId)
	{
		var payments = await Context.InvoicePayments.Where(x => x.InvoiceId == invoiceId).ToListAsync();
		var paymentsDto = payments.Select(x => x.ToDto());
		return paymentsDto;
	}

	private async Task<ActivityDto> MapActivityEntryDto(Guid activityId)
	{
		var activity = await Context.Activities
			.FirstAsync(x => x.Id == activityId);

		var activityDto = new ActivityDto
		{
			Quantity = activity.Quantity,
			Id = activity.Id,
			CostInDollars = activity.CostInDollars,
			Description = activity.Description,
		};

		return activityDto;
	}

	private async Task<List<ActivityDto>> MapActivitiesDto(Guid invoiceId)
	{
		var activityIds = await Context.Activities
			.Where(x => x.InvoiceId == invoiceId)
			.Select(x => x.Id).ToListAsync();

		var activitiesDtos = new List<ActivityDto>();
		foreach (var activityId in activityIds)
		{
			var activityDto = await MapActivityEntryDto(activityId);
			activitiesDtos.Add(activityDto);
		};

		return activitiesDtos;
	}

	private async Task<IIncludableQueryable<AccountStatement, List<Invoice>>> CreateAccountStatementIncludes()
	{
		IIncludableQueryable<AccountStatement, List<Invoice>> accountStatementIncludes = Context.AccountStatements
					.Include(x => x.Case)
						.ThenInclude(x => x.Client)
					.Include(x => x.Case)
						.ThenInclude(x => x.ManagerLawyer)
							.ThenInclude(x => x.DefaultHourlyElement)
					.Include(x => x.Invoices)
						.ThenInclude(x => x.Activities)
					.Include(x => x.Invoices)
						.ThenInclude(x => x.Payments)
					.Include(x => x.Invoices);

		return accountStatementIncludes;
	}

	private async Task<AccountStatement> GetAccountStatementById(Guid accountId)
	{
		var accountStatementIncludes = await CreateAccountStatementIncludes();
		AccountStatement accountStatement1 = await accountStatementIncludes.FirstAsync(x => x.Id == accountId);
		return accountStatement1;
	}

	private async Task<Guid> FindLawyerIdFromInvoiceId(Guid invoiceId)
	{
		var invoice = await Context.Invoices
			.Include(x => x.AccountStatement)
			.ThenInclude(x => x.Lawyer)
			.FirstAsync(x => x.Id == invoiceId);

		var lawyerId = invoice.AccountStatement.Lawyer.Id;
		return lawyerId;
	}
}
