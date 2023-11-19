using EFCoreBase.Entities;
using ProcedureMakerServer.Entities;

namespace ProcedureMakerServer.Trusts;

public class Trust : EntityBase
{
    public Guid ClientId { get; set; }
    public Client Client { get; set; }
    public List<TrustPayment> Payments { get; set; } = new();
    public List<TrustDisburse> Disburses { get; set; } = new();
}
