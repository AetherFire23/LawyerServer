using EFCoreBase.Entities;
using ProcedureMakerServer.Billing.StatementEntities;

namespace ProcedureMakerServer.Billing.InvoiceDtos;

public class ActivityEntryDto : EntityBase
{
    public string BillingElementName { get; set; } = "";
    public bool HasPersonalizedBillingElement { get; set; } = false;
    public decimal HoursWorked { get; set; } = 0;

    public void CopyTo(Activity target, BillingElement billingElement)
    {
        target.HasPersonalizedBillingElement = this.HasPersonalizedBillingElement;
        HoursWorked = target.HoursWorked;
        target.BillingElement = billingElement;
    }
}
