using DocumentFormat.OpenXml.InkML;
using EFCoreBase.Entities;
using EFCoreBase.Utils;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Billing.ChangeHandler;
using ProcedureMakerServer.Interfaces;

namespace ProcedureMakerServer.Billing.Services;


// not a service in fact. It is just a normal repository.
// IconTextReference extension?
public class BillingService : IBillingService, IContextReference
{
    public ProcedureContext ProcedureContext { get; set; }
    private readonly ICaseRepository _caseRepository;

    public BillingService(ProcedureContext procedureContext,
        ICaseRepository caseRepository)
    {
        ProcedureContext = procedureContext;
        _caseRepository = caseRepository;
    }
    
    public async Task UpdateInvoice(InvoiceDto updatedInvoice)
    {
        await this.HandleChangeDto(updatedInvoice.Activities, ProcedureContext.Activities, HandleActivityUpdate);
        await this.HandleChange(updatedInvoice.Payments, ProcedureContext.Payments, HandlePaymentUpdate);
    }

    private async Task HandleActivityUpdate(ActivityDto updatedEntity, Activity activity)
    {
        bool hasChangedBillingElement = activity.BillingElementId != updatedEntity.BillingElement.Id;
        if (hasChangedBillingElement)
        {
            var billingElement = await ProcedureContext.FirstById<BillingElement>(updatedEntity.BillingElement.Id);
            updatedEntity.CopyTo(activity, billingElement);
        }
    }

    private Task HandlePaymentUpdate(Payment updatedPayment, Payment storedPayment)
    {
        updatedPayment.CopyTo(storedPayment);

        return Task.CompletedTask;
    }


    // quoi faire si sont de type diff/rent ?
    //private async Task HandleChange<T>(IEnumerable<T> updatedEntities, IEnumerable<T> storedEntities, Action<T, T> onUpdate)
    //    where T : EntityBase
    //{
    //    var (removed, updated) = EntitiesRefesher.GetRefreshResultGeneric<T>(updatedEntities, storedEntities);

    //    var set = _procedureContext.Set<T>();
    //    foreach (var rem in removed)
    //    {
    //        var entity = await set.FirstAsync(x => x.Id == rem.Id);
    //        _procedureContext.Set<T>().Remove(entity);
    //    }
    //    foreach (var upd in updated)
    //    {
    //        var entity = await set.FirstAsync(x => x.Id == upd.Id);
    //        onUpdate(upd, entity);
    //    }
    //}

    private async Task HandlePaymentsChange(InvoiceDto updatedInvoice)
    {
        var currentPayments = ProcedureContext.Payments.AsNoTracking()
            .Where(x => x.InvoiceId == updatedInvoice.Id).ToList();

        var (removed, updated) = EntitiesRefesher.GetRefreshResult(updatedInvoice.Payments, currentPayments);

        foreach (var payment in removed) // on remove can be wholly abstracted
        {
            var entity = await ProcedureContext.FirstById<Payment>(payment.Id);

            payment.Id = entity.Id;// wtf ?
            ProcedureContext.Payments.Remove(entity);
        }

        foreach (Payment payment in updated) //CopyToCannot
        {
            var updatedPayment = updatedInvoice.Payments.First(x => x.Id == payment.Id);
            updatedPayment.CopyTo(payment);
        }
    }

    private async Task HandleActivitesChange(InvoiceDto updatedInvoice)
    {
        var currentActivities = ProcedureContext.Activities.AsNoTracking()
         .Where(x => x.InvoiceId == updatedInvoice.Id).ToList();

        var (removed, updated) = EntitiesRefesher.GetRefreshResult(updatedInvoice.Activities, currentActivities);

        foreach (var activity in removed)
        {
            var entity = await ProcedureContext.FirstById<Activity>(activity.Id);

            activity.Id = entity.Id;
            ProcedureContext.Activities.Remove(entity);
        }

        foreach (Activity activity in updated)
        {
            var updatedActivity = updatedInvoice.Activities.First(x => x.Id == activity.Id);

            bool hasChangedBillingElement = activity.BillingElementId != updatedActivity.BillingElement.Id;
            if (hasChangedBillingElement)
            {
                var billingElement = await ProcedureContext.FirstById<BillingElement>(activity.BillingElementId);
                updatedActivity.CopyTo(activity, billingElement);
            }
        }

    }

