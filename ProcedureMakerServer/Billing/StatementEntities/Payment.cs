using EFCoreBase.Entities;
using ProcedureMakerServer.Billing.StatementDtos;
using ProcedureMakerServer.Utils;
namespace ProcedureMakerServer.Billing.StatementEntities;

public class Payment : EntityBase, ListExtensions.ICopyFromAbleDto<PaymentDto>
{ 
    public Guid InvoiceId { get; set; }
    public Invoice Invoice { get; set; }
    public decimal AmountPaid { get; set; }
    public DateTime? AmountPaidDate { get; set; }

    public Payment()
    {
        
    }

    public Payment(Invoice trackedInvoice)
    {
        this.Invoice = trackedInvoice;
    }

    public PaymentDto ToDto()
    {
        var paymentDto = new PaymentDto()
        {
            Id = Id,
            AmountPaid = AmountPaid,
            AmountPaidDate = AmountPaidDate
        };
        return paymentDto;
    }

    public void CopyFromDto(PaymentDto dto)
    {
        this.AmountPaid = dto.AmountPaid;
        this.AmountPaidDate = dto.AmountPaidDate;
    }
}


public class InvoicePaymentCreationRequest
{

    public Guid ToInvoiceId { get; set; }
    public decimal AmountPaid { get; set; }
    public DateTime? AmountPaidDate { get; set; }
    public bool IsTrustPayment { get; set; }

    public string Description { get; set; } = string.Empty;
}