using EFCoreBase.Entities;

namespace ProcedureMakerServer.Billing.StatementEntities;

public class InvoicePaymentDto : EntityBase
{
    public decimal AmountPaid { get; set; }
    public DateTime? AmoundPaidDate { get; set; }
    public bool IsPaymentComingFromTrust { get; set; }
}
