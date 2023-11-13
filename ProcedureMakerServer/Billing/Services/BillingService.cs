using DocumentFormat.OpenXml.Drawing.Diagrams;
using EFCoreBase.Entities;
using EFCoreBase.RefresherService;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProcedureMakerServer.Interfaces;
using ProcedureMakerServer.Services;

namespace ProcedureMakerServer.Billing.Services;

public class BillingService : IBillingService
{
    private readonly ProcedureContext _procedureContext;
    private readonly ICaseRepository _caseRepository;
    private readonly IProcedureEntityRefresher _refresher;

    public BillingService(ProcedureContext procedureContext,
        ICaseRepository caseRepository, IProcedureEntityRefresher refresher)
    {
        _procedureContext = procedureContext;
        _caseRepository = caseRepository;
        _refresher = refresher;
    }

    // since accountstaetmnt is always created on creating a case and removed when deleting a case only the invoices getu pdated 
    public async Task UpdateAccountStatement(AccountStatement upToDateStatement)
    {
        foreach (var item in upToDateStatement.Invoices)
        {
            await UpdateInvoice(item);
        }

        await _procedureContext.SaveChangesAsync();
    }

    public async Task UpdateInvoice(Invoice upToDateInvoice)
    {
        var currentActivities = _procedureContext.Activities.AsNoTracking()
                .Where(x => x.InvoiceId == upToDateInvoice.Id).ToList();

        RemoveMissingAddCreated(upToDateInvoice.Activities, currentActivities);
    }

    public IEnumerable<T> RemoveMissingAddCreated<T>(IEnumerable<T> upToDate, IEnumerable<T> old) where T : EntityBase
    {
        var set = _procedureContext.Set<T>();
        foreach (var oldItem in old)
        {
            // if old is not in new, it was deleted
            var oldEntityFromNew = upToDate.FirstOrDefault(x => x.Id == oldItem.Id);

            bool isDeletedEntity = oldEntityFromNew is null;
            if (isDeletedEntity)
            {
                var asTracked = set.FirstOrDefault(x => x.Id == oldItem.Id);
                set.Remove(asTracked);
            }
        }

        foreach (var upToDateItem in upToDate)
        {

            // if new is not present in old, it was created.
            var newEntityFromOld = old.FirstOrDefault(x => x.Id == upToDateItem.Id);

            bool isNewEntity = newEntityFromOld is null;
            if (isNewEntity)
            {
                set.Add(upToDateItem);
            }
        }

        var common = old.Where(x => upToDate.Any(y => x.Id == y.Id));

        _procedureContext.SaveChanges();
        return common;
    }

    public async Task<AccountStatement> GetAccountStatement(Guid caseId)
    {
        var accountStatement = await _procedureContext.AccountStatements
            .Include(x=> x.Invoices)
                .ThenInclude(x=> x.Activities)
            .Include(x => x.Invoices)
                .ThenInclude(x => x.Payments)
            .FirstAsync(x => x.CaseId == caseId);
        return accountStatement;
    }

  



    // when last : return the task from the savechangesAsync
    // its the only await statement : unnecessary cos the await states suspends the method in case 
    // But nothing left in the method:

    // qu'est-ce tu peux vouloir faire ?
    // Adder des BillingElements
    // Adder une facture 
    // Adder des activities dans une facture
    // adder des payments dans une facture
    // produire un summary 
    // toute updater
}
