using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Repository.ProcedureRepo;
namespace ProcedureMakerServer.Trusts;

public partial class TrustRepository : ProcedureRepositoryContextBase
{
    public async Task<TrustDto> ConstructTrustDto(Guid clientId)
    {
        TrustClientCard trust = await GetTrust(clientId);
        IEnumerable<TrustPaymentDto> payments = await ConstructTrustPaymentsDtos(trust.Id);
        IEnumerable<TrustDisburseDto> disbursements = await ConstructTrustDisbursesDtos(trust.Id);

        TrustDto trustDto = new TrustDto()
        {
            Id = trust.Id,
            ClientId = trust.ClientId,
            Payments = payments.ToList(),
            Disburses = disbursements.ToList(),
        };

        return trustDto;
    }

    private async Task<IEnumerable<TrustPaymentDto>> ConstructTrustPaymentsDtos(Guid trustId)
    {
        List<TrustPayment> paymentsInTrust = await Context.TrustPayments.Where(x => x.TrustId == trustId).ToListAsync();
        IEnumerable<TrustPaymentDto> payments = paymentsInTrust.Select(p => p.ToDto());
        return payments;
    }

    private async Task<IEnumerable<TrustDisburseDto>> ConstructTrustDisbursesDtos(Guid trustId)
    {
        var trustDisburses = await Context.InvoicePayments.Where(x => x.IsPaymentComingFromTrust).ToListAsync();
        var payments = trustDisburses.Select(x => x.ToTrustDisburseDto());
        return payments;
    }

    private async Task<TrustClientCard> GetTrust(Guid clientId)
    {
        TrustClientCard trust = await Context.TrustClientCards
            .Include(x => x.Payments)
            .FirstAsync(x => x.ClientId == clientId);
        return trust;
    }
}
