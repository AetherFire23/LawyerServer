using EFCoreBase.Entities;

namespace ProcedureMakerServer.Billing;

public class Payment : EntityBase
{
    public Guid InvoiceId { get; set; }
    public Invoice Invoice { get; set; }

    public decimal AmountPaid { get; set; }
    public DateTime? AmountPaidDate { get; set; }
}
