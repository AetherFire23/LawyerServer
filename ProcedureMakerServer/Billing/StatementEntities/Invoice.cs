using EFCoreBase.Entities;

namespace ProcedureMakerServer.Billing.StatementEntities;

public partial class Invoice : EntityBase
{
	public Guid AccountStatementId { get; set; }
	public AccountStatement AccountStatement { get; set; }
	public InvoiceStatuses InvoiceStatus { get; set; } = InvoiceStatuses.InPreparation;

	public Guid DefaultBillingElementId { get; set; }
	public BillingElement? DefaultBillingElement { get; set; }

	public ICollection<Activity> Activities { get; set; } = new List<Activity>();
	public ICollection<InvoicePayment> Payments { get; set; } = new List<InvoicePayment>();
}