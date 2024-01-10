using AutoMapper;
using EFCoreBase.Utils;
using HtmlRenderFun;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Billing.InvoiceDtos;
using ProcedureMakerServer.Billing.StatementDtos;
using ProcedureMakerServer.Billing.StatementEntities;
using ProcedureMakerServer.Constants;
using ProcedureMakerServer.HtmlToPdf;
using ProcedureMakerServer.Repository;
using ProcedureMakerServer.Repository.ProcedureRepo;
using ProcedureShared.Dtos;

namespace ProcedureMakerServer.Billing.Services;

public class InvoiceRepository : ProcedureRepositoryContextBase
{
    private AccountStatementRepository _accountStatementRepository;
    private readonly CaseContextRepository _caseRepository;
    private readonly ProcedureHtmlRenderer _procedureHtmlRenderer;
    private readonly HtmlToPdfConverter _htmlToPdfConverter;

    public InvoiceRepository(IMapper mapper, ProcedureContext context, AccountStatementRepository accountStatemetnRepo, CaseContextRepository caseRepository, ProcedureHtmlRenderer procedureHtmlRenderer, HtmlToPdfConverter htmlToPdfConverter) : base(mapper, context)
    {
        _accountStatementRepository = accountStatemetnRepo;
        _caseRepository = caseRepository;
        _procedureHtmlRenderer = procedureHtmlRenderer;
        _htmlToPdfConverter = htmlToPdfConverter;
    }

    public async Task<Guid> CreateInvoice(Guid caseId)
    {
        var accountStatement = await _accountStatementRepository.GetAccountStatementByCaseId(caseId);

        // need to increment the lawyers bills emitted count for correct accountability
        accountStatement.Lawyer.BillsEmittedCount++;

        // When you create an invoice,
        // Make a copy of the current defaultHourlyElement
        // The reason is if you change the default hourly element,
        // It will change price for all cases (even ongoing ones)
        // Which is absolutely illegal. 
        var currentLawyerBillingElement = accountStatement.Case.ManagerLawyer.DefaultHourlyElement;
        var currentBillingElementCopy = new BillingElement
        {
            ManagerLawyer = accountStatement.Case.ManagerLawyer,
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
            InvoiceStatus = InvoiceStatuses.InPreparation,
            InvoiceNumber = accountStatement.Lawyer.BillsEmittedCount,
        };

        Context.Invoices.Add(newInvoice);
        await Context.SaveChangesAsync();

        return newInvoice.Id;
    }

    public async Task UpdateInvoiceProperties(InvoiceDto dto)
    {
        var invoice = await Context.Invoices.FirstByIdAsync(dto.Id);

        invoice.InvoiceStatus = dto.InvoiceStatus;

        await Context.SaveChangesAsync();
    }

    public async Task ArchiveInvoice(Guid invoiceId)
    {
        var invoice = await Context.Invoices.FirstAsync(x => x.Id == invoiceId);

        Context.Invoices.Remove(invoice);
        await Context.SaveChangesAsync();
    }

