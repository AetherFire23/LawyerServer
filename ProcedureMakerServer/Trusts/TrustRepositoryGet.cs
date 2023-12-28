using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Repository.ProcedureRepo;
namespace ProcedureMakerServer.Trusts;

public partial class TrustRepository : ProcedureRepositoryContextBase
{
	public async Task<TrustClientCardDto> ConstrustTrustClientCard(Guid clientId)
	{
		TrustClientCard trust = await GetTrust(clientId);
		var payments = await ConstructTrustPaymentsDtos(trust.Id);
		var disbursements = await ConstructTrustDisbursesDtos(clientId);

		TrustClientCardDto trustDto = new TrustClientCardDto()
		{
			Id = trust.Id,
			ClientId = trust.ClientId,
			Payments = payments.ToList(),
			Withdraws = disbursements.ToList(),
		};

		return trustDto;
	}

	private async Task<IEnumerable<TrustPaymentDto>> ConstructTrustPaymentsDtos(Guid trustId)
	{
		List<TrustPayment> paymentsInTrust = await Context.TrustPayments.Where(x => x.TrustId == trustId).ToListAsync();
		IEnumerable<TrustPaymentDto> payments = paymentsInTrust.Select(p => p.ToDto());
		return payments;
	}

	private async Task<IEnumerable<TrustWithdrawDto>> ConstructTrustDisbursesDtos(Guid clientId)
	{
		// get all accountstatements from the client
		var payments = await Context.AccountStatements
			.Include(x => x.Case)
			.Include(x => x.Invoices)
				.ThenInclude(x => x.Payments)
			.Where(x => x.Case.ClientId == clientId)
			.SelectMany(x => x.Invoices)
			.SelectMany(x => x.Payments)
			.Where(x => x.IsPaymentComingFromTrust)
			.ToListAsync();

		var trustDisburses = payments.Select(x => x.ToTrustDisburseDto());
		return trustDisburses;
	}

	private async Task<TrustClientCard> GetTrust(Guid clientId)
	{
		TrustClientCard trust = await Context.TrustClientCards
			.Include(x => x.Payments)
			.FirstAsync(x => x.ClientId == clientId);
		return trust;
	}
}
