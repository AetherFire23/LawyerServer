using EFCoreBase.Entities;


namespace ProcedureMakerServer.Billing;

public partial class Activity : EntityBase
{
    public Guid InvoiceId { get; set; }
    public Invoice Invoice { get; set; }

    public Guid BillingElementId { get; set; }
    public BillingElement BillingElement { get; set; }

    public bool HasPersonalizedBillingElement { get; set; } = false;
    public decimal HoursWorked { get; set; } = 0;

    public ActivityDto ToDto()
    {
        var dto = new ActivityDto()
        {
            Id = this.Id,
            BillingElement = this.BillingElement.ToDto(),
            HasPersonalizedBillingElement = this.HasPersonalizedBillingElement,
            HoursWorked = this.HoursWorked
        };
        return dto;
    }
}

public class ActivityDto : EntityBase
{
    public BillingElementDto BillingElement { get; set; }
    public bool HasPersonalizedBillingElement { get; set; } = false;
    public decimal HoursWorked { get; set; } = 0;

    public void CopyTo(Activity target, BillingElement billingElement)
    {
        target.HasPersonalizedBillingElement = this.HasPersonalizedBillingElement;
        HoursWorked = target.HoursWorked;
        target.BillingElement = billingElement;

    }
}




public class ActivityCreation
{
    public Guid BillingElementId { get; set; }
    public Guid InvoiceId { get; set; }
    public bool HasPersonalizedBillingElement { get; set; } = false;
    public decimal HoursWorked { get; set; } = 0;
}

