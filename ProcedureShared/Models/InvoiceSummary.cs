using ProcedureMakerServer.Billing.InvoiceDtos;
using ProcedureMakerServer.Billing.StatementDtos;
using ProcedureShared.Dtos;
namespace ProcedureMakerServer.Billing;




public class InvoiceSummation
{
    public decimal HourlyRatesCostTotal { get; set; }
    public decimal DisbursesTaxableTotal { get; set; }
    public decimal DisbursesNonTaxableTotal { get; set; }
    public decimal TPSTax { get; set; }
    public decimal TVQTax { get; set; }
    public decimal TaxableFeesCost { get; set; } // HourlyRate + disburses taxable
    public decimal InvoiceTotal { get; set; } = decimal.Zero; // HourlyRate + disburses taxable + disbursesnonTaxable
    public decimal PaymentsTotal { get; set; }
    public decimal Balance { get; set; } // with payments
    public decimal TaxableSubtotal { get; set; }
}


public class InvoiceSummary
{
    public CaseDto Case { get; set; }
    public InvoiceDto Invoice { get; set; }

    public List<ActivityDto> TaxableDisburses => this.Invoice.GetDisbursesTaxable();
    public List<ActivityDto> NonTaxableDisburses => this.Invoice.GetDisbursesNonTaxable();
    public List<ActivityDto> HourlyActivities => this.Invoice.GetHourlyRateActivities();

    public LawyerDto Lawyer { get; set; }
    public ClientDto Client { get; set; }

    public DateTime? Created { get; set; }
    public int BillNumber { get; set; }


    public InvoiceSummation InvoiceSummation { get; set; }

    //public decimal HourlyRatesCostTotal { get; set; }
    //public decimal DisbursesTaxableTotal { get; set; }
    //public decimal DisbursesNonTaxableTotal { get; set; }
    //public decimal TPSTax { get; set; }
    //public decimal TVQTax { get; set; }
    //public decimal TaxableFeesCost { get; set; } // HourlyRate + disburses taxable
    //public decimal InvoiceTotal { get; set; } = decimal.Zero; // HourlyRate + disburses taxable + disbursesnonTaxable
    //public decimal PaymentsTotal { get; set; }
    //public decimal Balance { get; set; } // with payments
    //public decimal TaxableSubtotal { get; set; }
}
