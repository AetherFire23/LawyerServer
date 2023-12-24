using AutoMapper;
using EFCoreBase.Utils;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Repository.ProcedureRepo;

namespace ProcedureMakerServer.Trusts;

public partial class TrustRepository : ProcedureRepositoryContextBase
{
    public TrustRepository(IMapper mapper, ProcedureContext context) : base(mapper, context)
    {
    }

    public async Task AddFundsToTrust(Guid clientId, TrustPaymentDto trustPayment)
    {
        var client = (await Context.Clients.FirstByIdAsync(clientId)).TrustClientCard;
        //var trustClientCard = await Context.TrustClientCards.FirstAsync(x => x.Id == client.TrustClientCardId);


        var payment = new TrustPayment()
        {
            Amount = trustPayment.Amount,
            Date = trustPayment.Date,
            Trust = client,
        };

        Context.TrustPayments.Add(payment);
        await Context.SaveChangesAsync();
    }

    public async Task UpdateTrustFund(TrustPaymentDto updatedPayment)
    {
        var trustPayment = await Context.TrustPayments.FirstByIdAsync(updatedPayment.Id);
        trustPayment.Amount = updatedPayment.Amount;
        trustPayment.Date = updatedPayment.Date;
    }

    public async Task RemoveTrustFund(Guid trustPaymentId)
    {
        var payment = await Context.TrustPayments.FirstAsync(x => x.Id == trustPaymentId);

        Context.TrustPayments.Remove(payment);
        await Context.SaveChangesAsync();
    }

    private async Task<TrustClientCard> GetTrustAccount(Guid clientId)
    {
        TrustClientCard accountStatement = await Context.TrustClientCards
            .Include(x => x.Client)
            .Include(x => x.Payments)
            .FirstAsync(x => x.Id == clientId);

        return accountStatement;
    }
}