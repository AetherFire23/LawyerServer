using EFCoreBase.Entities;
using ProcedureMakerServer.Entities;

namespace ProcedureMakerServer.Trusts;



public class TrustPayment : EntityBase
{
    public Guid TrustId { get; set; }
    public Trust Trust { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }

    public TrustPaymentDto ToDto()
    {
        var payz = new TrustPaymentDto()
        {
            Id = this.Id,
            Amount = this.Amount,
            Date = this.Date,
        };
        return payz;
    }
    public void CopyFromDto(TrustPaymentDto dto, Trust trackedTrust)
    {
        this.Amount = dto.Amount;
        this.Date = dto.Date;
        this.Trust = trackedTrust;
    }
}

public class TrustDisburse : EntityBase
{
    public Guid TrustId { get; set; }
    public Trust Trust { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }

    public TrustDisburseDto ToDto()
    {
        var disburse = new TrustDisburseDto()
        {
            Id = this.Id,
            Amount = this.Amount,
            Date = this.Date,
        };
        return disburse;
    }

    public void CopyFromDto(TrustDisburseDto dto, Trust trackedTrust)
    {
        this.Amount = dto.Amount;
        this.Date = dto.Date;
        this.Trust = trackedTrust;

        bool hasChangedTrustAccount = this.TrustId != trackedTrust.Id;
        if (hasChangedTrustAccount)
        {
            this.Trust = trackedTrust;
        }
    }
}
