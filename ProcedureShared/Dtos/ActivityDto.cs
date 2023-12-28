using EFCoreBase.Entities;

namespace ProcedureMakerServer.Billing.InvoiceDtos;

public class ActivityDto : EntityBase
{
	public decimal Quantity { get; set; } = 0;
	public string Description { get; set; } = string.Empty;
	public decimal CostInDollars { get; set; } = 0;
	public bool IsDisburse { get; set; } = false;
	public bool IsTaxable { get; set; } = true;


	public decimal GetTotalCost()
	{
		var totalCost = this.CostInDollars * Quantity;
		return totalCost;
	}
}
