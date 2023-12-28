using AutoMapper;
using EFCoreBase.Entities;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Authentication.Interfaces;
using ProcedureMakerServer.Billing.InvoiceDtos;
using ProcedureMakerServer.Billing.Services;
using ProcedureMakerServer.Billing.StatementDtos;
using ProcedureMakerServer.Billing.StatementEntities;
using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Interfaces;
using ProcedureMakerServer.Repository.ProcedureRepo;
using System.Net.WebSockets;

namespace ProcedureMakerServer.Repository;

public class CaseRepository : ProcedureCrudBase<Case>
{

	private ProcedureContext c2;
	public CaseRepository(ProcedureContext context, IMapper mapper) : base(context, mapper)
	{
		c2 = context;
	}


	// asks for guids or for the actual entity?
	public async Task<List<CaseDto>> MapCaseDtos(IEnumerable<Guid> caseIds)
	{
		var caseDtos = new List<CaseDto>();

		foreach (var caseId in caseIds)
		{
			var caseDto = await MapCaseDto(caseId);
			caseDtos.Add(caseDto);
		}
		return caseDtos;
	}

	public async Task<CaseDto> MapCaseDto(Guid caseId)
	{
		var lcase = await GetEntityById(caseId);
		var participants = await GetCaseParticipantsDto(caseId);
		var invoices = await MapInvoicesForCase(lcase.AccountStatement.Id);

		var caseDto = new CaseDto
		{
			Id = lcase.Id,
			Client = lcase.Client,
			ManagerLawyer = lcase.ManagerLawyer,
			Participants = participants,
			CaseNumber = lcase.CaseNumber,
			CourtAffairNumber = lcase.CourtAffairNumber,
			CourtNumber = lcase.CourtNumber,
			ChamberName = lcase.ChamberName,
			DistrictName = lcase.DistrictName,
			Invoices = invoices,
		};

		await PopulatePlaintiffAndDefender(caseDto);

		return caseDto;
	}
	public override async Task<Case> GetEntityById(Guid id)
	{
		Case lcase = await c2.Cases
			.Include(c => c.CaseParticipants)
			.Include(c => c.Client)
			.Include(c => c.ManagerLawyer)
			.Include(x => x.AccountStatement)
				.ThenInclude(x => x.Invoices)
			.FirstAsync(c => c.Id == id);

		return lcase;
	}
	public async Task<List<string>> GetEmailsToNotify(Guid caseId)
	{
		var notifiableInCase = await Context.CaseParticipants
			.Where(x => x.CaseId == caseId)
			.Where(c => c.MustNotify)
			.Where(c => c.NotificationEmail != "")
			.Select(c => c.NotificationEmail)
			.ToListAsync();

		return notifiableInCase;
	}
	private async Task<List<CaseParticipantDto>> GetCaseParticipantsDto(Guid caseId)
	{
		var lcase = await GetEntityById(caseId);
		var participants = lcase.CaseParticipants.ToList()
			.Select(x => x.ToDto())
			.ToList();

		return participants;
	}

	private async Task<List<InvoiceDto>> MapInvoicesForCase(Guid accountStatementId)
	{
		var invoices = await Context.Invoices
			.Include(x => x.DefaultBillingElement)
			.Include(x => x.AccountStatement)
			.Where(x => x.AccountStatementId == accountStatementId)
			.ToListAsync();

		var invoiceDtos = new List<InvoiceDto>();
		foreach (var invoice in invoices)
		{
			var invoiceDto = await MapInvoiceDto(invoice);
			invoiceDtos.Add(invoiceDto);
		}

		return invoiceDtos;
	}

	public async Task<InvoiceDto> MapInvoiceDto(Invoice? invoice)
	{
		var activityDtos = await MapActivityDtos(invoice.Id);
		var paymentDtos = await MapInvoicePaymentDtos(invoice.Id);
		var invoiceDto = new InvoiceDto()
		{
			Id = invoice.Id,
			InvoiceStatus = invoice.InvoiceStatus,
			Activities = activityDtos,
			Payments = paymentDtos,
		};
		return invoiceDto;
	}


	private async Task<List<ActivityDto>> MapActivityDtos(Guid invoiceId)
	{
		var activities = await Context.Activities
			.Where(x => x.InvoiceId == invoiceId)
			.ToListAsync();

		var activityDtos = new List<ActivityDto>();
		foreach (var activity in activities)
		{
			var activityDto = new ActivityDto
			{
				Id = activity.Id,
				Description = activity.Description,
				Quantity = activity.Quantity,
				CostInDollars = activity.CostInDollars,
				IsDisburse = activity.IsDisburse,
				IsTaxable = activity.IsTaxable
			};

			activityDtos.Add(activityDto);
		}

		return activityDtos;
	}
	private async Task<List<InvoicePaymentDto>> MapInvoicePaymentDtos(Guid invoiceId)
	{
		var invoicePayments = await Context.InvoicePayments
			.Where(x => x.InvoiceId == invoiceId)
			.ToListAsync();

		var invoicePaymentDtos = new List<InvoicePaymentDto>();
		foreach (var invoicePayment in invoicePayments)
		{
			var invoicePaymentDto = new InvoicePaymentDto()
			{
				Id = invoicePayment.Id,
				AmoundPaidDate = invoicePayment.AmountPaidDate,
				AmountPaid = invoicePayment.AmountPaid,
				IsPaymentComingFromTrust = invoicePayment.IsPaymentComingFromTrust,
			};
			invoicePaymentDtos.Add(invoicePaymentDto);
		}

		return invoicePaymentDtos;
	}
	private async Task PopulatePlaintiffAndDefender(CaseDto caseDto)
	{
		caseDto.Plaintiff = await Context.CaseParticipants.FirstOrDefaultAsync(x => x.CaseId == caseDto.Id && x.CourtRole == Enums.CourtRoles.Plaintiff);
		caseDto.Defender = await Context.CaseParticipants.FirstOrDefaultAsync(x => x.CaseId == caseDto.Id && x.CourtRole == Enums.CourtRoles.Defender);
	}
}
