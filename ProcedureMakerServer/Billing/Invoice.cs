using EFCoreBase.Entities;

namespace ProcedureMakerServer.Billing;

public partial class Invoice : EntityBase
{
    public Guid AccountStatementId { get; set; }
    public AccountStatement? AccountStatement { get; set; }

    public InvoiceStatuses InvoiceStatuses { get; set; } = InvoiceStatuses.InPreparation;
    public ICollection<Activity> Activities { get; set; } = new List<Activity>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public void CopyTo(Invoice target)
    {
        target.InvoiceStatuses = this.InvoiceStatuses;
    }
}