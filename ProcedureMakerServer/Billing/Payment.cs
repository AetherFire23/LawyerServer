using EFCoreBase.Entities;
using EFCoreBase.RefresherService;


namespace ProcedureMakerServer.Billing;


public partial class Payment : EntityBase, ICopyToAble<Payment>
{
    public Guid InvoiceId { get; set; }
    public Invoice Invoice { get; set; }

    public decimal AmountPaid { get; set; }
    public DateTime? AmountPaidDate { get; set; }

    public void CopyTo(Payment target)
    {
        target.Invoice = this.Invoice;
        target.AmountPaid = this.AmountPaid;
        target.AmountPaidDate = this.AmountPaidDate;
    }
}
