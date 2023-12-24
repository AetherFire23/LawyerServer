using EFCoreBase.Entities;
using ProcedureMakerServer.Utils;

namespace ProcedureMakerServer.Trusts;



public class TrustPayment : EntityBase, ListExtensions.ICopyFromAbleDto<TrustPaymentDto>
{
    public Guid TrustId { get; set; }
    public TrustClientCard Trust { get; set; }
    public decimal Amount { get; set; }
    public DateTime? Date { get; set; }

    public TrustPaymentDto ToDto()
    {
        TrustPaymentDto payz = new TrustPaymentDto()
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

