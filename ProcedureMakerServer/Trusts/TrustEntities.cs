using EFCoreBase.Entities;
using ProcedureMakerServer.Utils;

namespace ProcedureMakerServer.Trusts;



public class TrustPayment : EntityBase, ListExtensions.ICopyFromAbleDto<TrustPaymentDto>
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

    public void CopyFromDto(TrustPaymentDto dto)
    {
        Amount = dto.Amount;
        Date = dto.Date;
    }
}

public class TrustDisburse : EntityBase, ListExtensions.ICopyFromAbleDto<TrustDisburseDto>
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

    public void CopyFromDto(TrustDisburseDto dto)
    {
        this.Amount = dto.Amount;
        this.Date = dto.Date;
    }
}
