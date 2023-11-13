using EFCoreBase.Entities;
using EFCoreBase.RefresherService;

namespace ProcedureMakerServer.Billing;

public partial class Activity : EntityBase, ICopyToAble<Activity>
{
    public Guid InvoiceId { get; set; }
    public Invoice Invoice{ get; set; }

    public Guid BillingElementId { get; set; }
    public BillingElement BillingElement { get; set; }

    public bool HasPersonalizedBillingElement { get; set; } = false;
    public decimal HoursWorked { get; set; } = 0;

    public void CopyTo(Activity target)
    {
        target.HasPersonalizedBillingElement = this.HasPersonalizedBillingElement;
        HoursWorked = target.HoursWorked;
    }
}
