using EFCoreBase.Entities;
using ProcedureMakerServer.Billing.InvoiceDtos;
using ProcedureMakerServer.Utils;


namespace ProcedureMakerServer.Billing.StatementEntities;

public partial class Activity : EntityBase, ListExtensions.ICopyFromAbleDto<ActivityEntryDto>
{
    public Guid InvoiceId { get; set; }
    public Invoice Invoice { get; set; }

    public Guid BillingElementId { get; set; }
    public BillingElement BillingElement { get; set; }

    public bool HasPersonalizedBillingElement { get; set; } = false;
    public decimal HoursWorked { get; set; } = 0;

    public ActivityEntryDto ToDto()
    {
        var dto = new ActivityEntryDto()
        {
            Id = Id,
            BillingElementName = BillingElement.ActivityName,
            HasPersonalizedBillingElement = HasPersonalizedBillingElement,
            HoursWorked = HoursWorked
        };
        return dto;
    }

    public void CopyFromDto(ActivityEntryDto dto)
    {
        this.HoursWorked = dto.HoursWorked;
        this.HasPersonalizedBillingElement = dto.HasPersonalizedBillingElement;
    }
}

public class ActivityCreation
{
    public Guid BillingElementId { get; set; }
    public Guid InvoiceId { get; set; }
    public decimal HoursWorked { get; set; } = 0;
    public bool IsPayableByTrust { get; set; } = false;
}