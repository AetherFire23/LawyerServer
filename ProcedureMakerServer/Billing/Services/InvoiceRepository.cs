using AutoMapper;
using EFCoreBase.Utils;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Billing.InvoiceDtos;
using ProcedureMakerServer.Billing.StatementDtos;
using ProcedureMakerServer.Billing.StatementEntities;
using ProcedureMakerServer.Repository.ProcedureRepo;
namespace ProcedureMakerServer.Billing.Services;

public class InvoiceRepository : ProcedureRepositoryContextBase
{
    private AccountStatementRepository _accountStatementRepository { get; set; }
    public InvoiceRepository(IMapper mapper, ProcedureContext context, AccountStatementRepository accountStatemetnRepo) : base(mapper, context)
    {
        _accountStatementRepository = accountStatemetnRepo;
    }

    public async Task CreateInvoice(Guid caseId)
    {
        var accountStatement = await _accountStatementRepository.GetAccountStatementByCaseId(caseId);

        // when you create an invoice,
        // make a copy of the current defaultHourlyElement
        // the reason is if you change the default hourly element,
        // it will change price for all cases (even ongoing ones)
        // Which is absolutely illegal. 
        var currentLawyerBillingElement = accountStatement.Case.ManagerLawyer.DefaultHourlyElement;
        var currentBillingElementCopy = new BillingElement
        {
            ActivityName = currentLawyerBillingElement.ActivityName,
            Amount = currentLawyerBillingElement.Amount,
            IsDisburse = currentLawyerBillingElement.IsDisburse,
            IsHourlyRate = currentLawyerBillingElement.IsHourlyRate,
        };

        Context.BillingElements.Add(currentBillingElementCopy);
        await Context.SaveChangesAsync();

        var newInvoice = new Invoice
        {
            AccountStatement = accountStatement,
            DefaultBillingElement = currentBillingElementCopy,
        };

        Context.Invoices.Add(newInvoice);
        await Context.SaveChangesAsync();
    }

    public async Task UpdateInvoiceProperties(InvoiceDto dto)
    {
        var invoice = await Context.Invoices.FirstByIdAsync(dto.Id);

        invoice.InvoiceStatus = dto.InvoiceStatus;

        await Context.SaveChangesAsync();
    }

    // a payment is a disburse for a Trust (mapped inside the MapTrustDisburse Method)
    public async Task AddInvoicePayment(Guid invoiceId, InvoicePaymentDto invoicePaymentDto)
    {
        var invoice = await Context.Invoices.FirstByIdAsync(invoiceId);

        var invoicePayment = new InvoicePayment
        {
            Invoice = invoice,
            AmountPaid = invoicePaymentDto.AmountPaid,
            AmountPaidDate = invoicePaymentDto.AmoundPaidDate,
            IsPaymentComingFromTrust = invoicePaymentDto.IsPaymentComingFromTrust,
        };

        invoicePayment.CopyFromDto(invoicePaymentDto);

        Context.InvoicePayments.Add(invoicePayment);
        await Context.SaveChangesAsync();
    }

    public async Task UpdateInvoicePayment(InvoicePaymentDto invoicePaymentDto)
    {
        var invoicePayment = await Context.InvoicePayments.FirstByIdAsync(invoicePaymentDto.Id);

        invoicePayment.CopyFromDto(invoicePaymentDto);

        await Context.SaveChangesAsync();
    }

    public async Task DeleteInvoicePayment(Guid invoicePaymentId)
    {
        var invoicePayment = await Context.InvoicePayments.FirstByIdAsync(invoicePaymentId);

        Context.InvoicePayments.Remove(invoicePayment);

        await Context.SaveChangesAsync();
    }

    public async Task AddActivity(Guid invoiceId, Guid billingElementId, ActivityDto createdActivityDto) // requires a billing element I guess
    {
        var billingElement = await Context.BillingElements.FirstByIdAsync(billingElementId);
        var invoice = await Context.Invoices.FirstByIdAsync(invoiceId);

        var newActivity = new Activity()
        {
            ActivityDescription = createdActivityDto.ActivityDescription,
            BillingElement = billingElement,
            Quantity = createdActivityDto.HoursWorked,
            Invoice = invoice,
        };

        Context.Activities.Add(newActivity);
        await Context.SaveChangesAsync();
    }

    public async Task UpdateActivity(Guid billingElementId, ActivityDto activityDto)
    {
        var billingElement = await Context.BillingElements.FirstByIdAsync(billingElementId);
        var activity = await Context.Activities.FirstByIdAsync(activityDto.Id);
        activity.BillingElement = billingElement;
        activity.ActivityDescription = activityDto.ActivityDescription;
        activity.Quantity = activityDto.HoursWorked;

        await Context.SaveChangesAsync();
    }

    public async Task RemoveActivity(Guid activityId)
    {
        var activity = await Context.Activities
            .Include(x => x.BillingElement)
            .FirstAsync(x => x.Id == activityId);

        Context.BillingElements.Remove(activity.BillingElement);
        Context.Activities.Remove(activity);
        await Context.SaveChangesAsync();
    }

    public async Task AddBillingElement(BillingElementDto dto, Guid lawyerId)
    {
        var lawyer = await Context.Lawyers.FirstByIdAsync(lawyerId);
        var billingElement = new BillingElement()
        {
            ManagerLawyer = lawyer,
            ActivityName = dto.ActivityName,
            Amount = dto.Amount,
            IsHourlyRate = dto.IsHourlyRate,
            IsDisburse = dto.IsDisburse,
        };

        Context.BillingElements.Add(billingElement);
        await Context.SaveChangesAsync();
    }

    public async Task UpdateBillingElement(BillingElementDto dto)
    {
        var billingElement = await Context.BillingElements.FirstByIdAsync(dto.Id);

        billingElement.ActivityName = dto.ActivityName;
        billingElement.Amount = dto.Amount;
        billingElement.IsHourlyRate = dto.IsHourlyRate;
        billingElement.IsDisburse = dto.IsDisburse;

        await Context.SaveChangesAsync();
    }

    public async Task RemoveBillingElement(Guid billingElementId)
    {
        var billingElement = await Context.BillingElements.FirstByIdAsync(billingElementId);
        Context.BillingElements.Remove(billingElement);
    }
}
