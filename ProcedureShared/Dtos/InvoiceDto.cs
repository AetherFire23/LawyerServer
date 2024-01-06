using EFCoreBase.Entities;
using ProcedureMakerServer.Billing.InvoiceDtos;
using ProcedureShared.Dtos;
namespace ProcedureMakerServer.Billing.StatementDtos;

public class InvoiceDto : EntityBase
{
	public InvoiceStatuses InvoiceStatus { get; set; } = InvoiceStatuses.InPreparation;
	public List<ActivityDto> Activities { get; set; } = [];
	public List<InvoicePaymentDto> Payments { get; set; } = [];
	public List<BillingElementDto> AvailableBillingElementsForInvoice { get; set; } = [];
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
		var total = this.GetDisbursesTaxable().Sum(x => x.GetTotalCost());
		return total;
	}
	public decimal GetDisbursesNonTaxableTotal()
	{
		decimal total = this.GetDisbursesNonTaxable().Sum(x => x.GetTotalCost());
		return total;
	}
	public decimal GetHourlyRatesTotal()
	{
		decimal total = this.GetHourlyRateActivities().Sum(x => x.GetTotalCost());
		return total;
	}
	public decimal GetPaymentsTotal()
	{
		var total = this.Payments.Sum(x => x.AmountPaid);
		return total;
	}

}
