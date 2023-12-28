using ProcedureMakerServer.Billing.StatementDtos;
using ProcedureShared.Dtos;
namespace ProcedureMakerServer.Billing;

public class InvoiceSummary
{
	public CaseDto Case { get; set; }
	public InvoiceDto Invoice { get; set; }

	public DateTime? Created { get; set; }
	public int BillNumber { get; set; }


	public LawyerDto Lawyer { get; set; }
	public ClientDto Client { get; set; }

	public decimal HourlyRatesTotal { get; set; }
	public decimal DisbursesTaxableTotal { get; set; }
	public decimal DisbursesNonTaxableTotal { get; set; }
	public decimal TPSTax { get; set; }
	public decimal TVQTax { get; set; }
	public decimal TaxableActivitiesCost { get; set; } // HourlyRate + disburses taxable
	public decimal Total { get; set; } = decimal.Zero; // HourlyRate + disburses taxable + disbursesnonTaxable
	public decimal PaymentsTotal { get; set; }
	public decimal Balance { get; set; } // with payments
}
