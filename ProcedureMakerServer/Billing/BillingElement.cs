using EFCoreBase.Entities;
using Microsoft.AspNetCore.Routing.Constraints;
using ProcedureMakerServer.Entities;

namespace ProcedureMakerServer.Billing;


// will be constant
public class BillingElement : EntityBase
{

    public Guid LawyerId { get; set; }
    public Lawyer Lawyer { get; set; }

    public string ActivityName { get; set; } = string.Empty;
    public decimal Amount { get; set; } = 0;
    public bool IsHourlyRate { get; set; } = true;
}