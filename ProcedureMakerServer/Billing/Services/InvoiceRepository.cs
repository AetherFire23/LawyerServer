using AutoMapper;
using EFCoreBase.Utils;
using HtmlRenderFun;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Billing.InvoiceDtos;
using ProcedureMakerServer.Billing.StatementDtos;
using ProcedureMakerServer.Billing.StatementEntities;
using ProcedureMakerServer.Constants;
using ProcedureMakerServer.Repository;
using ProcedureMakerServer.Repository.ProcedureRepo;
using ProcedureShared.Dtos;

namespace ProcedureMakerServer.Billing.Services;

public class InvoiceRepository : ProcedureRepositoryContextBase
{
	private AccountStatementRepository _accountStatementRepository;
	private readonly CaseContextRepository _caseRepository;
	private readonly ProcedureHtmlRenderer _procedureHtmlRenderer;

	public InvoiceRepository(IMapper mapper, ProcedureContext context, AccountStatementRepository accountStatemetnRepo, CaseContextRepository caseRepository, ProcedureHtmlRenderer procedureHtmlRenderer) : base(mapper, context)
	{
		_accountStatementRepository = accountStatemetnRepo;
		_caseRepository = caseRepository;
		_procedureHtmlRenderer = procedureHtmlRenderer;
	}

	public async Task CreateInvoice(Guid caseId)
	{
		var accountStatement = await _accountStatementRepository.GetAccountStatementByCaseId(caseId);

		// need to increment the lawyers bills emitted count for correct accountability
		accountStatement.Lawyer.BillsEmittedCount++;

		// When you create an invoice,
		// Make a copy of the current defaultHourlyElement
		// The reason is if you change the default hourly element,
		// It will change price for all cases (even ongoing ones)
		// Which is absolutely illegal. 

		var currentLawyerBillingElement = accountStatement.Case.ManagerLawyer.DefaultHourlyElement;
		var currentBillingElementCopy = new BillingElement
		{
			ManagerLawyer = accountStatement.Case.ManagerLawyer,
			ActivityName = currentLawyerBillingElement.ActivityName,
			Amount = currentLawyerBillingElement.Amount,
			IsDisburse = currentLawyerBillingElement.IsDisburse,
			IsHourlyRate = currentLawyerBillingElement.IsHourlyRate,
		};

		Context.BillingElements.Add(currentBillingElementCopy);
		await Context.SaveChangesAsync();

		var newInvoice = new Invoice
		{
			AccountStatement = accountStatement,
			DefaultBillingElement = currentBillingElementCopy,
		};

		Context.Invoices.Add(newInvoice);
		await Context.SaveChangesAsync();

		var accountStatement2 = await _accountStatementRepository.GetAccountStatementByCaseId(caseId);
	}

	public async Task UpdateInvoiceProperties(InvoiceDto dto)
	{
		var invoice = await Context.Invoices.FirstByIdAsync(dto.Id);

		invoice.InvoiceStatus = dto.InvoiceStatus;

		await Context.SaveChangesAsync();
	}

	public async Task ArchiveInvoice(Guid invoiceId)
	{
		var invoice = await Context.Invoices.FirstAsync(x => x.Id == invoiceId);

		Context.Invoices.Remove(invoice);
		await Context.SaveChangesAsync();
	}

	// a payment is a disburse for a Trust (mapped inside the MapTrustDisburse Method)
	public async Task AddInvoicePayment(Guid invoiceId, InvoicePaymentDto invoicePaymentDto)
	{
		var invoice = await Context.Invoices.FirstByIdAsync(invoiceId);

		var invoicePayment = new InvoicePayment
		{
			Id = invoicePaymentDto.GenerateIdIfNull(),
			Invoice = invoice,
			AmountPaid = invoicePaymentDto.AmountPaid,
			AmountPaidDate = invoicePaymentDto.AmoundPaidDate,
			IsPaymentComingFromTrust = invoicePaymentDto.IsPaymentComingFromTrust,
		};

		invoicePayment.CopyFromDto(invoicePaymentDto);

		Context.InvoicePayments.Add(invoicePayment);
		await Context.SaveChangesAsync();
	}

	public async Task UpdateInvoicePayment(InvoicePaymentDto invoicePaymentDto)
	{
		var invoicePayment = await Context.InvoicePayments.FirstByIdAsync(invoicePaymentDto.Id);

		invoicePayment.CopyFromDto(invoicePaymentDto);

		await Context.SaveChangesAsync();
	}

	public async Task DeleteInvoicePayment(Guid invoicePaymentId)
	{
		var invoicePayment = await Context.InvoicePayments.FirstByIdAsync(invoicePaymentId);

		Context.InvoicePayments.Remove(invoicePayment);

		await Context.SaveChangesAsync();
	}

