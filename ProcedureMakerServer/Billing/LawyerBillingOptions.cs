using EFCoreBase.Entities;
using ProcedureMakerServer.Entities;

namespace ProcedureMakerServer.Billing;

public class LawyerBillingOptions : EntityBase
{
    public decimal TPS { get; set; } = 0;
    public decimal TVQ { get; set; } = 0;


    public Guid LawyerId { get; set; }
    public Lawyer Lawyer { get; set; }
    

    public ICollection<BillingElement> BillingElements { get; set; } = new List<BillingElement>();

}
