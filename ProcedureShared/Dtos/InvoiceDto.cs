using EFCoreBase.Entities;
using ProcedureMakerServer.Billing.InvoiceDtos;
using ProcedureMakerServer.Constants;
using ProcedureShared.Dtos;
using System.Text.Json.Serialization;
namespace ProcedureMakerServer.Billing.StatementDtos;

public class InvoiceDto : EntityBase
{
    public InvoiceStatuses InvoiceStatus { get; set; } = InvoiceStatuses.InPreparation;
    public List<ActivityDto> Activities { get; set; } = [];
    public List<InvoicePaymentDto> Payments { get; set; } = [];
    public List<BillingElementDto> AvailableBillingElementsForInvoice { get; set; } = [];


    public InvoiceSummation InvoiceSummation { get; set; }
    public int InvoiceNumber { get; set; }

    public List<ActivityDto> GetDisbursesTaxable()
    {
        var disburses = this.Activities.Where(x => x.IsDisburse && x.IsTaxable).ToList();
        return disburses;
    }
    public List<ActivityDto> GetDisbursesNonTaxable()
    {
        var disbursesNonTaxable = this.Activities.Where(x => x.IsDisburse && !x.IsTaxable).ToList();
        return disbursesNonTaxable;
    }
    public List<ActivityDto> GetHourlyRateActivities()
    {
        var hourlyRate = this.Activities.Where(x => !x.IsDisburse).ToList();
        return hourlyRate;
    }
    public decimal GetDisbursesTaxableTotal()
    {
        var total = this.GetDisbursesTaxable().Sum(x => x.TotalCost);
        return total;
    }
    public decimal GetDisbursesNonTaxableTotal()
    {
        decimal total = this.GetDisbursesNonTaxable().Sum(x => x.TotalCost);
        return total;
    }
    public decimal GetHourlyRatesTotal()
    {
        decimal total = this.GetHourlyRateActivities().Sum(x => x.TotalCost);
        return total;
    }
    public decimal GetPaymentsTotal()
    {
        var total = this.Payments.Sum(x => x.AmountPaid);
        return total;
    }
    public InvoiceSummation GetInvoiceSummation()
    {
        var taxableActivitiesCost = this.GetDisbursesTaxableTotal() + this.GetHourlyRatesTotal();
        var tps = taxableActivitiesCost * Taxes.TPSInPercentage;
        var tvq = taxableActivitiesCost * Taxes.TVQInPercentage;
        var taxableSubtotal = taxableActivitiesCost + tps + tvq;
        var nonTaxableActivitiesCost = this.GetDisbursesNonTaxableTotal();
        var total = nonTaxableActivitiesCost + taxableSubtotal;
        var paymentsTotal = this.GetPaymentsTotal();
        var balance = total - paymentsTotal;

        var invoiceSummation = new InvoiceSummation
        {
            HourlyRatesCostTotal = this.GetHourlyRatesTotal(),
            DisbursesTaxableTotal = this.GetDisbursesTaxableTotal(),
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