    public async Task<AccountStatementDto> MapAccountStatementDto(Guid caseId)
    {
        AccountStatement accountStatement = await ProcedureContext.AccountStatements

            .Include(x => x.Invoices)
                .ThenInclude(x => x.Activities)

            .Include(x => x.Invoices)
                .ThenInclude(x => x.Payments)

            .Include(x => x.Lawyer)
                .ThenInclude(x => x.LawyerBillingOptions)
                    .ThenInclude(x => x.BillingElements)

            .FirstAsync(x => x.CaseId == caseId);


        var invoices = new List<InvoiceDto>();
        foreach (var invoice in accountStatement.Invoices)
        {
            var invoiceDto = await MapInvoiceDto(invoice.Id);
            invoices.Add(invoiceDto);
        }

        var dto = new AccountStatementDto()
        {
            Id = accountStatement.Id,
            Invoices = invoices,
            LawyerBillingElements = accountStatement.Lawyer.LawyerBillingOptions.BillingElements.Select(x => x.ToDto()).ToList(),
        };

        // accountStatement

        return dto;
    }
    public async Task AddInvoice(Guid caseId)
    {
        var accountStatement = await ProcedureContext.AccountStatements.FirstAsync(x => x.CaseId == caseId);

        var Invoice = new Invoice()
        {
            AccountStatement = accountStatement,
            InvoiceStatuses = InvoiceStatuses.InPreparation,
        };

        await ProcedureContext.Invoices.AddAsync(Invoice);
        await ProcedureContext.SaveChangesAsync();
    }

    public async Task AddPayment(PaymentCreationRequest paymentCreation)
    {
        var invoice = await ProcedureContext.Invoices.FirstAsync(x => x.Id == paymentCreation.InvoiceId);
        var payment = new Payment()
        {
            AmountPaid = paymentCreation.AmountPaid,
            AmountPaidDate = paymentCreation.AmountPaidDate,
            Invoice = invoice,
            InvoiceId = invoice.Id,
        };

        await ProcedureContext.AddAsync(payment);
        await ProcedureContext.SaveChangesAsync();
    }

    private async Task<InvoiceDto> MapInvoiceDto(Guid invoiceId)
    {
        var invoice = ProcedureContext.Invoices.First(x => x.Id == invoiceId);
        var invoiceDto = new InvoiceDto()
        {
            Id = invoice.Id,
            Activities = invoice.Activities.Select(x => x.ToDto()).ToList(),
            Payments = invoice.Payments.ToList(),
            InvoiceStatus = invoice.InvoiceStatuses,

        };

        return invoiceDto;
    }


    public async Task AddActivityToInvoice(ActivityCreation activityCreation)
    {
        var invoice = await ProcedureContext.Invoices
            .Include(x => x.AccountStatement)
                .ThenInclude(x => x.Lawyer)
            .FirstAsync(x => activityCreation.InvoiceId == x.Id);

        var billingElement = await ProcedureContext.FirstById<BillingElement>(activityCreation.BillingElementId);

        var activity = new Activity()
        {
            BillingElement = billingElement,
            HoursWorked = activityCreation.HoursWorked,
            Invoice = invoice,
            HasPersonalizedBillingElement = activityCreation.HasPersonalizedBillingElement,
        };

        await ProcedureContext.Activities.AddAsync(activity);
        await ProcedureContext.SaveChangesAsync();
    }


    // either to the lawyer or the account summary
    // the actual mapping should be done at summary creation or at account statement dto creation thing
    public async Task AddBillingElement(BillingElementCreationRequest billingElementCreation)
    {
        var statement = await ProcedureContext.AccountStatements
            .Include(x => x.Lawyer)
                .ThenInclude(x => x.LawyerBillingOptions)
            .FirstAsync(x => x.Id == billingElementCreation.AccountStatementId);

        var billingElement = new BillingElement()
        {
            ActivityName = billingElementCreation.ActivityName,
            Amount = billingElementCreation.Amount,
            IsHourlyRate = billingElementCreation.IsHourlyRate,
            IsPersonalizedBillingElement = billingElementCreation.IsPersonalizedBillingElement,
            AccountStatementGuid = statement.Id,
            Lawyer = statement.Lawyer,
            LawyerBillingOptions = statement.Lawyer.LawyerBillingOptions,
        };

        await ProcedureContext.AddAsync(billingElement);
        await ProcedureContext.SaveChangesAsync();
    }



    // when last : return the task from the savechangesAsync
    // its the only await statement : unnecessary cos the await states suspends the method in case 
    // But nothing left in the method:

    // qu'est-ce tu peux vouloir faire ?
    // Adder des BillingElements
    // Adder une facture 
    // Adder des activities dans une facture
    // adder des payments dans une facture


    // j'peux produce un summary mais pas le show
    // produire un summary a plus tard i guess
    // toute updater
}


public static class SetExtensions
{
    public static async Task<T> FirstById<T>(this DbContext self, Guid id) where T : EntityBase
    {
        var set = await self.Set<T>().FirstAsync(x => x.Id == id);
        return set;
    }
}