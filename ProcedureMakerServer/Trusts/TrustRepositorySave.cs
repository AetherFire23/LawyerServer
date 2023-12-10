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

    public async Task AddPayment(Guid clientId, TrustPaymentDto newPayment)
    {
        var currentTrust = await GetTrustAccount(clientId);

        var newTrust = new TrustPayment
        {
            Id = Guid.NewGuid(),
            Trust = currentTrust,
            Amount = newPayment.Amount,
            Date = newPayment.Date,
        };

        Context.TrustPayments.Add(newTrust);
        await Context.SaveChangesAsync();
    }

    public async Task ModifyPayment(Guid clientId, TrustPaymentDto modifiedPayment)
    {
        var payment = await Context.TrustPayments.FirstByIdAsync(modifiedPayment.Id);
        payment.Amount = modifiedPayment.Amount;
        payment.Date = modifiedPayment.Date;

    }

    private async Task<Trust> GetTrustAccount(Guid clientId)
    {
        var accountStatement = await Context.Trusts
            .Include(x => x.Client)
            .Include(x => x.Disburses)
            .Include(x => x.Payments)
            .FirstAsync(x => x.Id == clientId);

        return accountStatement;
    }
}