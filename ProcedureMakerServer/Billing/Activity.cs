using EFCoreBase.Entities;
namespace ProcedureMakerServer.Billing;

public class Activity : EntityBase
{
    public Guid BillingElementId { get; set; }
    public BillingElement BillingElement { get; set; }

    public bool HasPersonalizedBillingElement { get; set; } = false;
    public decimal TimeWorking { get; set; } = 0;
}
