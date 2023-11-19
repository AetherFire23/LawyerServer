using EFCoreBase.Entities;
namespace ProcedureMakerServer.Trusts;
public class TrustDto : EntityBase
{
    public Guid ClientId { get; set; }
    public List<TrustPaymentDto> Payments { get; set; } = new();
    public List<TrustDisburseDto> Disburses { get; set; } = new();

}


public class TrustPaymentDto : EntityBase
{
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }

}

public class TrustDisburseDto : EntityBase
{
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }

    
}