	public async Task AddActivity(Guid invoiceId, ActivityDto createdActivityDto) // requires a billing element I guess
	{
		var invoice = await Context.Invoices.FirstByIdAsync(invoiceId);

		var newActivity = new Activity()
		{
			Description = createdActivityDto.Description,
			Quantity = createdActivityDto.Quantity,
			Invoice = invoice,
			IsDisburse = createdActivityDto.IsDisburse,
			IsTaxable = createdActivityDto.IsTaxable,
			CostInDollars = createdActivityDto.CostInDollars,
		};

		Context.Activities.Add(newActivity);
		await Context.SaveChangesAsync();
	}

	public async Task UpdateActivity(ActivityDto activityDto)
	{
		var activity = await Context.Activities.FirstByIdAsync(activityDto.Id);
		activity.Description = activityDto.Description;
		activity.Quantity = activityDto.Quantity;

		await Context.SaveChangesAsync();
	}

	public async Task RemoveActivity(Guid activityId)
	{
		var activity = await Context.Activities
			.FirstAsync(x => x.Id == activityId);

		Context.Activities.Remove(activity);
		await Context.SaveChangesAsync();
	}

	public async Task AddBillingElement(BillingElementDto dto, Guid lawyerId)
	{
		var lawyer = await Context.Lawyers.FirstByIdAsync(lawyerId);

		// TODO: add override billingElement 
		// so that i can have variable costs 
		var billingElement = new BillingElement
		{
			ManagerLawyer = lawyer,
			ActivityName = dto.ActivityName,
			Amount = dto.Amount,
			IsHourlyRate = dto.IsHourlyRate,
			IsDisburse = dto.IsDisburse,
		};

		Context.BillingElements.Add(billingElement);
		await Context.SaveChangesAsync();
	}

	public async Task UpdateBillingElement(BillingElementDto dto)
	{
		var billingElement = await Context.BillingElements.FirstByIdAsync(dto.Id);

		billingElement.ActivityName = dto.ActivityName;
		billingElement.Amount = dto.Amount;
		billingElement.IsHourlyRate = dto.IsHourlyRate;
		billingElement.IsDisburse = dto.IsDisburse;

		await Context.SaveChangesAsync();
	}

	public async Task RemoveBillingElement(Guid billingElementId)
	{
		var billingElement = await Context.BillingElements.FirstByIdAsync(billingElementId);
		Context.BillingElements.Remove(billingElement);
	}


	/// <returns> Path to the pdf files :) </returns>
	public async Task<string> GetInvoicePdf(Guid invoiceId)
	{
		var invoiceSummary = await GetInvoiceSummary(invoiceId);
		var html = await _procedureHtmlRenderer.RenderInvoiceToHtml(invoiceSummary);

		string randomFilePath = Path.Combine(ConstantPaths.TemporaryFilesPath, $"{Guid.NewGuid()}.html");


		return string.Empty;
	}

	public async Task<InvoiceSummary> GetInvoiceSummary(Guid invoiceId)
	{
		// lil query just to infer the caseId from the invoiceId
		var lcase = (await Context.Invoices
			.Include(x => x.AccountStatement)
				.ThenInclude(x => x.Case)
					.ThenInclude(x => x.ManagerLawyer)
			.FirstAsync(x => x.Id == invoiceId)
			).AccountStatement.Case;

		var ctx = await _caseRepository.MapCasesContext(lcase.ManagerLawyerId);
		var caseDto = ctx.Cases.First(x => x.Id == lcase.Id);
		var invoice = caseDto.Invoices.First(x => x.Id == invoiceId);

		var taxableActivitiesCost = invoice.GetDisbursesTaxableTotal() + invoice.GetHourlyRatesTotal();
		var nonTaxableActivitiesCost = invoice.GetDisbursesNonTaxableTotal();
		var tps = taxableActivitiesCost * Taxes.TPSInPercentage;
		var tvq = taxableActivitiesCost * Taxes.TVQInPercentage;
		var total = taxableActivitiesCost + nonTaxableActivitiesCost + tps + tvq;
		var paymentsTotal = invoice.GetPaymentsTotal();
		var balance = total = paymentsTotal;

		// Get the bill number
		var invoiceSummary = new InvoiceSummary
		{
			Case = caseDto,
			Invoice = invoice,
			Created = DateTime.Now,
			BillNumber = lcase.ManagerLawyer.BillsEmittedCount,
			HourlyRatesTotal = invoice.GetHourlyRatesTotal(),
			DisbursesTaxableTotal = invoice.GetDisbursesTaxableTotal(),
			DisbursesNonTaxableTotal = invoice.GetDisbursesNonTaxableTotal(),
			TaxableActivitiesCost = taxableActivitiesCost,
			TPSTax = tps,
			TVQTax = tvq,
			Total = total,
			PaymentsTotal = paymentsTotal,
			Balance = balance,
		};

		return invoiceSummary;
	}
}
