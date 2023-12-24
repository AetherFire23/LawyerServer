using EFCoreBase.Entities;
using ProcedureMakerServer.Billing.StatementDtos;
using ProcedureMakerServer.Trusts;
using ProcedureMakerServer.Utils;
namespace ProcedureMakerServer.Billing.StatementEntities;

public class InvoicePayment : EntityBase
{
    public Guid InvoiceId { get; set; }
    public Invoice Invoice { get; set; }
    public decimal AmountPaid { get; set; }
    public DateTime? AmountPaidDate { get; set; }

    public bool IsPaymentComingFromTrust { get; set; } = false;
    public InvoicePayment()
    {

    }

    public InvoicePayment(Invoice trackedInvoice)
    {
        this.Invoice = trackedInvoice;
    }

    public InvoicePaymentDto ToDto()
    {
        var paymentDto = new InvoicePaymentDto()
        {
            Id = Id,
            IsPaymentComingFromTrust = this.IsPaymentComingFromTrust,
            AmoundPaidDate = this.AmountPaidDate,
            AmountPaid = this.AmountPaid,
        };
        return paymentDto;
    }

    public void CopyFromDto(InvoicePaymentDto dto)
    {
        this.AmountPaid = dto.AmountPaid;
        this.AmountPaidDate = dto.AmoundPaidDate;
        this.IsPaymentComingFromTrust = dto.IsPaymentComingFromTrust;
    }


    /// <summary>
    /// Trust disburses are mapped to invoice payments. Therefore a trust disburse is nothing but an invoicepayment with a IsTrustDisburse. 
    /// But we are creating this distinction for the client-side for clarity sake.
    /// </summary>
    public TrustDisburseDto ToTrustDisburseDto()
    {
        // TODO: 
        // specifiy for which account / case / invoice this was paid for so that we can log it
        var trustDisburse = new TrustDisburseDto
        {
            AmountPaid = this.AmountPaid,
            Date = DateTime.UtcNow,
            Id = this.Id,
        };

        return trustDisburse;
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