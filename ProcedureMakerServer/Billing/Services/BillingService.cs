using EFCoreBase.Entities;
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
            AmountPaidDate = DateTime.Now,
        };

        await UpdatePayment(newPayment);


    }

    // when last : return the task from the savechangesAsync
    public Task UpdatePayment(Payment newPayment)
    {

        _procedureContext.Update(newPayment);
        var task = _procedureContext.SaveChangesAsync();

        return task;
    }

    // qu'est-ce tu peux vouloir faire ?
    // Adder des BillingElements
    // Adder une facture 
    // Adder des activities dans une facture
    // adder des payments dans une facture
    // produire un summary 
    // toute updater
}
