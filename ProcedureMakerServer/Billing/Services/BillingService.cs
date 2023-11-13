using EFCoreBase.Entities;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Interfaces;
using ProcedureMakerServer.Repository;
using ProcedureMakerServer.Services;
using ProcedureMakerServer.Utils;

namespace ProcedureMakerServer.Billing.Services;

public class BillingService : IBillingService
{
    private readonly ProcedureContext _procedureContext;
    private readonly ICaseRepository _caseRepository;

    public BillingService(ProcedureContext procedureContext,
        ICaseRepository caseRepository)
    {
        _procedureContext = procedureContext;
        _caseRepository = caseRepository;
    }

    // since accountstaetmnt is always created on creating a case and removed when deleting a case only the invoices getu pdated 
    public async Task UpdateInvoices(AccountStatement upToDateStatement)
    {
        var statement = _procedureContext.AccountStatements.First(x => x.Id == upToDateStatement.Id);

        var inv = new Invoice()
        {
            AccountStatement = statement,
        };
        await _procedureContext.Invoices.AddAsync(inv);
        await _procedureContext.SaveChangesAsync();

        var np = new Payment()
        {
            AmountPaid = 1,
            Invoice = statement.Invoices.First(),
        };
        await _procedureContext.Payments.AddAsync(np);
        await _procedureContext.SaveChangesAsync();


        var newPayment = new Payment()
        {
            Id = np.Id,
            AmountPaid = 4200,
            Invoice = inv,
            AmountPaidDate = DateTime.UtcNow,
        };


        //
        var info = _procedureContext.Entry(np);
        info.State = EntityState.Detached;

        _procedureContext.Update(newPayment);
        await _procedureContext.SaveChangesAsync();


        var regotten = _procedureContext.Payments.First(x => x.Id == newPayment.Id);


        // await UpdatePayment(np, newPayment);


    }

    // when last : return the task from the savechangesAsync
    public async Task UpdatePayment(Payment old, Payment newPayment)
    {
        var trackinfo = _procedureContext.Entry(newPayment);
        trackinfo.State = EntityState.Detached;
        await _procedureContext.SaveChangesAsync();


        _procedureContext.Update(newPayment);
        await _procedureContext.SaveChangesAsync();
    }

    // qu'est-ce tu peux vouloir faire ?
    // Adder des BillingElements
    // Adder une facture 
    // Adder des activities dans une facture
    // adder des payments dans une facture
    // produire un summary 
    // toute updater
}
