using EFCoreBase.Entities;


namespace ProcedureMakerServer.Billing;



// Payment Type for Fideicommis ?
public partial class Payment : EntityBase
{
    public Guid InvoiceId { get; set; }
    public Invoice Invoice { get; set; } // payment cant change invoice, doesnt make sense

    public decimal AmountPaid { get; set; }
    public DateTime? AmountPaidDate { get; set; }

    public void CopyTo(Payment target)
    {
        target.Invoice = this.Invoice;
        target.AmountPaid = this.AmountPaid;
        target.AmountPaidDate = this.AmountPaidDate;
    }
}

public class PaymentCreationRequest
{
    public Guid InvoiceId { get; set; }
    public decimal AmountPaid { get; set; }
    public DateTime? AmountPaidDate { get; set; }
}