    // a payment is a disburse for a Trust (mapped inside the MapTrustDisburse Method)
    public async Task<Guid> AddInvoicePayment(Guid invoiceId)
    {
        var invoice = await Context.Invoices.FirstByIdAsync(invoiceId);

        var invoicePayment = new InvoicePayment
        {
            Invoice = invoice,
        };

        Context.InvoicePayments.Add(invoicePayment);
        await Context.SaveChangesAsync();
        return invoicePayment.Id;
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

    // should not be able to modify or create afterwards for UI reasons.
    public async Task<Guid> AddActivity(Guid invoiceId, bool isDisburse, bool isTaxable) // requires a billing element I guess
    {
        var invoice = await Context.Invoices.FirstByIdAsync(invoiceId);
        var newActivity = new Activity()
        {
            Invoice = invoice,
            IsDisburse = isDisburse,
            IsTaxable = isTaxable
        };

        Context.Activities.Add(newActivity);
        await Context.SaveChangesAsync();
        return newActivity.Id;
    }

    public async Task UpdateActivity(ActivityDto activityDto)
    {
        var activity = await Context.Activities.FirstByIdAsync(activityDto.Id);
        activity.Description = activityDto.Description;
        activity.Quantity = activityDto.Quantity;
        activity.CostInDollars = activityDto.CostInDollars;
        activity.CreatedAt = activityDto.CreatedAt;
        activity.IsDisburse = activityDto.IsDisburse;
        activity.IsTaxable = activityDto.IsTaxable;

        await Context.SaveChangesAsync();
    }

    public async Task RemoveActivity(Guid activityId)
    {
        var activity = await Context.Activities
            .FirstAsync(x => x.Id == activityId);

        Context.Activities.Remove(activity);
        await Context.SaveChangesAsync();
    }

    public async Task<Guid> AddBillingElement(Guid lawyerId)
    {
        var lawyer = await Context.Lawyers.FirstByIdAsync(lawyerId);


        var billingElement = new BillingElement
        {
            ManagerLawyer = lawyer,
        };

        Context.BillingElements.Add(billingElement);
        await Context.SaveChangesAsync();

        return billingElement.Id;
    }

    public async Task UpdateBillingElement(BillingElementDto dto)
    {
        var billingElement = await Context.BillingElements.FirstByIdAsync(dto.Id);

        billingElement.IsDisburse = dto.IsDisburse;
        billingElement.IsInvoiceSpecific = dto.IsInvoiceSpecific;
        billingElement.ActivityName = dto.ActivityName;
        billingElement.Amount = dto.Amount;
        billingElement.IsHourlyRate = dto.IsHourlyRate;

        await Context.SaveChangesAsync();
    }

    public async Task RemoveBillingElement(Guid billingElementId)
    {
        var billingElement = await Context.BillingElements.FirstByIdAsync(billingElementId);
        Context.BillingElements.Remove(billingElement);
    }

    /// <returns> Path to the pdf files :) </returns>
    public async Task<byte[]> GetInvoicePdfAsBytes(Guid invoiceId)
    {
        string path = await GenerateInvoicePdf(invoiceId);

        var fileBytes = File.ReadAllBytes(path);
        return fileBytes;
    }



    public async Task<string> GetInvoicePdf(Guid invoiceId)
    {
        var path = await GenerateInvoicePdf(invoiceId);
        return path;
    }
    private async Task<string> GenerateInvoicePdf(Guid invoiceId)
    {
        var invoiceSummary = await GetInvoiceSummary(invoiceId);
        var html = await _procedureHtmlRenderer.RenderInvoiceToHtml(invoiceSummary);
        string path = await _htmlToPdfConverter.ConvertHtmlToPdf(html);
        return path;
    }

    public async Task<InvoiceSummary> GetInvoiceSummary2(CaseContextDto caseContextDto, Guid invoiceId)
    {
        // lil query just to infer the caseId from the invoiceId
        var lcase = (await Context.Invoices
            .Include(x => x.AccountStatement)
                .ThenInclude(x => x.Case)
                    .ThenInclude(x => x.ManagerLawyer)
            .FirstAsync(x => x.Id == invoiceId)
            ).AccountStatement.Case;

        var invoice = caseContextDto.GetInvoice(invoiceId);
        var caseDto = caseContextDto.Clients
            .SelectMany(x => x.Cases)
            .First(x => x.Id == lcase.Id);

        var taxableActivitiesCost = invoice.GetDisbursesTaxableTotal() + invoice.GetHourlyRatesTotal();
        var tps = taxableActivitiesCost * Taxes.TPSInPercentage;
        var tvq = taxableActivitiesCost * Taxes.TVQInPercentage;
        var taxableSubtotal = taxableActivitiesCost + tps + tvq;
        var nonTaxableActivitiesCost = invoice.GetDisbursesNonTaxableTotal();
        var total = nonTaxableActivitiesCost + taxableSubtotal;
        var paymentsTotal = invoice.GetPaymentsTotal();
        var balance = total - paymentsTotal;

        // Get the bill number
        var invoiceSummary = new InvoiceSummary
        {
            Case = caseDto,
            Invoice = invoice,
            Created = DateTime.Now,
            BillNumber = invoice.InvoiceNumber,
            Lawyer = caseDto.ManagerLawyer,
            Client = caseDto.Client,

        };

        return invoiceSummary;
    }

    // here I should be injecting the casesContext I guess
    public async Task<InvoiceSummary> GetInvoiceSummary(Guid invoiceId)
    {
        // lil query just to infer the caseId from the invoiceId
        var lcase = (await Context.Invoices
            .Include(x => x.AccountStatement)
                .ThenInclude(x => x.Case)
                    .ThenInclude(x => x.ManagerLawyer)
            .FirstAsync(x => x.Id == invoiceId)
            ).AccountStatement.Case;

        var ctx = await _caseRepository.MapCasesContext(lcase.ManagerLawyerId);
        var caseDto = ctx.Cases.First(x => x.Id == lcase.Id);
        var invoice = caseDto.Invoices.First(x => x.Id == invoiceId);

        var taxableActivitiesCost = invoice.GetDisbursesTaxableTotal() + invoice.GetHourlyRatesTotal();
        var tps = taxableActivitiesCost * Taxes.TPSInPercentage;
        var tvq = taxableActivitiesCost * Taxes.TVQInPercentage;
        var taxableSubtotal = taxableActivitiesCost + tps + tvq;
        var nonTaxableActivitiesCost = invoice.GetDisbursesNonTaxableTotal();
        var total = nonTaxableActivitiesCost + taxableSubtotal;
        var paymentsTotal = invoice.GetPaymentsTotal();
        var balance = total - paymentsTotal;

        var invoiceSummations = await GetInvoiceSummations(invoice);
        // Get the bill number
        var invoiceSummary = new InvoiceSummary
        {
            Lawyer = caseDto.ManagerLawyer,
            Case = caseDto,
            Invoice = invoice,
            Created = DateTime.Now,
            BillNumber = invoice.InvoiceNumber,
            Client = caseDto.Client,
            InvoiceSummation = invoiceSummations,
        };

        return invoiceSummary;

    }

    public async Task<InvoiceSummation> GetInvoiceSummations(InvoiceDto invoiceDto)
    {
        var taxableActivitiesCost = invoiceDto.GetDisbursesTaxableTotal() + invoiceDto.GetHourlyRatesTotal();
        var tps = taxableActivitiesCost * Taxes.TPSInPercentage;
        var tvq = taxableActivitiesCost * Taxes.TVQInPercentage;
        var taxableSubtotal = taxableActivitiesCost + tps + tvq;
        var nonTaxableActivitiesCost = invoiceDto.GetDisbursesNonTaxableTotal();
        var total = nonTaxableActivitiesCost + taxableSubtotal;
        var paymentsTotal = invoiceDto.GetPaymentsTotal();
        var balance = total - paymentsTotal;

        var invoiceSummation = new InvoiceSummation
        {
            HourlyRatesCostTotal = invoiceDto.GetHourlyRatesTotal(),
            DisbursesTaxableTotal = invoiceDto.GetDisbursesTaxableTotal(),
            DisbursesNonTaxableTotal = nonTaxableActivitiesCost,
            TaxableFeesCost = taxableActivitiesCost,
            TaxableSubtotal = taxableSubtotal,
            TPSTax = tps,
            TVQTax = tvq,
            InvoiceTotal = total,
            PaymentsTotal = paymentsTotal,
            Balance = balance,
        };
        return invoiceSummation;
    }
}
