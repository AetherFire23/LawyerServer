using EFCoreBase.Entities;
using ProcedureMakerServer.Billing.StatementEntities;

namespace ProcedureMakerServer.Billing.InvoiceDtos;

public class ActivityDto : EntityBase
{
    public BillingElementDto BillingElementDto { get; set; }
    public decimal HoursWorked { get; set; } = 0;
    public string ActivityDescription { get; set; } = string.Empty;
}
