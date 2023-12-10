using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Repository.ProcedureRepo;
namespace ProcedureMakerServer.Trusts;

public partial class TrustRepository : ProcedureRepositoryContextBase
{
    public async Task<TrustDto> ConstructTrustDto(Guid clientId)
    {
        Trust trust = await GetTrust(clientId);
        var payments = await ConstructTrustPaymentsDtos(trust.Id);
        var disbursements = await ConstructTrustDisbursesDtos(trust.Id);

        var trustDto = new TrustDto()
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
        List<TrustDisburse> trustDisburses = await Context.TrustDisburses.Where(x => x.TrustId == trustId).ToListAsync();
        IEnumerable<TrustDisburseDto> payments = trustDisburses.Select(d => d.ToDto());
        return payments;
    }

    private async Task<Trust> GetTrust(Guid clientId)
    {
        var trust = await Context.Trusts
            .Include(x => x.Payments)
            .Include(x => x.Disburses)
            .FirstAsync(x => x.ClientId == clientId);
        return trust;
    }
}
