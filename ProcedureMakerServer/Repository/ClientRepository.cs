using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using EFCoreBase.Entities;
using EFCoreBase.Utils;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg.Sig;
using ProcedureMakerServer.Billing.InvoiceDtos;
using ProcedureMakerServer.Billing.StatementDtos;
using ProcedureMakerServer.Billing.StatementEntities;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Repository.ProcedureRepo;
using ProcedureMakerServer.Trusts;
using ProcedureShared.Dtos;
namespace ProcedureMakerServer.Repository;

public class ClientRepository : ProcedureCrudBase<Client>
{
	private readonly TrustRepository _trustRepository;
	private readonly CaseRepository _caseRepository;
	private readonly LawyerRepository _lawyerRepository;
	public ClientRepository(ProcedureContext context,
						 IMapper mapper,
						 TrustRepository trustRepository,
						 CaseRepository caseRepository,
						 LawyerRepository lawyerRepository) : base(context, mapper)
	{
		_trustRepository = trustRepository;
		_caseRepository = caseRepository;
		_lawyerRepository = lawyerRepository;
	}

	public async Task CreateClient(Guid lawyerId, ClientDto clientDto)
	{
		var lawyer = await Context.Lawyers.FirstByIdAsync(lawyerId);

		var client = new Client
		{
			Lawyer = lawyer,
		};

		client.CopyFromCourtMember(clientDto);

		Context.Clients.Add(client);
		await Context.SaveChangesAsync();

		var createdTrust = await CreateTrust(client.Id);

		client.TrustClientCard = createdTrust;
		await Context.SaveChangesAsync();


		var regottenClient = await Context.Clients.FirstAsync(x => x.Id == client.Id);
	}

	public async Task UpdateClientInfo(ClientDto client)
	{
		Client entity = await GetEntityById(client.Id);

		entity.CopyFromCourtMember(client);

		await Context.SaveChangesAsync();
	}

	public async Task RemoveClient(Guid clientId)
	{
		var client = await Context.Clients.FirstByIdAsync(clientId);

		Context.Clients.Remove(client);
		await Context.SaveChangesAsync();
	}

	/// <summary> trust is created when client is created. Returns the created trust as tracked  </summary>
	private async Task<TrustClientCard> CreateTrust(Guid clientId)
	{
		var client = await Context.Clients.FirstOrDefaultAsync(x => x.Id == clientId);
		var trustCard = new TrustClientCard
		{
			Client = client,
		};

		Context.TrustClientCards.Add(trustCard);
		await Context.SaveChangesAsync();

		return trustCard;
	}

	public async Task<List<ClientDto>> MapClientDtos(List<Client> clients)
	{
		var clientDtos = new List<ClientDto>();

		foreach (var client in clients)
		{
			var clientDto = await MapClientAndCaseDto(client.Id);
			clientDtos.Add(clientDto);
		}
		return clientDtos;
	}
	public async Task<ClientDto> MapClientAndCaseDto(Guid clientId)
	{
		var client = await Context.Clients
			.Include(x => x.Cases)
			.FirstAsync(x => x.Id == clientId);

		var trustClientCardDto = await _trustRepository.ConstrustTrustClientCard(clientId);
		var clientDto = new ClientDto
		{
			TrustClientCard = trustClientCardDto,
		};
		var caseDtos = await this.MapCaseDtos(client.Cases.Select(x => x.Id), clientDto);

		// map cases after because there is a circular dependency
		clientDto.Cases = caseDtos;
		clientDto.CopyFromCourtMember(client);

		return clientDto;
	}

	private async Task<List<CaseParticipantDto>> GetCaseParticipantsDto(Guid caseId)
	{
		var lcase = await Context.Cases
			.Include(c => c.CaseParticipants)
			.Include(c => c.Client)
			.Include(c => c.ManagerLawyer)
			.Include(x => x.AccountStatement)
				.ThenInclude(x => x.Invoices)
			.FirstAsync(c => c.Id == caseId);

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

	public async Task<List<CaseDto>> MapCaseDtos(IEnumerable<Guid> caseIds, ClientDto clientDto)
	{
		var caseDtos = new List<CaseDto>();

		foreach (var caseId in caseIds)
		{
			var caseDto = await MapCaseDto(caseId, clientDto);
			caseDtos.Add(caseDto);
		}
		return caseDtos;
	}

	// I have circular depdendency between case and client.
	// I need to map case without lawyer first and then map lawyer and add it to caseDto
	public async Task<CaseDto> MapCaseDto(Guid caseId, ClientDto clientDto)
	{
		var lcase = await _caseRepository.GetCase(caseId);
		var participants = await GetCaseParticipantsDto(caseId);
		var invoices = await MapInvoicesForCase(lcase.AccountStatement.Id);
		var lawyerDto = await _lawyerRepository.MapLawyerDto(lcase.ManagerLawyerId);

		var caseDto = new CaseDto
		{
			Id = lcase.Id,
			ManagerLawyer = lawyerDto,
			Participants = participants,
			CaseNumber = lcase.CaseNumber,
			CourtAffairNumber = lcase.CourtAffairNumber,
			CourtNumber = lcase.CourtNumber,
			ChamberName = lcase.ChamberName,
			DistrictName = lcase.DistrictName,
			Invoices = invoices,
			Client = clientDto,
		};

		await PopulatePlaintiffAndDefender(caseDto);

		return caseDto;
	}

	// Whole goal is to map the whole Client (since it is necessary to get the caseDto
	public async Task<CaseDto> GetCaseDto(Guid caseId)
	{
		// ifner the client from the caseId
		var client = (await Context.Cases
			.Include(x => x.Client)
			.FirstAsync(x => x.Id == caseId))
			.Client;

		var clientDto = await MapClientAndCaseDto(client.Id);
		var caseDto = clientDto.Cases.First(x => x.Id == caseId);

		return caseDto;
	}
}